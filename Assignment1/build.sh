#!/bin/bash
mono gplex.exe $1.lex
mono gppg.exe /gplex $2.y > $2.cs
dmcs /r:QUT.ShiftReduceParser.dll $1.cs $2.cs /out:$2.exe
