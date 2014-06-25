using System;
using System.IO;

public class Program
{
  // The parser needs a constructor
  Program() { }
  
  static void Main(string[] args)
  {
    string filename = "";
    if(args.Length == 0) {
      System.Console.WriteLine("Usage: lab4 [options] [input]");
      System.Console.WriteLine("             -ast     Dump the AST after parsing.");
    }
    else {
      bool print_ast = false;
      int i;
      for(i=0; i<args.Length; ++i) {
        if(args[i][0] == '-') {
          if(args[i] == "-ast")
            print_ast = true;
          else
            System.Console.WriteLine("Unknown argument \""+args[i]+"\". Ignoring.");
        }
        else {
          filename = args[i++];
          break;
        }
      }
      if(i < args.Length)
        System.Console.WriteLine("Ignoring arguments after filename \""+filename+"\".");

      // setup the lexer
      FileStream src = File.OpenRead(filename);
      LexScanner.Scanner sc = new LexScanner.Scanner(src);

      System.Console.WriteLine("File: " + filename);
      System.Console.WriteLine("Parsing:");

      // setup the parser with the lexer as argument
      Parser parser = new Parser(sc);

      // call the parsing method
      if (!parser.Parse()) {
        Console.WriteLine("Exiting with errors.");
        return;
      }

      if(print_ast) {
        ASTWriter wrt = new ASTWriter();
        parser.SyntaxTree.Accept(wrt);
      }

      System.Console.WriteLine("\nType Checking:");

      ASTTypeCheck check = new ASTTypeCheck();
      parser.SyntaxTree.Accept(check);

      IR.IRBuilder builder = new IR.IRBuilder(filename);

      // the first pass will gather functions so that we
      // can resolve references to functions
      System.Console.WriteLine("\nIntermediate Code Generation: (Pass 1)");
      ASTIRBuildPass1 pass1 = new ASTIRBuildPass1(builder);
      parser.SyntaxTree.Accept(pass1);

      // create a module to place the code
      IR.Module module = builder.GetModule();
      uint num_funcs = module.NumFunctions;
      string plural = (num_funcs == 1) ? "" : "s";
      Console.WriteLine("    {0} function{1} discovered.",num_funcs,plural);

      System.Console.WriteLine("\nIntermediate Code Generation: (Pass 2)");
      ASTIRBuildPass2 pass2 = new ASTIRBuildPass2(builder);
      parser.SyntaxTree.Accept(pass2);

      System.Console.WriteLine("\nIntermediate Code Result:");
      module.Write();
    }
  }
}

