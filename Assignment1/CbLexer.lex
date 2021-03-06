%namespace FrontEnd
%tokentype Tokens

%{
  public int lineNum = 1;

  public int LineNumber { get{ return lineNum; } }

  public override void yyerror( string msg, params object[] args ) {
    Console.WriteLine("{0}: ", lineNum);
    if (args == null || args.Length == 0) {
      Console.WriteLine("{0}", msg);
    }
    else {
      Console.WriteLine(msg, args);
    }
  }

  public void yyerror( int lineNum, string msg, params object[] args ) {
    Console.WriteLine("{0}: {1}", msg, args);
  }

%}

space [ \t]
opchar [=><+\-*/%]

%%
{space}          {}
"||"             {last_token_text=yytext;return (int)Tokens.OROR;}
"&&"             {last_token_text=yytext;return (int)Tokens.ANDAND;}
"=="             {last_token_text=yytext;return (int)Tokens.EQEQ;}
"!="             {last_token_text=yytext;return (int)Tokens.NOTEQ;}
">="             {last_token_text=yytext;return (int)Tokens.GTEQ;}
"<="             {last_token_text=yytext;return (int)Tokens.LTEQ;}
"using"          {last_token_text=yytext;return (int)Tokens.Kwd_using;}
"class"          {last_token_text=yytext;return (int)Tokens.Kwd_class;}
"public"         {last_token_text=yytext;return (int)Tokens.Kwd_public;}
"const"          {last_token_text=yytext;return (int)Tokens.Kwd_const;}
"override"       {last_token_text=yytext;return (int)Tokens.Kwd_override;}
"static"         {last_token_text=yytext;return (int)Tokens.Kwd_static;}
"break"          {last_token_text=yytext;return (int)Tokens.Kwd_break;}
"return"         {last_token_text=yytext;return (int)Tokens.Kwd_return;}
"else"           {last_token_text=yytext;return (int)Tokens.Kwd_else;}
"if"             {last_token_text=yytext;return (int)Tokens.Kwd_if;}
"while"          {last_token_text=yytext;return (int)Tokens.Kwd_while;}
"void"           {last_token_text=yytext;return (int)Tokens.Kwd_void;}
"int"            {last_token_text=yytext;return (int)Tokens.Kwd_int;}
"char"           {last_token_text=yytext;return (int)Tokens.Kwd_char;}
"string"         {last_token_text=yytext;return (int)Tokens.Kwd_string;}
"new"            {last_token_text=yytext;return (int)Tokens.Kwd_new;}
"out"            {last_token_text=yytext;return (int)Tokens.Kwd_out;}
"++"             {last_token_text=yytext;return (int)Tokens.PLUSPLUS;}
"--"             {last_token_text=yytext;return (int)Tokens.MINUSMINUS;}

[a-zA-Z][a-zA-Z_0-9]*           {last_token_text=yytext;return (int)Tokens.Ident;}
0|[1-9][0-9]*    {last_token_text=yytext;return (int)Tokens.Number;}
"\""[.]*"\""     {last_token_text=yytext;return (int)Tokens.StringConst;}
{opchar}         {return (int)(yytext[0]);}
"\n"             {return (int)'\n';}
"\r\n"           {return (int)'\n';}

.                { yyerror("illegal character ({0})", yytext); }

%%

public string last_token_text = "";

