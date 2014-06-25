#define USE_INSTR_ASSIGNMENT_NOTATION

using System;
using System.Collections.Generic;

namespace IR
{
  public abstract class Oper
  {
    public Oper() { }
  
    public virtual bool IsReg()   {return false;}
    public virtual bool IsImm()   {return false;}
    public virtual bool IsCC()    {return false;}
    public virtual bool IsCP()    {return false;}
    public virtual bool IsLabel() {return false;}
    public virtual bool IsBB()    {return false;}
    public virtual bool IsFN()    {return false;}

    public abstract void Write();
  }

  public class Reg : Oper
  {
    bool is_def; // def=true, use=false
    bool is_kill; // no uses of register after this one
    bool is_dead; // defined but never used
    
    public uint Number {get;set;} // the register number

    public Reg(uint rnum, bool is_def) {
      Number=rnum; this.is_def=is_def;
    }
    public override bool IsReg() {return true;}
    public bool IsDef() {return is_def;}
    public bool IsUse() {return !is_def;}

    public void setKill(bool k) {is_kill = k;}
    public void setDead(bool d) {is_dead = d;}
    public void setDef(bool d) {is_def = d;}

    public override void Write() {
      Console.Write("V{0}",Number);
      char c = '<';
      if (is_def) {Console.Write("{0}def",c);c=',';}
      if (is_kill) {Console.Write("{0}kill",c);c=',';}
      if (is_dead) {Console.Write("{0}dead",c);c=',';}
      if (c == ',') Console.Write(">");
    }
  }

  public class Imm : Oper
  {
    int value;
    public Imm(int value) {
      this.value=value;
    }
    public override bool IsImm() {return true;}
    public int GetValue() {return value;}
    public override void Write() {Console.Write("{0}",value);}
  }
  
  // Condition Code, for compare operations
  public class CC : Oper
  {
    public enum ty { AL, EQ, NE, LT, LE, GT, GE }
    public ty Code {private set; get;}

    public CC(ty code) {Code = code;}
    public override bool IsCC() {return true;}
    public override void Write() {Console.Write("{0}",Code);}
  }

  // Constant Pool Offset
  public class CP : Oper
  {
    public Constant ConstVal {private set; get;}
    public CP(Constant c) {ConstVal = c;}
    public override bool IsCP() {return true;}
    public override void Write() {Console.Write("<cp#{0}>",ConstVal.Index);}
  }

  public class Label : Oper
  {
    public BasicBlock Block {get;set;}
    public uint BlockNumber {private set; get;}
    
    public Label(uint blk_num) {BlockNumber = blk_num; Block=null;}
    public void SetBasicBlock(BasicBlock bb) {Block = bb;BlockNumber = bb.Number;}
    
    public override bool IsLabel() {return true;}
    public override void Write() {Console.Write("<BB#{0}>",BlockNumber);}
  }

  public class BB : Oper
  {
    public BasicBlock Block {get;set;}

    public BB(BasicBlock bb) {Block = bb;}
    public override bool IsBB() {return true;}
    public override void Write() {Console.Write("<BB#{0}>",Block.Number);}
  }

  public class FN : Oper
  {
    public Function Func {get;set;}
    public uint BlockNumber {private set; get;}
    
    public FN(Function fn) {Func=fn;}
    
    public override bool IsFN() {return true;}
    public override void Write() {Console.Write("<@{0}>",Func.Name);}
  }

  public class Instr
  {
    public enum op_t { ADD=0, SUB, MUL, CALL, CMP, BAL, BCC, RET, STR, LDR, MOV };
    public op_t Opcode {private set; get;}

    List<Oper> operands;

    private Instr(op_t opc, List<Oper> args) {
      Opcode = opc; operands = args;
    }
    
    public bool IsBranch() {return Opcode == op_t.BAL || Opcode == op_t.BCC;}
    public bool IsReturn() {return Opcode == op_t.RET;}
    public bool IsCall() {return Opcode == op_t.CALL;}
    public bool IsSTR() {return Opcode == op_t.STR;}
    public bool IsLDR() {return Opcode == op_t.LDR;}
    public bool IsTerminator() {return IsBranch() || IsReturn();}
    
