using System.Collections.Generic;


public class Symbol
{
  public string name;
  public Type type;
  public Symbol(Type type, string name){
    this.type=type;
    this.name=name;
  }
  public override string ToString() {
    return name + ':' + type;
  }
}

public class SymbolTable
{
  Stack<Symbol> table = new Stack<Symbol>();

  public SymbolTable() { }

  public void Binding(string name, Type t) {
    if (name == null || name.Length == 0)
      return;
    Symbol s = new Symbol(t,name);
    table.Push(s);
  }

  public Symbol LookUp(string name) {
    foreach (Symbol s in table) {
      if (s.name.Equals(name))
        return s;
    }
    return null;
  }

  public void EnterScope() {
    Symbol s = new Symbol(Type.Void_t,"");
    table.Push(s);
  }

  public void ExitScope() {
    if (table.Count > 0) {
      Symbol s = table.Pop();
      while(table.Count > 0 && s.name != "") {
        s = table.Pop();
      }
    }
  }
  public override string ToString() {
    string s = "SymbolTable:\n";
    string tab = "\t";
    s += "{\t";
    foreach (Symbol x in table) {
      if (x.name.Length == 0) {
        s += "{\t";
        tab = tab + "\t";
      }
      else {
        s = s + x.name + ':' + x.type.ToString();
        s = s + '\n' + tab;
      }
    }
    return s;
  }
}




