#!/bin/bash
gplex.exe $1.l
Gppg.exe /gplex $1.y > $1_main.cs
dmcs /r:QUT.ShiftReduceParser.dll $1.cs $1_main.cs
