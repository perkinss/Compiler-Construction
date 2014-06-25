using System.Collections.Generic;

namespace ASTree
{
  public interface ASTVisitor
  {
    void Visit(ASTProgram  tree);
    void Visit(ASTFunction tree);
    void Visit(ASTVarDecl  tree);
    void Visit(ASTAssign   tree);
    void Visit(ASTParam    tree);
    void Visit(ASTType     tree);
    void Visit(ASTBlock    tree);
    void Visit(ASTIf       tree);
    void Visit(ASTReturn   tree);
    void Visit(ASTWhile    tree);
    void Visit(ASTBinExpr  tree);
    void Visit(ASTUnrExpr  tree);
    void Visit(ASTCall     tree);
    void Visit(ASTIdent    tree);
    void Visit(ASTInteger  tree);
    void Visit(ASTString   tree);
    void Visit(ASTDebug    tree);
  }
  
  /// Abstract Syntax Tree

  public abstract class AST {
    public int line_number;
    public Type type;
    public abstract void Accept(ASTVisitor v);
  }

  public class ASTProgram : AST {
    public List<AST> statements = new List<AST>();
    public ASTProgram() { }
    public void Add(AST s) {
      statements.Add(s);
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }

  public class ASTFunction : AST {
    public string name;
    public List<ASTParam> parameters;
    public ASTBlock body;
    public ASTType return_type;
    public bool requires_return;
    public ASTFunction(ASTIdent n, List<ASTParam> ps, ASTType t, ASTBlock b) {
      name = n.name;
      parameters = ps;
      body=b;
      return_type = t;
      requires_return = false;
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }

  public class ASTIf : AST {
    public string name;
    public ASTExpr cond;
    public ASTBlock then_side;
    public ASTBlock else_side;
    public ASTIf(ASTExpr cond, ASTBlock lhs, ASTBlock rhs) {
      this.cond = cond;
      then_side = lhs;
      else_side = rhs;
    }
    public override void Accept(ASTVisitor v) {v.Visit(this);}
  }

  public class ASTWhile : AST {
    public ASTExpr cond;
    public ASTBlock body;
    public ASTWhile(ASTExpr cond, ASTBlock body) {
      this.cond = cond;
      this.body = body;
    }
    public override void Accept(ASTVisitor v) {v.Visit(this);}
  }

  public class ASTAssign : AST {
    public string name;
    public ASTExpr expr;
    public ASTAssign(ASTIdent n, ASTExpr e) {
      name = n.name;
      expr = e;
    }
    public override void Accept(ASTVisitor v) {v.Visit(this);}
  }

  public class ASTVarDecl : AST {
    public string name;
    public ASTExpr expr;
    public ASTVarDecl(ASTIdent n, ASTExpr e) {
      name = n.name; expr = e;
    }
    public override void Accept(ASTVisitor v) {v.Visit(this);}
  }

  public class ASTReturn : AST {
    public ASTExpr expr; // might be null
    public ASTReturn() : base() {expr = null;}
    public ASTReturn(ASTExpr ex) : base() {expr = ex;}
    public override void Accept(ASTVisitor v) {v.Visit(this);}
  }

  public class ASTParam : AST {
    public string name;
    public ASTType param_type;
    public ASTParam(string id, ASTType t) {
      name = id; param_type = t;
    }
    public override void Accept(ASTVisitor v) {v.Visit(this);}
  }

  public class ASTType : AST {
    public string name;
    public ASTType(string t) {name = t;}
    public override void Accept(ASTVisitor v) {v.Visit(this);}
  }

  public class ASTBlock : AST {
    public List<AST> statements;
    public ASTBlock() {statements=new List<AST>();}
    public ASTBlock(List<AST> xs) {statements = xs;}
    public override void Accept(ASTVisitor v) {v.Visit(this);}
  }

  public abstract class ASTExpr : AST {
    
  }

  public class ASTBinExpr : ASTExpr {
    public ASTExpr lhs;
    public char oper;
    public ASTExpr rhs;
    public ASTBinExpr(ASTExpr l, char op, ASTExpr r) {
      lhs = l;
      oper = op;
      rhs = r;
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }

  public class ASTUnrExpr : ASTExpr {
    public char oper;
    public ASTExpr rhs;
    public ASTUnrExpr(char op, ASTExpr r) {
      oper = op;
      rhs = r;
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }

  public class ASTCall : ASTExpr {
    public string name;
    public List<ASTExpr> arguments;
    public ASTCall(ASTIdent n, List<ASTExpr> xs) {
      name = n.name;
      arguments = xs;
      line_number = n.line_number;
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }

  public class ASTIdent : ASTExpr {
    public string name;
    public ASTIdent(int line, string n) {
      line_number=line;
      name=n;
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }

  public class ASTInteger : ASTExpr {
    public string value;
    public ASTInteger(int line, string v) {
      line_number=line;
      value=v;
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }

  public class ASTString : ASTExpr {
    public string value;
    public ASTString(int line, string v) {
      line_number=line;
      value=v;
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }

  public class ASTDebug : ASTExpr {
    public string kind;
    public ASTDebug(int line, string kind) {
      this.kind = kind;
      line_number = line;
    }
    public override void Accept(ASTVisitor v) {
      v.Visit(this);
    }
  }
}

