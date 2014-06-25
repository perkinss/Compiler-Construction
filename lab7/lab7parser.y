%using System.IO;
%using ASTree;

%token NUM
%token IDENT
//%token ASSIGN
%token FUN
%token VAR
%token WHILE
%token IF
%token THEN
%token ELSE
%token RETURN
%token LE GE NE AND OR NEG
%token DBG_SYMS

%{
    LexScanner.Scanner lexer;

    // define our own constructor for the Parser class
    public Parser( LexScanner.Scanner lexer ): base(lexer) {
        this.Scanner = this.lexer = lexer;
    }

    // returns the lexer's current line number
    public int LineNumber {
        get{ return lexer.LineNumber; }
    }

    // Use this function for reporting non-fatal errors discovered
    // while parsing. An example usage is:
    //    yyerror( "Identifier {0} has not been declared", idname );
    public void yyerror( string format, params Object[] args ) {
        Console.Write("{0}: ", LineNumber);
        Console.WriteLine(format, args);
    }

    ASTProgram program = new ASTProgram();

    public AST SyntaxTree {get{return program;}}

    string saved_text;
%}

%union {
  public AST statement;
  public List<AST> statements;
  public ASTExpr expr;
  public ASTBlock block;
  public List<ASTExpr> exprs;
  public List<string> idents;
  public List<ASTParam> parms;
  public ASTIdent ident;
  public ASTType tipe;
  public ASTParam parm;
}

%type <statements> StatementList
%type <block> Block
%type <statement>  Statement Debug Function While IfStmt Return
%type <exprs> ExprList
%type <expr> Expr Ident Call Atom BExpr
%type <parms> ParamList
%type <ident> Ident
%type <tipe> Type
%type <parm> Param

/* operator precedences & associativities in expressions */
%right '='
%left AND OR
%left '<' '>' LE GE NE EQ
%left '-' '+'
%left '*' '/'
%left UMINUS NOT
%right '^'

// Start parsing at rule Statement
%start Program

%%

Program: // type=void
    Program Function            {program.Add($2);}
  |
  ;
Debug: // type=statement
    DBG_SYMS {$$ = new ASTDebug(lexer.LineNumber,lexer.yytext.Trim());}
  ;
Function: // type=statement
    FUN Ident '(' ParamList ')' ':' Type Block
                                  {$$ = new ASTFunction($2,$4,$7,$8);}
  ;
ParamList: // type=idents
    ParamList ',' Param           {$$ = $1; $1.Add($3);}
  | Param                         {$$ = new List<ASTParam>();$$.Add($1);}
  |                               {$$ = new List<ASTParam>();}
  ;
Param:
    IDENT {saved_text=lexer.yytext;} ':' Type
                                  {$$ = new ASTParam(saved_text,$4);}
  ;
Type:
    IDENT                         {$$ = new ASTType(lexer.yytext);}
  ;
Block: // type=expr
  | '{' StatementList '}'         {$$ = new ASTBlock($2);}
  | '{'               '}'         {$$ = new ASTBlock();}
  ;
StatementList: // type=statements
    StatementList Statement       {$$ = $1;$$.Add($2);}
  | Statement                     {$$ = new List<AST>();$$.Add($1);}
  ;
IfStmt:
    IF BExpr Block ELSE Block {$$ = new ASTIf($2,$3,$5);}
  ;
While:
    WHILE BExpr Block             {$$ = new ASTWhile($2,$3);}
  ;
Return:
    RETURN Expr ';'               {$$ = new ASTReturn($2);}
  | RETURN      ';'               {$$ = new ASTReturn();}
  ;
Statement: // type=statement
    VAR Ident '=' Expr ';'        {$$ = new ASTVarDecl($2,$4);}
  | Ident '=' Expr ';'            {$$ = new ASTAssign($1,$3);}
  | Call  ';'                     {$$ = $1;}
  | IfStmt                        {$$ = $1;}
  | While                         {$$ = $1;}
  | Debug                         {$$ = $1;}
  | Return                        {$$ = $1;}
  ;
Expr: // type=expr
    Expr '+' Expr               {$$ = new ASTBinExpr($1,'+',$3);}
  | Expr '-' Expr               {$$ = new ASTBinExpr($1,'-',$3);}
  | Expr '*' Expr               {$$ = new ASTBinExpr($1,'*',$3);}
//  | Expr '/' Expr               {$$ = new ASTBinExpr($1,'/',$3);}
  | Expr '^' Expr               {$$ = new ASTBinExpr($1,'^',$3);}
  | Expr '<' Expr               {$$ = new ASTBinExpr($1,'<',$3);}
//  | Expr '>' Expr               {$$ = new ASTBinExpr($1,'<',$3);}
  | Expr EQ Expr                {$$ = new ASTBinExpr($1,'=',$3);}
//  | Expr AND Expr               {$$ = new ASTBinExpr($1,'&',$3);}
//  | Expr OR Expr                {$$ = new ASTBinExpr($1,'|',$3);}
  |      '-' Expr %prec UMINUS  {$$ = new ASTUnrExpr(   '-',$2);}
//  |      NOT Expr               {$$ = new ASTUnrExpr(   '!',$2);}
  | BExpr                       {$$ = $1;}
  | Atom                        {$$ = $1;}
  ;
BExpr: '(' Expr ')'             {$$ = $2;};
Atom: // type=expr
    NUM                         {$$ = new ASTInteger(lexer.LineNumber,lexer.yytext);}
  | Ident                       {$$ = $1;}
  | Call                        {$$ = $1;}
  ;
Call: // type=expr
    Ident '(' ExprList ')'      {$$ = new ASTCall($1,$3);}
  ;
Ident: // type=ident
    IDENT                       {$$ = new ASTIdent(lexer.LineNumber,lexer.yytext);}
  ;
ExprList: // type=exprs
    ExprList ',' Expr           {$1.Add($3);$$ = $1;}
  | Expr                        {$$ = new List<ASTExpr>();$$.Add($1);}
  |                             {$$ = new List<ASTExpr>();}
  ;


%%

