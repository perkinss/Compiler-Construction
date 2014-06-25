using System;
using System.Collections.Generic;
using ASTree;

public class ASTTypeCheck : ASTVisitor {
  SymbolTable symbols = new SymbolTable();
  Type fn_return_type;
  bool has_return_statement;

  public string SymbolsToString() {
    return symbols.ToString();
  }
  
  public void complain(int line_number, string message) {
    System.Console.WriteLine("Error["+line_number+"]: "+message);
  }

  public void Visit(ASTProgram tree) {
    foreach(AST s in tree.statements) {
      s.Accept(this);
    }
  }
  public void Visit(ASTFunction tree) {
    ASTType return_type = tree.return_type;
    List<ASTParam> parameters = tree.parameters;
    AST body = tree.body;

    // Visit the return type subtree, which will decorate with a type
    return_type.Accept(this);
    if (return_type.type == null) {
      throw new Exception("Function return type is null");
    }
    // Visit the parameters and collect parameter types
    List<Type> param_types = new List<Type>();
    foreach(ASTParam t in parameters) {
      t.Accept(this);
      if (t.type == null) {
        throw new Exception("Argument has null type");
      }
      param_types.Add(t.type);
    }
   
    FunctionType ft = FunctionType.Get(tree.return_type.type,param_types);

    // save it in this tree node
    tree.type = ft;
    fn_return_type = ft.ReturnType;

    symbols.Binding(tree.name, ft);

    symbols.EnterScope();

    foreach (ASTParam p in parameters) {
      symbols.Binding(p.name,p.type);
    }

    has_return_statement = false;

    // now Visit the body of the function
    body.Accept(this);

    if (fn_return_type != Type.Void_t && !has_return_statement) {
        throw new Exception(
          String.Format("Not all code paths return a value (type {0})",
            fn_return_type.ToString()));
    }

    tree.requires_return = !has_return_statement;
    
    symbols.ExitScope();

    fn_return_type = null;
  }

  public void Visit(ASTParam tree) {
    tree.param_type.Accept(this);
    // save the type in the tree node 
    tree.type = tree.param_type.type;
  }

  public void Visit(ASTType tree) {
    Type t = Type.Get(tree.name);
    if (t == null) {
      throw new Exception(
        String.Format("name '{0}' is not a valid type name",tree.name));
    }
    tree.type = t;
  }

  public void Visit(ASTIf tree) {
    tree.cond.Accept(this);
    if (tree.cond.type != Type.Int_t) {
      throw new Exception(
        String.Format(
          "If statement conditional must evaluate to type int not {0}",
          tree.cond.type));
    }

    has_return_statement = false;
    tree.then_side.Accept(this);
    bool ht = has_return_statement;

    has_return_statement = false;
    tree.else_side.Accept(this);

    has_return_statement = has_return_statement && ht;
  }

  public void Visit(ASTWhile tree) {
    tree.cond.Accept(this);
    if (tree.cond.type != Type.Int_t) {
      throw new Exception(
        String.Format(
          "While statement conditional must evaluate to type int not {0}",
          tree.cond.type));
    }

    tree.body.Accept(this);
  }

  public void Visit(ASTReturn tree) {
    Type rt = Type.Void_t;

    if (tree.expr != null) {
      tree.expr.Accept(this);
      rt = tree.expr.type;
    }
    if (rt != fn_return_type) {
      throw new Exception(
        String.Format("expected return type of {0} but got {1}",
          fn_return_type,rt));
    }
    has_return_statement = true;
  }

  public void Visit(ASTAssign tree) {
    string name = tree.name;

    // Determine if the name is already in use somewhere
    Symbol sym = symbols.LookUp(name);
    if (sym == null) {
      complain(tree.line_number,"the name "+name+" has not been defined");
    }

    tree.expr.Accept(this);

    if (tree.expr.type != sym.type) {
      complain(tree.line_number,
        String.Format(
        "attempting to assign type {0} to variable '{1}' of type {2}",
        tree.expr.type,sym.name,sym.type));
    }
  }

  public void Visit(ASTVarDecl tree) {

    // Determine if the name is already in use somewhere
    Symbol sym = symbols.LookUp(tree.name);
    if (sym != null) {
      complain(tree.line_number,"the name "+tree.name+" is already in use");
    }

    tree.expr.Accept(this);

    symbols.Binding(tree.name, tree.expr.type);
  }

