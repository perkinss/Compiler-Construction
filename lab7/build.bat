gplex.exe /out:lab7lexer.cs lab7lexer.l
gppg.exe /gplex lab7parser.y > lab7parser.cs
csc /r:QUT.ShiftReduceParser.dll /out:main.exe AST.cs ASTWriter.cs ASTTypeCheck.cs ASTIRBuild.cs SymbolTable.cs lab7lexer.cs lab7parser.cs IR.cs Main.cs Types.cs