    public IEnumerable<Oper> OpIter {get{return operands;}}
    public Oper getOperand(int o) {return operands[o];}
    public int NumOperands {get{return operands.Count;}}

    
    private static Oper CheckReg(Oper o, bool is_def) {
      if (o.IsReg()) {Reg r=(Reg)o; o = new Reg(r.Number,is_def);}
      return o;
    }

    private static Oper CheckImmReg(Oper o) {
      if (o.IsReg() || o.IsImm()) {return CheckReg(o,false);}
      else {throw new Exception("Expected (IMM|REG) argument");}
    }

    private static Instr CreateMATH(op_t op, Reg def, Reg lhs, Oper rhs) {
      rhs = CheckImmReg(rhs);
      def = new Reg(def.Number,true);
      lhs = new Reg(lhs.Number,false);
      var args = new List<Oper>{def,lhs,rhs};
      return new Instr(op,args);
    }
    public static Instr CreateADD(Reg def, Reg lhs, Oper rhs) {return CreateMATH(op_t.ADD,def,lhs,rhs);}
    public static Instr CreateSUB(Reg def, Reg lhs, Oper rhs) {return CreateMATH(op_t.SUB,def,lhs,rhs);}
    public static Instr CreateMUL(Reg def, Reg lhs, Oper rhs) {return CreateMATH(op_t.MUL,def,lhs,rhs);}
    public static Instr CreateRET(Reg use) {return new Instr(op_t.RET,new List<Oper>{use});}
	public static Instr CreateRET() {return new Instr(op_t.RET,new List<Oper>());}
    public static Instr CreateCMP(Reg def, CC.ty cc, Reg lhs, Oper rhs) {
      rhs = CheckImmReg(rhs);
      CC cop = new CC(cc);
      return new Instr(op_t.CMP,new List<Oper>{def,cop,lhs,rhs});
    }
    public static Instr CreateBAL(BB bb) {return new Instr(op_t.BAL, new List<Oper>{bb});}
    public static Instr CreateBCC(Reg cc, BB tbb, BB fbb) {
      return new Instr(op_t.BCC, new List<Oper>{new Reg(cc.Number,false),tbb,fbb});
    }
    public static Instr CreateSTR(uint valu_reg, uint addr_reg) {return new Instr(op_t.LDR,new List<Oper>{new Reg(valu_reg,false),new Reg(addr_reg,false)});}
    public static Instr CreateVoidCALL(Function f, List<Oper> args) {
      List<Oper> xs = new List<Oper>();
      xs.Add(new FN(f));
      foreach (Oper o in args) {xs.Add(CheckReg(o,false));}
      return new Instr(op_t.CALL,xs);
    }
    public static Instr CreateCALL(Function f, Reg def, List<Oper> args) {
      List<Oper> xs = new List<Oper>();
      xs.Add(new FN(f));
      xs.Add(new Reg(def.Number,true));
      foreach (Oper o in args) {xs.Add(CheckReg(o,false));}
      return new Instr(op_t.CALL,xs);
    }
    public static Instr CreateMOV(Reg def, Oper valu) {
      valu = CheckImmReg(valu);
      return new Instr(op_t.MOV,new List<Oper>{new Reg(def.Number,true),valu});
    }

    public void Write() {
      bool comma = false;
#if USE_INSTR_ASSIGNMENT_NOTATION
      int i = 0;
      Console.Write("  ");
      while (i < operands.Count) {
        Oper o = operands[i];
        if (!o.IsReg() || !((Reg)o).IsDef()) break;
        if (comma) Console.Write(", ");
        else comma = true;
        o.Write();
        i++;
      }
      if (i > 0) {Console.Write(" = ");}
      Console.Write("{0} ",Opcode);
      comma = false;
      while (i < operands.Count) {
        Oper o = operands[i];
        if (comma) Console.Write(", ");
        else comma = true;
        o.Write();
        i++;
      }
#else
      foreach (Oper o in operands) {
        if (comma) Console.Write(',');
        else comma = true;
        o.Write();
      }
#endif
      Console.WriteLine();
    }
  }


  public class BasicBlock {
    public uint Number {private set; get;}
    public int InstrCount {get{return instructions.Count;}}
    