  public void Visit(ASTBlock tree) {
    List<AST> statements = tree.statements;

    symbols.EnterScope();

    foreach(AST x in statements) {
      x.Accept(this);
    }

    symbols.ExitScope();
  }

  public void Visit(ASTBinExpr tree) {
    ASTExpr lhs = tree.lhs;
    // Visit the left hand side to discover its type
    tree.lhs.Accept(this);
    Type lhs_t = lhs.type;

    ASTExpr rhs = tree.rhs;
    tree.rhs.Accept(this);
    Type rhs_t = rhs.type;

    char oper = tree.oper;
    switch(oper) {
      case '+':
      case '-':
      case '*': {
        // FIXME: add if statement checks on lhs_t and rhs_t
        //        for example: lhs_t.Is____()
        //        also: lhs_t.Equals(rhs_t)
        if (lhs_t == Type.Int_t && rhs_t == Type.Int_t) {
          tree.type = Type.Int_t;
        }
        else {
          complain(tree.line_number,"incompatible types to op"+oper+" : "+lhs_t+" and "+rhs_t);
        }
        break;
      }
      case '=': {
        if(lhs_t.Equals(rhs_t)) {
          tree.type = lhs_t;
        }
        else {
          complain(tree.line_number,"incompatible types to op"+oper+" : "+lhs_t+" and "+rhs_t);
        }
        break;
      }
      case '<': {
        if(lhs_t.Equals(rhs_t)) {
          // FIXME: you probably don't want Void_t here
          tree.type = Type.Int_t;
        }
        else {
          complain(tree.line_number,"incompatible types to op"+oper+" : "+lhs_t+" and "+rhs_t);
        }
        break;
      }
      default:
        throw new Exception("Not a valid unary operator '"+oper+"'");
    }
  }

  public void Visit(ASTUnrExpr tree) {
    // Visit the right hand side to discover its type
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
    }
  }

  public void Visit(ASTCall tree) {
    string name = tree.name;
    List<ASTExpr> arguments = tree.arguments;

    // we will put the arg types into the following list 
    List<Type> arg_ts = new List<Type>();
    // Visit the arguments to discover their types
    foreach(AST x in arguments) {
      x.Accept(this);
      if (x.type == null) {
        throw new Exception("Call argument has null type");
      }
      arg_ts.Add(x.type);
    }

    // look up in the symbol table for the function
    Symbol sym = symbols.LookUp(name);
    if(sym == null){
      complain(tree.line_number,"Unknown function '"+name+"'");
      tree.type = Type.Void_t;
      return;
    }

    Type sym_t = sym.type;

    // If it's not a function then there is a problem
    if(!sym_t.IsFunction()) {
      complain(tree.line_number,"'"+name+" is not a function, has type: "+sym_t);
      tree.type = Type.Void_t;
      return;
    }

    FunctionType fun_t = (FunctionType)sym_t;
    // check the argument types against the symbol type 
    if( ! fun_t.CompareParameters(arg_ts) ) {
      string ss = arg_ts.ToString();
      complain(tree.line_number,"incompatible argument types for function: "+ss);
    }
    else {
      tree.type = fun_t.ReturnType;
    }
  }

  // Identifiers
  public void Visit(ASTIdent tree) {
    // lookup the name in the symbol table so we can get its type
    Symbol s = symbols.LookUp(tree.name);
    if(s == null){
      complain(tree.line_number,"Undefined name "+tree.name);
      tree.type = Type.Void_t;
    }
    else
      tree.type = s.type;
  }

  // Basic Values
  public void Visit(ASTInteger tree) {
    // FIXME: what does this node get set to ? It's not void
    tree.type = Type.Int_t;
  }
/*
  public void Visit(ASTFloat tree) {
    // FIXME: what does this node get set to ? It's not void
    tree.type = Type.Float_t;
  }
*/
  public void Visit(ASTString tree) {
    // FIXME: what does this node get set to ? It's not void
    tree.type = Type.String_t;
  }

  // Debug Node

  public void Visit(ASTDebug tree) {
    // this node exists solely for debugging this exercise 
    System.Console.WriteLine("Debug["+tree.line_number+"]:");
    System.Console.WriteLine(symbols);
  }

}
