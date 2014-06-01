// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2010
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.0
// Machine:  Catherby
// DateTime: 01/06/2014 9:53:21 AM
// UserName: tlavallee
// Input file <CbParser.y - 29/05/2014 12:31:30 PM>

// options: lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;

namespace FrontEnd
{
public enum Tokens {error=126,
    EOF=127,OROR=128,ANDAND=129,EQEQ=130,NOTEQ=131,GTEQ=132,
    LTEQ=133,UMINUS=134,Kwd_using=135,Kwd_class=136,Kwd_public=137,Kwd_const=138,
    Kwd_override=139,Kwd_static=140,Kwd_break=141,Kwd_return=142,Kwd_else=143,Kwd_if=144,
    Kwd_while=145,Kwd_void=146,Kwd_int=147,Kwd_char=148,Kwd_string=149,Kwd_new=150,
    Kwd_out=151,PLUSPLUS=152,MINUSMINUS=153,Ident=154,Number=155,StringConst=156};

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
  // Verbatim content from CbParser.y - 29/05/2014 12:31:30 PM
#line 35 "CbParser.y"
  public void yyerror( string format, params Object[] args ) {
#line 36 "CbParser.y"
    Console.Write("{0}: ", 99);//LineNumber);
#line 37 "CbParser.y"
    Console.WriteLine(format, args);
#line 38 "CbParser.y"
  }
  // End verbatim content from CbParser.y - 29/05/2014 12:31:30 PM

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[80];
  private static State[] states = new State[159];
  private static string[] nonTerms = new string[] {
      "Program", "$accept", "UsingList", "ClassList", "ClassDecl", "DeclList", 
      "ConstDecl", "MethodDecl", "FieldDeclList", "Type", "InitVal", "FieldDecl", 
      "IdentList", "OptFormals", "Block", "FormalPars", "FormalDecl", "TypeName", 
      "Statement", "Designator", "Expr", "OptActuals", "OptElsePart", "ActPars", 
      "DeclsAndStmts", "LocalDecl", "Qualifiers", };

