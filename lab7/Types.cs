using System;
using System.Collections.Generic;

public class Type {
  public enum TypeID {VOID=0, INT=1, FLOAT=2, STRING=3, FUNCTION=4}
  private static string[] type_id_strings = new string[]{
                                       "void","int","float",
                                       "string","function"};

  public static string TypeIDToString(TypeID t) {
    if ( t < Type.TypeID.VOID || t > Type.TypeID.FUNCTION)
      return "??";
    return type_id_strings[(int)t];
  }

  
  protected TypeID type_id;
  
  protected Type(TypeID t) {
    type_id = t;
  }

  public Type.TypeID Type_ID { get {return type_id;} }

  // now construct the basic types so there's only one instance of each
  private static Type[] basic_types = new Type[]{
                                            new Type(TypeID.VOID),
                                            new Type(TypeID.INT),
                                            new Type(TypeID.FLOAT),
                                            new Type(TypeID.STRING)
                                          };

  public static Type Get(TypeID tid) {
    if ( tid < Type.TypeID.VOID || tid >= Type.TypeID.FUNCTION)
      throw new Exception("not a primitive TypeID"); 
    return basic_types[(int)tid];
  }

  public static Type Get(string text) {
    for (int i=0;i<(int)TypeID.FUNCTION;++i) {
      if (text.Equals(type_id_strings[i]))
        return basic_types[i];
    }
    return null;
  }

  // basic type property getters
  public static Type Void_t { get { return basic_types[(int)TypeID.VOID];} }
  public static Type Int_t { get { return basic_types[(int)TypeID.INT];} }
  public static Type Float_t { get { return basic_types[(int)TypeID.FLOAT];} }
  public static Type String_t { get { return basic_types[(int)TypeID.STRING];} }

  // function type getter
  public static Type GetFunctionType(Type ret_type, List<Type> param_types) {
    return FunctionType.Get(ret_type,param_types);
  }
  public virtual bool IsFunction() {return false;}
  public bool IsVoid() {return type_id == TypeID.VOID;}
  public bool IsInt() {return type_id == TypeID.INT;}
  public bool IsFloat() {return type_id == TypeID.FLOAT;}
  public bool IsString() {return type_id == TypeID.STRING;}

  public virtual bool Equals(Type t) {
    return type_id == t.Type_ID;
  }
  public override string ToString() {
    return type_id_strings[(int)type_id];
  }
}

public class FunctionType : Type
{
  static List<FunctionType> created_function_types = new List<FunctionType>();

  // Function Type Construction
  public static FunctionType Get(Type ret_type, List<Type> param_types) {
    if (ret_type == null) throw new Exception("ret_type is null");
    foreach (FunctionType f in created_function_types) {
      if (f.Equals(ret_type,param_types)) {
        return f;
      }
    }
    FunctionType fn = new FunctionType(ret_type,param_types);
    created_function_types.Add(fn);
    return fn;
  }

  // function type data
  Type[] param_types;
  Type return_type;

  public override bool Equals(Type t) {
    if (t == this) return true;
    if (t.Type_ID != TypeID.FUNCTION) return false;
    FunctionType f = (FunctionType)t;
    return f.Equals(return_type,param_types);
  }

  public bool Equals(Type ret_t, IList<Type> parameters_t) {
    if (!CompareParameters(parameters_t)) return false;
    if (!return_type.Equals(ret_t)) return false;
    return true;
  }

  public bool CompareParameters(IList<Type> parameters_t) {
    if (param_types.Length != parameters_t.Count) return false;
    if (param_types.Length == 0) return true;
    int i = 0;
    foreach (Type t in parameters_t) {
      Type a = param_types[i];
      if (a == null) {
        throw new Exception("It's all gone wrong");
      }
      if (!t.Equals(a)) return false;
      i += 1;
    }
    return true;
  }

  // private constructor
  private FunctionType(Type ret_type, List<Type> parameter_types) : base (TypeID.FUNCTION) {
    return_type = ret_type;
    param_types = new Type[parameter_types.Count];
    if (parameter_types.Count > 0) {
      int idx = 0;
      foreach (Type t in parameter_types) {
        param_types[idx++] = t;
      }
    }
  }

  public int NumParameters {get{return (param_types == null) ? 0 : param_types.Length;}}
  public Type ReturnType {get {return return_type;} }

  public override bool IsFunction() {return true;}

  public override string ToString() {
    string p = "";
    if (param_types.Length == 0)
      p = TypeIDToString(Type.TypeID.VOID);
    else if (param_types.Length == 1)
      p = param_types[0].ToString();
    else {
      p = "(";
      bool sep = false;
      foreach (Type t in param_types) {
        if (sep) p += "*";
        else sep=true;
        p += t.ToString();
      }
      p += ")";
    }
    return p + " -> " + return_type.ToString();
  }
}

