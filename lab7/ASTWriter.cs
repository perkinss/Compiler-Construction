using System;
using System.IO;
using System.Collections.Generic;
using ASTree;


public class ASTWriter : ASTVisitor
{
  protected static string fulltab = "        ";
  protected static string halftab = "    ";

  string indent = "";

  public void complain(int line_number, string message) {
    System.Console.WriteLine("Error["+line_number+"]: "+message);
  }
  
  protected void IndentSubtree(string indentation, AST tree) {
      string saved_indent = indentation;

    if(tree.GetType() == typeof(ASTInteger)) {
      Console.Write(" ");
      indent = "";
      tree.Accept(this);
    }
    else if(tree.GetType() == typeof(ASTIdent)) {
      Console.Write(" ");
      indent = "";
      tree.Accept(this);
    }
    else {
      Console.WriteLine("");
      indent = indent + fulltab;
      tree.Accept(this);
    }
    indent = saved_indent;
  }
  
  public void VisitStmts(string name, List<AST> statements) {
    Console.Write("{0}{1}: [",indent+halftab,name);

    if(statements.Count == 0)
      Console.WriteLine(" ]");
    else {
      string saved_indent = indent;
      indent = indent + fulltab;

      Console.WriteLine("");
      foreach(AST x in statements) {
        x.Accept(this);
        Console.WriteLine(",");
      }
      indent = saved_indent;
      Console.WriteLine("{0}]",indent+halftab);
    }
  }

  public void Visit(ASTProgram tree) {
    List<AST> statements = tree.statements;

    Console.WriteLine("{0}<ASTProgram: [",indent);
    string saved_indent = indent;
    indent = indent + fulltab;
    foreach (AST stmt in statements) {
      stmt.Accept(this);
      Console.WriteLine(",");
    }
    indent = saved_indent;
    Console.WriteLine("{0}]>",indent);
  }

  public void Visit(ASTFunction tree) {
    string name = tree.name;
    List<ASTParam> parameters = tree.parameters;
    ASTBlock body = tree.body;

    Console.WriteLine("{0}<ASTFunction:",indent);
    Console.WriteLine("{0}name: {1}",indent+halftab,name);
    Console.Write("{0}parameters: [ ",indent+halftab);
    string saved_indent = indent;
    indent = indent+fulltab;
    foreach (AST p in parameters) {
      p.Accept(this);
    }
    indent = saved_indent;
    Console.WriteLine("]\n{0}body:",indent+halftab);
    IndentSubtree(indent,body);
//    string saved_indent = indent;
//    indent = indent + fulltab;
//    body.Accept(this);
//    indent = saved_indent;
    Console.Write(">");
  }

  public void Visit(ASTType tree) {
    string name = tree.name;
    Console.Write("<ASTType: name:\"{0}\">",name);
  }
  
  public void Visit(ASTParam tree) {
    string name = tree.name;
    ASTType param_type = tree.param_type;
    
    Console.Write("{0}<ASTParam: name:{1}, ",indent,name);
    param_type.Accept(this);
    Console.Write(">");
  }

  public void Visit(ASTReturn tree) {
    Console.Write("{0}<ASTReturn:",indent);
    if (tree.expr != null) {
      Console.WriteLine();
      Console.Write("{0}expr:",indent+halftab);
      IndentSubtree(indent,tree.expr);
    }
    Console.Write(">");
  }

  public void Visit(ASTIf tree) {
    Console.WriteLine("{0}<ASTIf:",indent);
    Console.Write("{0}cond:",indent+halftab);
    IndentSubtree(indent,tree.cond);
    IndentSubtree(indent,tree.then_side);
    IndentSubtree(indent,tree.else_side);
    Console.Write(">");
  }

  public void Visit(ASTWhile tree) {
    Console.WriteLine("{0}<ASTWhile:",indent);
    Console.Write("{0}cond:",indent+halftab);
    IndentSubtree(indent,tree.cond);
    IndentSubtree(indent,tree.body);
    Console.Write(">");
  }

