/*****
AST IR Building consists of two passes:
1. Discover and create functions (do not Visit their bodies).
2. Visit function bodies and generate code.

*****/
using System;
using System.Collections.Generic;
using IR;
using ASTree;

/// AST IR Build Pass 1 //////////////////////////////////////////
public class ASTIRBuildPass1 : ASTVisitor {
  IRBuilder Builder = null;
  public ASTIRBuildPass1(IRBuilder b) {Builder = b;}

  public void Visit(ASTProgram tree) {
    foreach(AST s in tree.statements) {
      s.Accept(this);
    }
  }

  public void Visit(ASTFunction tree) {
    FunctionType ft = (FunctionType)tree.type;
    Builder.GetOrCreateFunction(tree.name,ft);
  }

  public void Visit(ASTParam   tree) { }
  public void Visit(ASTType    tree) { }
  public void Visit(ASTAssign  tree) { }
  public void Visit(ASTVarDecl tree) { }
  public void Visit(ASTBlock   tree) { }
  public void Visit(ASTIf      tree) { }
  public void Visit(ASTWhile   tree) { }
  public void Visit(ASTBinExpr tree) { }
  public void Visit(ASTUnrExpr tree) { }
  public void Visit(ASTCall    tree) { }
  public void Visit(ASTReturn  tree) { }
  public void Visit(ASTIdent   tree) { }
  public void Visit(ASTInteger tree) { }
//  public void Visit(ASTFloat   tree) { }
  public void Visit(ASTString  tree) { }
  public void Visit(ASTDebug   tree) { }

}

/// AST IR Build Pass 2 //////////////////////////////////////////
public class ASTIRBuildPass2 : ASTVisitor {
  // this map keeps track of identifiers and their associated registers
  // as in the case of assignment
  Dictionary<string,Reg> NameRegMap = new Dictionary<string,Reg>();
  Oper operand;
  IRBuilder Builder = null;

  public ASTIRBuildPass2(IRBuilder b) {Builder = b;}

  public void Complain(string fmt, params object[] parms) {
    System.Console.Write("Error: ");
    System.Console.WriteLine(fmt,parms);
  }

  public void Complain(int line_number, string fmt, params object[] parms) {
    System.Console.Write("Error[{0}]: ",line_number);
    System.Console.WriteLine(fmt,parms);
  }

  Reg TryCastReg(Oper o) {
    if (o.IsReg()) return (Reg)o;
    throw new Exception(
      String.Format("TryCastRegister failed: got {0}",o));
  }
  Reg ConvertToRegister(Oper o) {
    if (o.IsReg()) return (Reg)o;
    if (o.IsImm()) {
      Reg r = Builder.CreateVirtualRegister();
      Builder.CreateMOV(r,o);
      return r;
    }
    throw new Exception(
      String.Format("ConvertToRegister failed: cannot convert {0} to register",o));
  }

  CC TryCastCC(Oper o) {
    if (o.IsCC()) return (CC)o;
    throw new Exception(
      String.Format("TryCastCC failed: got {0}",o));
  }
  
  private Reg PlaceImmOrRegIntoReg(Oper op) {
    if (op.IsReg()) return (Reg)op;
    if (op.IsImm()) {
      Reg r = Builder.CreateVirtualRegister();
      Builder.CreateMOV(r,op);
      return r;
    }
    throw new Exception("PlaceImmOrRegIntoReg: Expected Imm or Reg operand");
  }
  
  public void Visit(ASTProgram tree) {
    foreach(AST s in tree.statements) {
      s.Accept(this);
    }
  }
  public void Visit(ASTFunction tree) {
    FunctionType ty = (FunctionType)tree.type;
    Builder.BeginFunction(tree.name,ty);

    foreach (ASTParam p in tree.parameters) {
      Reg reg = Builder.CreateFunctionArgRegister();
      NameRegMap[p.name] = reg;
    }

    // a first block is automatically created by BeginFunction
    tree.body.Accept(this);

    if (tree.requires_return) {
      if (ty.ReturnType != Type.Void_t) {
        // we expect the type checker to catch missing returns when the
        // return value is non-void, but we allow a missing return if the
        // return type is void
        throw new Exception("CompilerError: ASTFunction marked as 'requires_return' but return type is not void");
      }
      Builder.CreateRET();
    }
    Builder.EndFunction();
  }

