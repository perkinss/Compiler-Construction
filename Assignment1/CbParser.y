%using System.IO;
/* CbParser.y */

// The grammar shown in this file is INCOMPLETE!!
// It does not support class inheritance, it does not permit
// classes to contain methods (other than Main).
// Other language features may be missing too.

%namespace  FrontEnd
%tokentype  Tokens

// All tokens which can be used as operators in expressions
// they are ordered by precedence level (lowest first)
%right      '='
%left       OROR
%left       ANDAND
%nonassoc   EQEQ NOTEQ
%nonassoc   '>' GTEQ '<' LTEQ
%left       '+' '-'
%left       '*' '/' '%'
%left       UMINUS

// All other named tokens (i.e. the single character tokens are omitted)
// The order in which they are listed here does not matter.
%token      Kwd_using Kwd_class            	// Sections
%token      Kwd_public				// Scope
%token      Kwd_const Kwd_override Kwd_static		// Modifiers
%token      Kwd_break Kwd_return Kwd_else Kwd_if Kwd_while	// Flow control
%token      Kwd_void Kwd_int Kwd_char Kwd_string       	// Types
%token      Kwd_new Kwd_out	// Not sure if needed
%token      PLUSPLUS MINUSMINUS Ident Number StringConst

%start Program

%{
  public void yyerror( string format, params Object[] args ) {
    Console.Write("{0}: ", 99);//LineNumber);
    Console.WriteLine(format, args);
  }
%}

%%

/* *************************************************************************
   *                                                                       *
   *         PRODUCTION RULES AND ASSOCIATED SEMANTIC ACTIONS              *
   *                                                                       *
   ************************************************************************* */

Program:        UsingList ClassList
        ;

UsingList:      /* empty */
        |       Kwd_using Ident ';' UsingList
        ;

ClassList:	    ClassList ClassDecl
		|		ClassDecl
		;

ClassDecl:		Kwd_class Ident '{'  DeclList  '}'
		;

DeclList:       DeclList ConstDecl
        |       DeclList MethodDecl
        |       DeclList FieldDeclList
        |       /* empty */
        ;

ConstDecl:      Kwd_public Kwd_const Type Ident '=' InitVal ';'
        ;

InitVal:        Number
        |       StringConst
        ;

FieldDeclList:  FieldDeclList FieldDecl
        |       /* empty */
        ;

FieldDecl:      Kwd_public Type IdentList ';'
        ;

IdentList:      IdentList ',' Ident
        |       Ident
        ;

MethodDecl:     Kwd_public Kwd_static Kwd_void Ident '(' OptFormals ')' Block
        ;

OptFormals:     /* empty */
        |       FormalPars
        ;

FormalPars:     FormalDecl
        |       FormalPars ',' FormalDecl
        ;

FormalDecl:     Type Ident
        ;

Type:           TypeName
        |       TypeName '[' ']'
        ;

TypeName:       Ident
        |       Kwd_int
        |       Kwd_string
        ;

Statement:      Designator '=' Expr ';'
        |       Designator '(' OptActuals ')' ';'
        |       Designator PLUSPLUS ';'
        |       Designator MINUSMINUS ';'
        |       Kwd_if '(' Expr ')' Statement OptElsePart
        |       Kwd_while '(' Expr ')' Statement
        |       Kwd_break ';'
        |       Kwd_return ';'
        |       Kwd_return Expr ';'
        |       Block
        |       ';'
        ;

OptActuals:     /* empty */
        |       ActPars
        ;

ActPars:        ActPars ',' Expr
        |       Expr
        ;

OptElsePart:    Kwd_else Statement
        |       /* empty */
        ;

Block:          '{' DeclsAndStmts '}'
        ;

LocalDecl:      Ident IdentList ';'
        |       Ident '[' ']' IdentList ';'
        ;

DeclsAndStmts:   /* empty */
        |       DeclsAndStmts Statement
        |       DeclsAndStmts LocalDecl
        ;

Expr:           Expr OROR Expr
        |       Expr ANDAND Expr
        |       Expr EQEQ Expr
        |       Expr NOTEQ Expr
        |       Expr LTEQ Expr
        |       Expr '<' Expr
        |       Expr GTEQ Expr
        |       Expr '>' Expr
        |       Expr '+' Expr
        |       Expr '-' Expr
        |       Expr '*' Expr
        |       Expr '/' Expr
        |       Expr '%' Expr
        |       '-' Expr %prec UMINUS
        |       Designator
        |       Designator '(' OptActuals ')'
        |       Number
        |       StringConst
        |       StringConst '.' Ident // Ident must be "Length"
        |       Kwd_new Ident '(' ')'
        |       Kwd_new Ident '[' Expr ']'
        |       '(' Expr ')'
        ;

Designator:     Ident Qualifiers
        ;

Qualifiers:     '.' Ident Qualifiers
        |       '[' Expr ']' Qualifiers
        |       /* empty */
        ;

%%

Stack<string> id_stack = new Stack<string>();

void push_id() {
	string t = ((FrontEnd.Scanner)Scanner).last_token_text;
	id_stack.Push(t);
}
string pop_id() {
	return id_stack.Pop();
}

string token_text() {
	return ((FrontEnd.Scanner)Scanner).last_token_text;
}

void writeln() {
	writeln(null,null);
}
void writeln(string opcode) {
	writeln(opcode,null);
}

void writeln(string opcode, string value) {
	if (opcode != null) {
		System.Console.Write(opcode);
		if (value != null) {
			System.Console.Write(' '+value);
		}
	}
	System.Console.Write('\n');
}

// The parser needs a constructor
Parser() : base(null) { }

static void Main(string[] args)
{
	Parser parser = new Parser();

	FileStream file = new FileStream(args[0], FileMode.Open);
	parser.Scanner = new FrontEnd.Scanner(file);
	System.Console.WriteLine("File: " + args[0]);

	parser.Parse();
}


