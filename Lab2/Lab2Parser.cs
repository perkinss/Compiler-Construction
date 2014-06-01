// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2010
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.0
// Machine:  Catherby
// DateTime: 01/06/2014 10:32:41 AM
// UserName: tlavallee
// Input file <Lab2Parser.y - 28/05/2014 1:07:59 PM>

// options: lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using System.IO;

public enum Tokens {error=95,EOF=96,
    NUM=97,IDENT=98,ASSIGN=99,HEX=100,UMINUS=101,OROR=102};

// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<int,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
public class ScanObj {
  public int token;
  public int yylval;
  public LexLocation yylloc;
  public ScanObj( int t, int val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

public class Parser: ShiftReduceParser<int, LexLocation>
{
  // Verbatim content from Lab2Parser.y - 28/05/2014 1:07:59 PM
#line 23 "Lab2Parser.y"
  public void yyerror( string format, params Object[] args ) {
#line 24 "Lab2Parser.y"
    Console.Write("{0}: ", 99);//LineNumber);
#line 25 "Lab2Parser.y"
    Console.WriteLine(format, args);
#line 26 "Lab2Parser.y"
  }
  // End verbatim content from Lab2Parser.y - 28/05/2014 1:07:59 PM

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[19];
  private static State[] states = new State[26];
  private static string[] nonTerms = new string[] {
      "StatementList", "$accept", "Statement", "Expr", "Ident", "Factor", "Atom", 
      };

  static Parser() {
    states[0] = new State(-3,new int[]{-1,1});
    states[1] = new State(new int[]{96,2,98,21,97,24,100,25,10,-5},new int[]{-3,3,-4,5,-5,18,-6,22,-7,23});
    states[2] = new State(-1);
    states[3] = new State(new int[]{10,4});
    states[4] = new State(-2);
    states[5] = new State(new int[]{43,6,45,8,42,10,47,12,37,14,102,16,10,-4});
    states[6] = new State(new int[]{98,21,97,24,100,25},new int[]{-4,7,-5,18,-6,22,-7,23});
    states[7] = new State(new int[]{43,-6,45,-6,42,10,47,12,37,14,102,16,10,-6});
    states[8] = new State(new int[]{98,21,97,24,100,25},new int[]{-4,9,-5,18,-6,22,-7,23});
    states[9] = new State(new int[]{43,-7,45,-7,42,10,47,12,37,14,102,16,10,-7});
    states[10] = new State(new int[]{98,21,97,24,100,25},new int[]{-4,11,-5,18,-6,22,-7,23});
    states[11] = new State(new int[]{43,-8,45,-8,42,-8,47,-8,37,-8,102,16,10,-8});
    states[12] = new State(new int[]{98,21,97,24,100,25},new int[]{-4,13,-5,18,-6,22,-7,23});
    states[13] = new State(new int[]{43,-9,45,-9,42,-9,47,-9,37,-9,102,16,10,-9});
    states[14] = new State(new int[]{98,21,97,24,100,25},new int[]{-4,15,-5,18,-6,22,-7,23});
    states[15] = new State(new int[]{43,-10,45,-10,42,-10,47,-10,37,-10,102,16,10,-10});
    states[16] = new State(new int[]{98,21,97,24,100,25},new int[]{-4,17,-5,18,-6,22,-7,23});
    states[17] = new State(-11);
    states[18] = new State(new int[]{61,19,43,-17,45,-17,42,-17,47,-17,37,-17,102,-17,10,-17});
    states[19] = new State(new int[]{98,21,97,24,100,25},new int[]{-4,20,-5,18,-6,22,-7,23});
    states[20] = new State(new int[]{43,6,45,8,42,10,47,12,37,14,102,16,10,-12});
    states[21] = new State(-18);
    states[22] = new State(-13);
    states[23] = new State(-14);
    states[24] = new State(-15);
    states[25] = new State(-16);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,96});
    rules[2] = new Rule(-1, new int[]{-1,-3,10});
    rules[3] = new Rule(-1, new int[]{});
    rules[4] = new Rule(-3, new int[]{-4});
    rules[5] = new Rule(-3, new int[]{});
    rules[6] = new Rule(-4, new int[]{-4,43,-4});
    rules[7] = new Rule(-4, new int[]{-4,45,-4});
    rules[8] = new Rule(-4, new int[]{-4,42,-4});
    rules[9] = new Rule(-4, new int[]{-4,47,-4});
    rules[10] = new Rule(-4, new int[]{-4,37,-4});
    rules[11] = new Rule(-4, new int[]{-4,102,-4});
    rules[12] = new Rule(-4, new int[]{-5,61,-4});
    rules[13] = new Rule(-4, new int[]{-6});
    rules[14] = new Rule(-6, new int[]{-7});
    rules[15] = new Rule(-7, new int[]{97});
    rules[16] = new Rule(-7, new int[]{100});
    rules[17] = new Rule(-7, new int[]{-5});
    rules[18] = new Rule(-5, new int[]{98});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // StatementList -> StatementList, Statement, '\n'
#line 32 "Lab2Parser.y"
{writeln(";----");}
        break;
      case 6: // Expr -> Expr, '+', Expr
#line 40 "Lab2Parser.y"
{writeln("add");}
        break;
      case 7: // Expr -> Expr, '-', Expr
#line 41 "Lab2Parser.y"
{writeln("sub");}
        break;
      case 8: // Expr -> Expr, '*', Expr
#line 42 "Lab2Parser.y"
{writeln("mul");}
        break;
      case 9: // Expr -> Expr, '/', Expr
#line 43 "Lab2Parser.y"
{writeln("div");}
        break;
      case 10: // Expr -> Expr, '%', Expr
#line 44 "Lab2Parser.y"
{writeln("mod");}
        break;
      case 11: // Expr -> Expr, OROR, Expr
#line 45 "Lab2Parser.y"
{writeln("oror");}
        break;
      case 12: // Expr -> Ident, '=', Expr
#line 46 "Lab2Parser.y"
{writeln("eq");}
        break;
      case 15: // Atom -> NUM
#line 52 "Lab2Parser.y"
{writeln("ldc",token_text());}
        break;
      case 16: // Atom -> HEX
#line 53 "Lab2Parser.y"
{writeln("ldh",token_text());}
        break;
      case 18: // Ident -> IDENT
#line 57 "Lab2Parser.y"
{push_id();writeln("ldi",pop_id());}
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

#line 62 "Lab2Parser.y"
Stack<string> id_stack = new Stack<string>();
#line 63 "Lab2Parser.y"

#line 64 "Lab2Parser.y"
void push_id() {
#line 65 "Lab2Parser.y"
	string t = ((LexScanner.Scanner)Scanner).last_token_text;
#line 66 "Lab2Parser.y"
	id_stack.Push(t);
#line 67 "Lab2Parser.y"
}
#line 68 "Lab2Parser.y"
string pop_id() {
#line 69 "Lab2Parser.y"
	return id_stack.Pop();
#line 70 "Lab2Parser.y"
}
#line 71 "Lab2Parser.y"

#line 72 "Lab2Parser.y"
string token_text() {
#line 73 "Lab2Parser.y"
	return ((LexScanner.Scanner)Scanner).last_token_text;
#line 74 "Lab2Parser.y"
}
#line 75 "Lab2Parser.y"

#line 76 "Lab2Parser.y"
void writeln() {
#line 77 "Lab2Parser.y"
	writeln(null,null);
#line 78 "Lab2Parser.y"
}
#line 79 "Lab2Parser.y"
void writeln(string opcode) {
#line 80 "Lab2Parser.y"
	writeln(opcode,null);
#line 81 "Lab2Parser.y"
}
#line 82 "Lab2Parser.y"

#line 83 "Lab2Parser.y"
void writeln(string opcode, string value) {
#line 84 "Lab2Parser.y"
	if (opcode != null) {
#line 85 "Lab2Parser.y"
		System.Console.Write(opcode);
#line 86 "Lab2Parser.y"
		if (value != null) {
#line 87 "Lab2Parser.y"
			System.Console.Write(' '+value);
#line 88 "Lab2Parser.y"
		}
#line 89 "Lab2Parser.y"
	}
#line 90 "Lab2Parser.y"
	System.Console.Write('\n');
#line 91 "Lab2Parser.y"
}
#line 92 "Lab2Parser.y"

#line 93 "Lab2Parser.y"
// The parser needs a constructor
#line 94 "Lab2Parser.y"
Parser() : base(null) { }
#line 95 "Lab2Parser.y"

#line 96 "Lab2Parser.y"
static void Main(string[] args)
#line 97 "Lab2Parser.y"
{
#line 98 "Lab2Parser.y"
	Parser parser = new Parser();
#line 99 "Lab2Parser.y"

#line 100 "Lab2Parser.y"
	FileStream file = new FileStream(args[0], FileMode.Open);
#line 101 "Lab2Parser.y"
	parser.Scanner = new LexScanner.Scanner(file);
#line 102 "Lab2Parser.y"
	System.Console.WriteLine("File: " + args[0]);
#line 103 "Lab2Parser.y"

#line 104 "Lab2Parser.y"
	parser.Parse();
#line 105 "Lab2Parser.y"
}
#line 106 "Lab2Parser.y"

#line 107 "Lab2Parser.y"

#line 108 "Lab2Parser.y"

}