  public void Visit(ASTParam tree) {
    throw new Exception("ASTIRBuilder2::ASTParam: should not be Visited");
  }

  public void Visit(ASTType tree) {
    throw new Exception("ASTIRBuilder2::ASTType: should not be Visited");
  }

  public void Visit(ASTAssign tree) {
    Reg reg = null;
    if (!NameRegMap.TryGetValue(tree.name, out reg)) {
      throw new Exception("ASTIRBuild2::ASTAssign: Identifier "+tree.name+" does not map to a register");
    }

    tree.expr.Accept(this);
    Oper op = this.operand;
    
    Builder.CreateMOV(reg,op);
    this.operand = null;
  }

  public void Visit(ASTVarDecl tree) {
    tree.expr.Accept(this);
    Oper op = this.operand;

    Reg reg = Builder.CreateVirtualRegister();
    NameRegMap[tree.name] = reg;
    Builder.CreateMOV(reg,op);
    this.operand = null;
  }

  public void Visit(ASTReturn tree) {
    if (tree.expr != null) {
      tree.expr.Accept(this);
      Reg reg = PlaceImmOrRegIntoReg(this.operand);
      Builder.CreateRET(reg);
    }
    else
      Builder.CreateRET();
  }

  public void Visit(ASTBlock tree) {
    List<AST> statements = tree.statements;
    foreach(AST stmt in statements) {
      stmt.Accept(this);
    }

    // this.operand may or may not be a register
  }

  public void Visit(ASTIf tree) {
    ASTExpr cond = tree.cond;
    ASTBlock then_side = tree.then_side;
    ASTBlock else_side = tree.else_side;
    // FIXME: The template for an if statement looks like:
    // BBcurrent:
    //     ...
    //     cond expression
    //     BCC cc, BBthen, BBelse
    // BBthen:
    //     then_side statements
    //     BAL BBnext
    // BBelse:
    //     else_side statements
    //     BAL bbnext
    // BBnext:
    //     ...
    // -------------------------------
    // 1. evaluate the conditional expression
		cond.Accept (this);
		Oper cond_op = this.operand;
		Reg cc = TryCastReg (cond_op);
    // 2. create 3 blocks: BBthen, BBelse, BBnext
		BasicBlock BBthen = CreateBlock ();
		BasicBlock BBelse = CreateBlock ();
		BasicBlock BBnext = CreateBlock ();

    // 3. Create a BCC instruction using the condition
		Builder.CreateBCC (cc, BBthen, BBelse);
    //    expression result (a register?) two of the blocks.
    // 4. Begin BBthen and Visit each statement in then_side, ie Builder.BeginBlock(tbb);
		//note: this is not handling a return statement inside a block.  In that case it shouldn't always branch!
		Builder.BeginBlock (BBthen);
		then_side.Accept (this);
		Builder.CreateBAL (BBnext);

		Builder.BeginBlock (BBelse);
		then_side.Accept (this);
		Builder.CreateBAL (BBnext);
    // 5. Now, branch always to BBnext
		Builder.BeginBlock (BBnext);
    // 6. we're not done yet ...

//    cond.Accept(this);
//    Oper cond_op = this.operand; // should be a condition code CC
//    Reg cc = TryCastReg(cond_op); // it might be an immediate value so force it into a register

//    throw new Exception("ASTIRBuild::Visit(ASTIf): not implemented");
    
    // statements do not return anything
    this.operand = null;
  }

  public void Visit(ASTWhile tree) {
    ASTExpr cond = tree.cond;
    ASTBlock body = tree.body;
    // FIXME: The template for the while statement looks something like this:
    // BBcurrent: ...
    //     BAL BBheader
    // BBheader:
    //     Va = conditional expression                Visit cond
    //     BCC Va, BBloop, BBpost                     
    // BBloop:
    //     loop statements
    //     BAL BBheader
    // BBpost:
    //     ...
		cond.Accept (this);
		Oper cond_op = this.operand;
		Reg cc = TryCastReg (cond_op);
		// 2. create 3 blocks: BBthen, BBelse, BBnext
		BasicBlock BBnext = CreateBlock ();
		BasicBlock BBcurrent = BeginBlock ();

		Builder.CreateBCC (cc, BBcurrent, BBnext);



    throw new Exception("ASTIRBuild::Visit(ASTWhile): not implemented");

    // statements do not return anything
    this.operand = null;
  }