    List<Instr> instructions = new List<Instr>();
    List<BasicBlock> successors = new List<BasicBlock>();
    List<BasicBlock> predecessors = new List<BasicBlock>();
    public BasicBlock(uint number) {
      Number = number;
    }

    public void AddPredecessor(BasicBlock bb) {predecessors.Add(bb);}
    public void AddSuccessor(BasicBlock bb) {successors.Add(bb);}

    public void Add(Instr i) {
      if (instructions.Count > 0) {
        Instr last = instructions[instructions.Count - 1];
        if (last.Opcode == Instr.op_t.BAL) {
          Console.Write("Attempting to add: "); i.Write();
          Console.Write("Last instruction : "); last.Write();
          throw new Exception("Can't add instructions after a branch");
        }
        else if (last.Opcode == Instr.op_t.BCC && i.Opcode != Instr.op_t.BAL) {
          Console.Write("Attempting to add:"); i.Write();
          Console.Write("Last instruction :"); last.Write();
          throw new Exception("Can't add non-BAL instruction after a BCC");
        }
      }
      instructions.Add(i);
    }
    public IEnumerable<BasicBlock> SuccIter {get{return successors;}}
    public IEnumerable<BasicBlock> PredIter {get{return predecessors;}}
    public IEnumerable<Instr> InstrIter {get{return instructions;}}
    public IEnumerable<Instr> RevInstrIter() {
      int idx = instructions.Count - 1;
      while (idx >= 0) yield return instructions[idx--];
    }
    public Instr GetTerminator() {
      if (InstrCount == 0) return null;
      Instr i = instructions[instructions.Count - 1];
      return (i.IsTerminator()) ? i : null;
    }
    
    public void Write() {
      Console.WriteLine("BB#{0}:",Number);
      if (predecessors.Count > 0) {
        Console.Write("    Predecessors:");
        foreach (BasicBlock bb in predecessors) {
          Console.Write(" BB#{0}",bb.Number);
        }
        Console.WriteLine();
      }
      foreach (Instr i in instructions) {
        Console.Write("    "); i.Write();
      }
      if (successors.Count > 0) {
        Console.Write("    Successors:");
        foreach (BasicBlock bb in successors) {
          Console.Write(" BB#{0}",bb.Number);
        }
        Console.WriteLine();
      }
    }
  }
  
  public class Constant
  {
    string s_val;
    int i_val;
    public uint Index {private set; get;}
    public Type ConstType {private set; get;}

    public Constant(uint index, string valu) {
      ConstType = Type.String_t;
      s_val = valu;
      Index = index;
    }
    public Constant(uint index, int valu) {
      ConstType = Type.String_t;
      i_val = valu;
      Index = index;
    }
    public string GetStr() {return s_val;}
    public int GetInt() {return i_val;}
    public void Write() {
      // cp#0: @verbosity = common global i32 0, align 4
      Console.Write("cp#{0}: {1} ",Index,ConstType);
      if (ConstType == Type.String_t) Console.Write("\"{0}\"",s_val);
      else if (ConstType == Type.Int_t) Console.Write("\"{0}\"",i_val);
      Console.WriteLine();
    }
  }
  public class Function
  {
    public FunctionType FuncType {private set; get;}
    public string Name {private set; get;}
    public int NumArgs {get{return FuncType.NumParameters;}}
    public int BlockCount {get{return blocks.Count;}}
    List<Reg> live_ins = new List<Reg>();
    List<Constant> ConstantPool = new List<Constant>();
    List<BasicBlock> blocks = new List<BasicBlock>();
  
    public Function(string name, FunctionType ty) {
      Name = name; FuncType = ty;
    }

    public void AddLiveIn(Reg r) {live_ins.Add(r);}
    public void Add(BasicBlock block) {blocks.Add(block);}

