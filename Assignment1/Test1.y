/* Test.y */

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

%%

/* *************************************************************************
   *                                                                       *
   *         PRODUCTION RULES AND ASSOCIATED SEMANTIC ACTIONS              *
   *                                                                       *
   ************************************************************************* */

Program: DeclList
        ;

DeclList:       DeclList ConstDecl
        |       DeclList MethodDecl
        |       DeclList FieldDeclList
        |       /* empty */
        ;

ConstDecl:      Kwd_public Kwd_const Type Ident '=' InitVal ';'
        ;

FieldDeclList:  FieldDeclList FieldDecl
        ;

FieldDecl:      Kwd_public Type IdentList ';'
        ;

IdentList:      IdentList ',' Ident
        |       Ident
        ;

MethodDecl:     Kwd_public Kwd_static Kwd_void Ident '(' OptFormals ')' Block
        ;

Type: 		Kwd_int
	;

OptFormals:	Ident
	;

Block:		Ident
	;

InitVal:	Ident
	;

%%