  public void Visit(ASTBinExpr tree) {
    // Visit the left hand side to discover its type
    tree.lhs.Accept(this);
//    Reg lhs = TryCastReg(this.operand);
    Reg lhs = PlaceImmOrRegIntoReg(this.operand);

    tree.rhs.Accept(this);
    Oper rhs_op = this.operand;
    Reg rhs = PlaceImmOrRegIntoReg(rhs_op);

    switch(tree.oper) {
      case '+': {
        // FIXME: create an ADD instruction:
				this.operand = Builder.CreateADD(lhs, rhs);
				// throw new Exception("ASTIRBuild::Visit(ASTBinExpr): '-' not implemented");
        break;
      }
      case '-': {
				this.operand = Builder.CreateSUB (lhs, rhs);
				//throw new Exception("ASTIRBuild::Visit(ASTBinExpr): '-' not implemented");
        break;
      }
      case '*': {
				this.operand = Builder.CreateMUL (lhs, rhs);
//        throw new Exception("ASTIRBuild::Visit(ASTBinExpr): '*' not implemented");
        break;
      }
		case '=': {//CC.ty cc, Reg lhs, Oper rhs
				this.operand = Builder.CreateCMP (tree.oper, lhs, rhs);
        // FIXME: create a CMP instruction:
//        throw new Exception("ASTIRBuild::Visit(ASTBinExpr): '=' not implemented");
        break;
      }
      case '<': {
//        throw new Exception("ASTIRBuild::Visit(ASTBinExpr): '<' not implemented");
        break;
      }
      default:
        throw new Exception("Not a valid binary operator '"+tree.oper+"'");
    }
  }

  public void Visit(ASTUnrExpr tree) {
    throw new Exception("ASTUnrExpr is not implemented.");
/*    // Visit the right hand side to discover its type
    tree.rhs.Accept(this);

    char oper = tree.oper;
    switch(oper) {
      case '-': {
        // FIXME: negation works for what types ?
        if (tree.rhs.type == Type.Int_t || tree.rhs.type == Type.Float_t) {
          // FIXME: you probably don't want Void_t here
          tree.type = tree.rhs.type;
        }
        else {
          complain(tree.line_number,
            "incompatible types to unary operator '-': "+tree.rhs.type);
        }
        break;
      }
      default:
        throw new Exception("Not a valid unary operator '"+oper+"'");
    }*/
  }

  public void Visit(ASTCall tree) {
    List<Oper> args = new List<Oper>();
    List<Type> arg_ts = new List<Type>();
    // Visit the arguments to discover their types
    foreach(AST x in tree.arguments) {
      x.Accept(this);
      args.Add(this.operand);
      arg_ts.Add(x.type);
    }

    Function fun = Builder.GetFunction(tree.name, arg_ts);

    if (fun == null) {
      throw new Exception("Function "+tree.name+" does not exist.");
    }
    
    this.operand = Builder.CreateCALL(fun,args);
  }

  // Identifiers
  public void Visit(ASTIdent tree) {
    Reg reg = null;
    if (!NameRegMap.TryGetValue(tree.name, out reg)) {
      throw new Exception("Identifier "+tree.name+" has not been declared");
    }
    this.operand = reg;
  }

  // Basic Values
  public void Visit(ASTInteger tree) {
    int value = int.Parse(tree.value);
    
		this.operand = Builder.CreateImmediate (value);
  }
/*
  public void Visit(ASTFloat tree) {
    // FIXME: what does this node get set to ? It's not void
    tree.type = Type.Float_t;
  }
*/
  public void Visit(ASTString tree) {
    operand = Builder.CreateString(tree.value);
  }

  // Debug Node

  public void Visit(ASTDebug tree) { }

}