  static Parser() {
    states[0] = new State(new int[]{135,155,136,-3},new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{127,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{136,6},new int[]{-4,4,-5,154});
    states[4] = new State(new int[]{136,6,127,-2},new int[]{-5,5});
    states[5] = new State(-5);
    states[6] = new State(new int[]{154,7});
    states[7] = new State(new int[]{123,8});
    states[8] = new State(-11,new int[]{-6,9});
    states[9] = new State(new int[]{125,10,137,28},new int[]{-7,11,-8,12,-9,13});
    states[10] = new State(-7);
    states[11] = new State(-8);
    states[12] = new State(-9);
    states[13] = new State(new int[]{137,15,125,-10},new int[]{-12,14});
    states[14] = new State(-15);
    states[15] = new State(new int[]{154,25,147,26,149,27},new int[]{-10,16,-18,22});
    states[16] = new State(new int[]{154,21},new int[]{-13,17});
    states[17] = new State(new int[]{59,18,44,19});
    states[18] = new State(-17);
    states[19] = new State(new int[]{154,20});
    states[20] = new State(-18);
    states[21] = new State(-19);
    states[22] = new State(new int[]{91,23,154,-26});
    states[23] = new State(new int[]{93,24});
    states[24] = new State(-27);
    states[25] = new State(-28);
    states[26] = new State(-29);
    states[27] = new State(-30);
    states[28] = new State(new int[]{138,29,140,37});
    states[29] = new State(new int[]{154,25,147,26,149,27},new int[]{-10,30,-18,22});
    states[30] = new State(new int[]{154,31});
    states[31] = new State(new int[]{61,32});
    states[32] = new State(new int[]{155,35,156,36},new int[]{-11,33});
    states[33] = new State(new int[]{59,34});
    states[34] = new State(-12);
    states[35] = new State(-13);
    states[36] = new State(-14);
    states[37] = new State(new int[]{146,38});
    states[38] = new State(new int[]{154,39});
    states[39] = new State(new int[]{40,40});
    states[40] = new State(new int[]{154,25,147,26,149,27,41,-21},new int[]{-14,41,-16,148,-17,153,-10,151,-18,22});
    states[41] = new State(new int[]{41,42});
    states[42] = new State(new int[]{123,44},new int[]{-15,43});
    states[43] = new State(-20);
    states[44] = new State(-51,new int[]{-25,45});
    states[45] = new State(new int[]{125,46,154,120,144,127,145,135,141,140,142,142,123,44,59,147},new int[]{-19,47,-26,48,-20,49,-15,146});
    states[46] = new State(-48);
    states[47] = new State(-52);
    states[48] = new State(-53);
    states[49] = new State(new int[]{61,50,40,112,152,116,153,118});
    states[50] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,51,-20,81});
    states[51] = new State(new int[]{59,52,128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77});
    states[52] = new State(-31);
    states[53] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,54,-20,81});
    states[54] = new State(new int[]{128,-54,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,59,-54,44,-54,41,-54,93,-54});
    states[55] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,56,-20,81});
    states[56] = new State(new int[]{128,-55,129,-55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,59,-55,44,-55,41,-55,93,-55});
    states[57] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,58,-20,81});
    states[58] = new State(new int[]{128,-56,129,-56,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,59,-56,44,-56,41,-56,93,-56});
    states[59] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,60,-20,81});
    states[60] = new State(new int[]{128,-57,129,-57,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,59,-57,44,-57,41,-57,93,-57});
    states[61] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,62,-20,81});
    states[62] = new State(new int[]{128,-58,129,-58,130,-58,131,-58,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,59,-58,44,-58,41,-58,93,-58});
    states[63] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,64,-20,81});
    states[64] = new State(new int[]{128,-59,129,-59,130,-59,131,-59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,59,-59,44,-59,41,-59,93,-59});
    states[65] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,66,-20,81});
    states[66] = new State(new int[]{128,-60,129,-60,130,-60,131,-60,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,59,-60,44,-60,41,-60,93,-60});
    states[67] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,68,-20,81});
    states[68] = new State(new int[]{128,-61,129,-61,130,-61,131,-61,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,59,-61,44,-61,41,-61,93,-61});
    states[69] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,70,-20,81});
    states[70] = new State(new int[]{128,-62,129,-62,130,-62,131,-62,133,-62,60,-62,132,-62,62,-62,43,-62,45,-62,42,73,47,75,37,77,59,-62,44,-62,41,-62,93,-62});
    states[71] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,72,-20,81});
    states[72] = new State(new int[]{128,-63,129,-63,130,-63,131,-63,133,-63,60,-63,132,-63,62,-63,43,-63,45,-63,42,73,47,75,37,77,59,-63,44,-63,41,-63,93,-63});
    states[73] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,74,-20,81});
    states[74] = new State(-64);
    states[75] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,76,-20,81});
    states[76] = new State(-65);
    states[77] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,78,-20,81});
    states[78] = new State(-66);
    states[79] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,80,-20,81});
    states[80] = new State(-67);
    states[81] = new State(new int[]{40,82,59,-68,128,-68,129,-68,130,-68,131,-68,133,-68,60,-68,132,-68,62,-68,43,-68,45,-68,42,-68,47,-68,37,-68,44,-68,41,-68,93,-68});
    states[82] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108,41,-42},new int[]{-22,83,-24,85,-21,111,-20,81});
    states[83] = new State(new int[]{41,84});
    states[84] = new State(-69);
    states[85] = new State(new int[]{44,86,41,-43});
    states[86] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,87,-20,81});
    states[87] = new State(new int[]{128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,44,-44,41,-44});
    states[88] = new State(new int[]{46,90,91,93,40,-79,59,-79,128,-79,129,-79,130,-79,131,-79,133,-79,60,-79,132,-79,62,-79,43,-79,45,-79,42,-79,47,-79,37,-79,44,-79,41,-79,93,-79,61,-79,152,-79,153,-79},new int[]{-27,89});
    states[89] = new State(-76);
    states[90] = new State(new int[]{154,91});
    states[91] = new State(new int[]{46,90,91,93,40,-79,59,-79,128,-79,129,-79,130,-79,131,-79,133,-79,60,-79,132,-79,62,-79,43,-79,45,-79,42,-79,47,-79,37,-79,44,-79,41,-79,93,-79,61,-79,152,-79,153,-79},new int[]{-27,92});
    states[92] = new State(-77);
    states[93] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,94,-20,81});
    states[94] = new State(new int[]{93,95,128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77});
    states[95] = new State(new int[]{46,90,91,93,40,-79,59,-79,128,-79,129,-79,130,-79,131,-79,133,-79,60,-79,132,-79,62,-79,43,-79,45,-79,42,-79,47,-79,37,-79,44,-79,41,-79,93,-79,61,-79,152,-79,153,-79},new int[]{-27,96});
    states[96] = new State(-78);
    states[97] = new State(-70);
    states[98] = new State(new int[]{46,99,59,-71,128,-71,129,-71,130,-71,131,-71,133,-71,60,-71,132,-71,62,-71,43,-71,45,-71,42,-71,47,-71,37,-71,44,-71,41,-71,93,-71});
    states[99] = new State(new int[]{154,100});
    states[100] = new State(-72);
    states[101] = new State(new int[]{154,102});
    states[102] = new State(new int[]{40,103,91,105});
    states[103] = new State(new int[]{41,104});
    states[104] = new State(-73);
    states[105] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,106,-20,81});
    states[106] = new State(new int[]{93,107,128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77});
    states[107] = new State(-74);
    states[108] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,109,-20,81});
    states[109] = new State(new int[]{41,110,128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77});
    states[110] = new State(-75);
    states[111] = new State(new int[]{128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77,44,-45,41,-45});
    states[112] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108,41,-42},new int[]{-22,113,-24,85,-21,111,-20,81});
    states[113] = new State(new int[]{41,114});
    states[114] = new State(new int[]{59,115});
    states[115] = new State(-32);
    states[116] = new State(new int[]{59,117});
    states[117] = new State(-33);
    states[118] = new State(new int[]{59,119});
    states[119] = new State(-34);
    states[120] = new State(new int[]{91,123,46,90,154,21,61,-79,40,-79,152,-79,153,-79},new int[]{-27,89,-13,121});
    states[121] = new State(new int[]{59,122,44,19});
    states[122] = new State(-49);
    states[123] = new State(new int[]{93,124,45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,94,-20,81});
    states[124] = new State(new int[]{154,21},new int[]{-13,125});
    states[125] = new State(new int[]{59,126,44,19});
    states[126] = new State(-50);
    states[127] = new State(new int[]{40,128});
    states[128] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,129,-20,81});
    states[129] = new State(new int[]{41,130,128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77});
    states[130] = new State(new int[]{154,88,144,127,145,135,141,140,142,142,123,44,59,147},new int[]{-19,131,-20,49,-15,146});
    states[131] = new State(new int[]{143,133,125,-47,154,-47,144,-47,145,-47,141,-47,142,-47,123,-47,59,-47},new int[]{-23,132});
    states[132] = new State(-35);
    states[133] = new State(new int[]{154,88,144,127,145,135,141,140,142,142,123,44,59,147},new int[]{-19,134,-20,49,-15,146});
    states[134] = new State(-46);
    states[135] = new State(new int[]{40,136});
    states[136] = new State(new int[]{45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,137,-20,81});
    states[137] = new State(new int[]{41,138,128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77});
    states[138] = new State(new int[]{154,88,144,127,145,135,141,140,142,142,123,44,59,147},new int[]{-19,139,-20,49,-15,146});
    states[139] = new State(-36);
    states[140] = new State(new int[]{59,141});
    states[141] = new State(-37);
    states[142] = new State(new int[]{59,143,45,79,154,88,155,97,156,98,150,101,40,108},new int[]{-21,144,-20,81});
    states[143] = new State(-38);
    states[144] = new State(new int[]{59,145,128,53,129,55,130,57,131,59,133,61,60,63,132,65,62,67,43,69,45,71,42,73,47,75,37,77});
    states[145] = new State(-39);
    states[146] = new State(-40);
    states[147] = new State(-41);
    states[148] = new State(new int[]{44,149,41,-22});
    states[149] = new State(new int[]{154,25,147,26,149,27},new int[]{-17,150,-10,151,-18,22});
    states[150] = new State(-24);
    states[151] = new State(new int[]{154,152});
    states[152] = new State(-25);
    states[153] = new State(-23);
    states[154] = new State(-6);
    states[155] = new State(new int[]{154,156});
    states[156] = new State(new int[]{59,157});
    states[157] = new State(new int[]{135,155,136,-3},new int[]{-3,158});
    states[158] = new State(-4);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,127});
    rules[2] = new Rule(-1, new int[]{-3,-4});
    rules[3] = new Rule(-3, new int[]{});
    rules[4] = new Rule(-3, new int[]{135,154,59,-3});
    rules[5] = new Rule(-4, new int[]{-4,-5});
    rules[6] = new Rule(-4, new int[]{-5});
    rules[7] = new Rule(-5, new int[]{136,154,123,-6,125});
    rules[8] = new Rule(-6, new int[]{-6,-7});
    rules[9] = new Rule(-6, new int[]{-6,-8});
    rules[10] = new Rule(-6, new int[]{-6,-9});
    rules[11] = new Rule(-6, new int[]{});
    rules[12] = new Rule(-7, new int[]{137,138,-10,154,61,-11,59});
    rules[13] = new Rule(-11, new int[]{155});
    rules[14] = new Rule(-11, new int[]{156});
    rules[15] = new Rule(-9, new int[]{-9,-12});
    rules[16] = new Rule(-9, new int[]{});
    rules[17] = new Rule(-12, new int[]{137,-10,-13,59});
    rules[18] = new Rule(-13, new int[]{-13,44,154});
    rules[19] = new Rule(-13, new int[]{154});
    rules[20] = new Rule(-8, new int[]{137,140,146,154,40,-14,41,-15});
    rules[21] = new Rule(-14, new int[]{});
    rules[22] = new Rule(-14, new int[]{-16});
    rules[23] = new Rule(-16, new int[]{-17});
    rules[24] = new Rule(-16, new int[]{-16,44,-17});
    rules[25] = new Rule(-17, new int[]{-10,154});
    rules[26] = new Rule(-10, new int[]{-18});
    rules[27] = new Rule(-10, new int[]{-18,91,93});
    rules[28] = new Rule(-18, new int[]{154});
    rules[29] = new Rule(-18, new int[]{147});
    rules[30] = new Rule(-18, new int[]{149});
    rules[31] = new Rule(-19, new int[]{-20,61,-21,59});
    rules[32] = new Rule(-19, new int[]{-20,40,-22,41,59});
    rules[33] = new Rule(-19, new int[]{-20,152,59});
    rules[34] = new Rule(-19, new int[]{-20,153,59});
    rules[35] = new Rule(-19, new int[]{144,40,-21,41,-19,-23});
    rules[36] = new Rule(-19, new int[]{145,40,-21,41,-19});
    rules[37] = new Rule(-19, new int[]{141,59});
    rules[38] = new Rule(-19, new int[]{142,59});
    rules[39] = new Rule(-19, new int[]{142,-21,59});
    rules[40] = new Rule(-19, new int[]{-15});
    rules[41] = new Rule(-19, new int[]{59});
    rules[42] = new Rule(-22, new int[]{});
    rules[43] = new Rule(-22, new int[]{-24});
    rules[44] = new Rule(-24, new int[]{-24,44,-21});
    rules[45] = new Rule(-24, new int[]{-21});
    rules[46] = new Rule(-23, new int[]{143,-19});
    rules[47] = new Rule(-23, new int[]{});
    rules[48] = new Rule(-15, new int[]{123,-25,125});
    rules[49] = new Rule(-26, new int[]{154,-13,59});
    rules[50] = new Rule(-26, new int[]{154,91,93,-13,59});
    rules[51] = new Rule(-25, new int[]{});
    rules[52] = new Rule(-25, new int[]{-25,-19});
    rules[53] = new Rule(-25, new int[]{-25,-26});
    rules[54] = new Rule(-21, new int[]{-21,128,-21});
    rules[55] = new Rule(-21, new int[]{-21,129,-21});
    rules[56] = new Rule(-21, new int[]{-21,130,-21});
    rules[57] = new Rule(-21, new int[]{-21,131,-21});
    rules[58] = new Rule(-21, new int[]{-21,133,-21});
    rules[59] = new Rule(-21, new int[]{-21,60,-21});
    rules[60] = new Rule(-21, new int[]{-21,132,-21});
    rules[61] = new Rule(-21, new int[]{-21,62,-21});
    rules[62] = new Rule(-21, new int[]{-21,43,-21});
    rules[63] = new Rule(-21, new int[]{-21,45,-21});
    rules[64] = new Rule(-21, new int[]{-21,42,-21});
    rules[65] = new Rule(-21, new int[]{-21,47,-21});
    rules[66] = new Rule(-21, new int[]{-21,37,-21});
    rules[67] = new Rule(-21, new int[]{45,-21});
    rules[68] = new Rule(-21, new int[]{-20});
    rules[69] = new Rule(-21, new int[]{-20,40,-22,41});
    rules[70] = new Rule(-21, new int[]{155});
    rules[71] = new Rule(-21, new int[]{156});
    rules[72] = new Rule(-21, new int[]{156,46,154});
    rules[73] = new Rule(-21, new int[]{150,154,40,41});
    rules[74] = new Rule(-21, new int[]{150,154,91,-21,93});
    rules[75] = new Rule(-21, new int[]{40,-21,41});
    rules[76] = new Rule(-20, new int[]{154,-27});
    rules[77] = new Rule(-27, new int[]{46,154,-27});
    rules[78] = new Rule(-27, new int[]{91,-21,93,-27});
    rules[79] = new Rule(-27, new int[]{});
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

#line 180 "CbParser.y"

#line 181 "CbParser.y"

#line 182 "CbParser.y"

#line 183 "CbParser.y"

}
}