    public BasicBlock GetBlockNumbered(uint num) {
      foreach (BasicBlock bb in blocks) {
        if (bb.Number == num)
          return bb;
      }
      return null;
    }
    private void JoinBlocks(BasicBlock bb, Instr i, int oper_num) {
      Oper o = i.getOperand(oper_num);
      if (o.IsBB()) {
        BasicBlock sb = ((BB)o).Block;
        bb.AddSuccessor(sb);
        sb.AddPredecessor(bb);
      }
      else {
        o.Write();
        Console.WriteLine();
        throw new Exception(
          String.Format("Expected BB as argument {0} to branch instruction",
            oper_num));
      }
    }
    // Connect edges according to BB arguments to branch instructions
    public void DiscoverEdges() {
      foreach (BasicBlock bb in blocks) {
        Instr i = bb.GetTerminator();
        if (i == null) {
          throw new Exception(
            "Basic Block has no terminator instruction (RET|BAL|BCC)");
        }
        else if (i.Opcode == Instr.op_t.BCC) {
          JoinBlocks(bb,i,1);
          JoinBlocks(bb,i,2);
        }
        else if (i.Opcode == Instr.op_t.BAL) {
          JoinBlocks(bb,i,0);
        }
      }
    }
    public IEnumerator<BasicBlock> BlockIter {
      get{return blocks.GetEnumerator();}
    }
    
    public void Write() {
      Console.WriteLine("# Function {0}:",Name);
      if (ConstantPool.Count > 0) {
        Console.WriteLine("ConstantPool:");
        foreach (Constant c in ConstantPool) {
          Console.Write("  "); c.Write();
        }
        Console.WriteLine();
      }
      if (live_ins.Count > 0) {
        Console.Write("Live In:");
        foreach (Reg r in live_ins) {
          Console.Write(" "); r.Write();
        }
        Console.WriteLine();
      }
      foreach (BasicBlock bb in blocks) {
        bb.Write();
      }
      Console.WriteLine("# End of function {0}.\n",Name);
    }
  }
  public class Module
  {
    List<Function> functions = new List<Function>();
    public String Name {private set; get;}
    public uint NumFunctions {get{return (uint)functions.Count;}}

    public Module(string name) {
      Name = name;
    }

    public void Add(Function f) {functions.Add(f);}

    public Function GetFunction(string name, FunctionType ty) {
      foreach (Function f in functions) {
        if (f.Name.Equals(name) && f.FuncType == ty) {
          return f;
        }
      }
      return null;
    }
    public Function GetFunction(string name, IList<Type> parm_types) {
      foreach (Function f in functions) {
        FunctionType ft = f.FuncType;
        if (f.Name.Equals(name) && ft.CompareParameters(parm_types)) {
          return f;
        }
      }
      return null;
    }

    public void Write() {
      Console.WriteLine("### Module \"{0}\":",Name);
      foreach (Function f in functions) {f.Write();}
      Console.WriteLine("### End of module \"{0}\".",Name);
    }
  }

  // IR Builder
  public class IRBuilder
  {
    uint number_of_registers = 0;
    public uint NumberOfRegisters {get{return number_of_registers;}}
  
    Module module = null;
    Function current_function = null;
    BasicBlock current_block = null;
  
    public IRBuilder(string module_name) {
      module = new Module(module_name);
    }
    void AssertFunctionOpen() {
      if (current_block == null) {
        throw new Exception("A function is not currently open");
      }
    }
    void AssertBlockOpen() {
      if (current_block == null) {
        throw new Exception("A block is not currently open");
      }
    }
    public Function GetFunction(string name, List<Type> arg_ts) {
      return module.GetFunction(name,arg_ts);
    }
    public Module GetModule() {return module;}
  
    public Function GetOrCreateFunction(string name, FunctionType ty) {
      Function f = module.GetFunction(name,ty);
      if (f == null) {
        f = new Function(name,ty);
        module.Add(f);
      }
      return f;
    }
  
    // We duplicate a register as an instruction argument so
    // that we can decorate register operands independently
    // when doing analysis, etc
    Reg DuplicateRegister(Oper op) {
      if (!op.IsReg()) throw new Exception("Not a register");
      Reg reg = (Reg)op;
      return new Reg(reg.Number,reg.IsDef());
    }
  
    public Reg CreateFunctionArgRegister() {
      Reg r = CreateVirtualRegister();
      current_function.AddLiveIn(r);
      return r;
    }
    public Reg CreateVirtualRegister() {
      uint rnum = number_of_registers;
      number_of_registers += 1;
      return new Reg(rnum,false);
    }
    public Reg CreateVirtualRegister(uint rnum) {
      if (rnum > NumberOfRegisters)
        throw new Exception("IRBuilder::CreateVirtualRegiser: the requested number "+rnum+" exceeds the number of registers");
      if (rnum == NumberOfRegisters)
        return CreateVirtualRegister();
      return new Reg(rnum,false);
    }
  
