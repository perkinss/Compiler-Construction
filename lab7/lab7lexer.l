%namespace LexScanner
%tokentype Tokens

%{
  public override void yyerror( string msg, params object[] args ) {
    Console.Write("{0}: ", yyline);
    if (args == null || args.Length == 0)
      Console.WriteLine("{0}", msg);
    else
      Console.WriteLine(msg, args);
  }

  public void yyerror( int lineNum, string msg, params object[] args ) {
    Console.WriteLine("{0}: {1}", msg, args);
  }

  public int LineNumber { get{return yyline;} }    
%}

space [ \t]
linecomment [/][/][^\n]*
opchar [+\-*/^%()<>=,:;{}]
eol   [ \t\r]*[\n]
%%
{space}            {}
{linecomment}      {}
"fun"              {return (int)Tokens.FUN;}
"var"              {return (int)Tokens.VAR;}
"while"            {return (int)Tokens.WHILE;}
"if"               {return (int)Tokens.IF;}
"then"             {return (int)Tokens.THEN;}
"else"             {return (int)Tokens.ELSE;}
"return"           {return (int)Tokens.RETURN;}
"and"              {return (int)Tokens.AND;}
"or"               {return (int)Tokens.OR;}
"not"              {return (int)Tokens.NOT;}
[#][a-z]+{eol}     {return (int)Tokens.DBG_SYMS;}
0|[1-9][0-9]*      {return (int)Tokens.NUM;}
[a-z][a-zA-Z0-9]*  {return (int)Tokens.IDENT;}
"<="               {return (int)Tokens.LE;}
">="               {return (int)Tokens.GE;}
"<>"               {return (int)Tokens.NE;}
"!="               {return (int)Tokens.NE;}
"=="               {return (int)Tokens.EQ;}
{opchar}           {return (int)(yytext[0]);}
[\n] {}
%%