  public void Visit(ASTAssign tree) {
    string name = tree.name;
    AST expr = tree.expr;
    
    Console.WriteLine("{0}<ASTAssign:",indent);
    Console.WriteLine("{0}name: {1}",indent+halftab,name);
    Console.Write("{0}expr:",indent+halftab);
    IndentSubtree(indent,expr);
//    string saved_indent = indent;
//    indent = indent + fulltab;

//    expr.Accept(this);
//    indent = saved_indent;

    Console.Write(">");
  }

  public void Visit(ASTVarDecl tree) {
    string name = tree.name;
    AST expr = tree.expr;
    
    Console.WriteLine("{0}<ASTVarDecl:",indent);
    Console.WriteLine("{0}name: {1}",indent+halftab,name);
    Console.Write("{0}expr:",indent+halftab);
    IndentSubtree(indent,expr);
//    string saved_indent = indent;
//    indent = indent + fulltab;

//    expr.Accept(this);
//    indent = saved_indent;

    Console.Write(">");
  }

  public void Visit(ASTBlock tree) {
    List<AST> statements = tree.statements;
    Console.WriteLine("{0}<ASTBlock:\n",indent);
    VisitStmts("stmts",statements);
    Console.Write(">");
  }

  public void Visit(ASTBinExpr tree) {
    char oper = tree.oper;
    AST lhs = tree.lhs;
    AST rhs = tree.rhs;
    
    Console.WriteLine("{0}<ASTBinExpr: oper:",indent);
    Console.WriteLine("{0}oper: {1}",indent+halftab,oper);
    Console.Write("{0}lhs:",indent+halftab);
    IndentSubtree(indent,lhs); // lhs.Accept(this);

    Console.Write("\n{0}rhs:",indent+halftab);
    IndentSubtree(indent,rhs); // rhs.Accept(this);

    Console.Write(">");
  }

  public void Visit(ASTUnrExpr tree) {
    char oper = tree.oper;
    AST rhs = tree.rhs;

    Console.WriteLine("{0}<ASTUnrExpr: oper:",indent);
    Console.WriteLine("{0}oper: {1}",indent+halftab,oper);
    Console.Write("{0}rhs:",indent+halftab);
//    string saved_indent = indent;
//    indent = indent+fulltab;
    
    IndentSubtree(indent,rhs); // rhs.Accept(this);
    
//    indent = saved_indent;
    Console.Write(">");
  }

  public void Visit(ASTCall tree) {
    string name = tree.name;
    List<ASTExpr> arguments = tree.arguments;

    Console.WriteLine("{0}<ASTCall: name:{1}\n",indent,name);
    Console.Write("{0}args: [",indent+halftab);
    if(arguments.Count > 0) {
      Console.WriteLine("");
      string saved_indent = indent;
      indent = indent+fulltab;
      foreach (ASTExpr arg in arguments) {
        arg.Accept(this);
        Console.WriteLine(",");
      }
      indent = saved_indent;
    }
    Console.Write("{0}]>",indent+halftab);
  }

  public void Visit(ASTIdent tree) {
    string name = tree.name;
    Console.Write("{0}<ASTIdent: \"{1}\">",indent,name);
  }

  public void Visit(ASTInteger tree) {
    string value = tree.value;
    Console.Write("{0}<ASTInteger: {1}>",indent,value);
  }
/*
  public void Visit(ASTFloat tree) {
    string value = tree.value;
    Console.Write("{0}<ASTFloat: {1}>",indent,value);
  }
*/
  public void Visit(ASTString tree) {
    string value = tree.value;
    Console.Write("{0}<ASTString: {1}>",indent,value);
  }

  public void Visit(ASTDebug tree) {
    string kind = tree.kind;
    int line_number = tree.line_number;
    Console.Write("{0}<ASTDebug: kind:{1}, line:{2}>",
      indent,kind,line_number);
  }
}