    public BasicBlock CreateBlock() {
      uint num = (uint)current_function.BlockCount;
      BasicBlock b = new BasicBlock(num);
      current_function.Add(b);
      return b;
    }
  
    public void BeginBlock() {
      if (current_block != null) {
        if (current_block.InstrCount == 0)
          return;
      }
      current_block = CreateBlock();
    }
    public void BeginBlock(BasicBlock b) {
      current_block = b;
    }
  
    public void BeginFunction(string name, FunctionType ty) {
      EndFunction();
      current_function = GetOrCreateFunction(name,ty);
      BeginBlock();
      number_of_registers = 0;
    }
  
    public void EndFunction() {
      if (current_function == null) return;
      current_function.DiscoverEdges();
      current_function = null;
      current_block = null;
    }
  
    public Reg CreateBinMATH(Instr.op_t op, Reg lhs, Oper rhs) {
      AssertBlockOpen();
      Reg def = CreateVirtualRegister();
      Instr i = null;
      switch(op) {
        case Instr.op_t.ADD: i = Instr.CreateADD(def,lhs,rhs); break;
        case Instr.op_t.SUB: i = Instr.CreateSUB(def,lhs,rhs); break;
        case Instr.op_t.MUL: i = Instr.CreateMUL(def,lhs,rhs); break;
        default:
          throw new Exception("Not a valid binary math operator '"+op+"'");
      }
      current_block.Add(i);
      return def;
    }
  
    public void CreateBCC(Reg cc, BasicBlock tbb, BasicBlock fbb) {
      current_block.Add(Instr.CreateBCC(cc,new BB(tbb), new BB(fbb)));
    }
    public void CreateBAL(BasicBlock bb) {
      current_block.Add(Instr.CreateBAL(new BB(bb)));
    }
    public Reg CreateADD(Reg lhs, Oper rhs) {return CreateBinMATH(Instr.op_t.ADD,lhs,rhs);}
    public Reg CreateSUB(Reg lhs, Oper rhs) {return CreateBinMATH(Instr.op_t.SUB,lhs,rhs);}
    public Reg CreateMUL(Reg lhs, Oper rhs) {return CreateBinMATH(Instr.op_t.MUL,lhs,rhs);}
    public void CreateRET(Reg rhs) {
      current_block.Add(Instr.CreateRET(rhs));
    }
    public void CreateRET() {
      current_block.Add(Instr.CreateRET());
    }
    public Reg CreateCALL(Function f, List<Oper> args) {
      FunctionType t = f.FuncType;
      Reg r = null;
      Instr i = null;
      if (t.ReturnType != Type.Void_t) {
        r = CreateVirtualRegister();
        i = Instr.CreateCALL(f,r,args);
      }
      else 
        i = Instr.CreateVoidCALL(f,args);
      current_block.Add(i);
      return r;
    }
    public void CreateMOV(Reg def, Oper rhs) {
      Instr i = Instr.CreateMOV(def,rhs);
      current_block.Add(i);
    }
    public Reg CreateCMP(CC.ty cc, Reg lhs, Oper rhs) {
      AssertBlockOpen();
      Reg r = CreateVirtualRegister();
      r.setDef(true);
      Instr i = Instr.CreateCMP(r,cc,lhs,rhs);
      current_block.Add(i);
      return r;
    }
    public Reg CreateCMP(char op, Reg lhs, Oper rhs) {
      CC.ty cc = CC.ty.AL;
      switch(op) {
        case '=': cc = CC.ty.EQ; break;
        case '!': cc = CC.ty.NE; break;
        case '<': cc = CC.ty.LT; break;
        case 'L': cc = CC.ty.LE; break;
        case '>': cc = CC.ty.GT; break;
        case 'G': cc = CC.ty.GE; break;
        default:
          throw new Exception("not a valid comparison operator '"+op+"'");
      }
      return CreateCMP(cc,lhs,rhs);
    }
    
    public Oper CreateImmediate(int value) {
      return new Imm(value);
    }
    
    public Oper CreateString(string s) {
      return null;
    }
  }
}

