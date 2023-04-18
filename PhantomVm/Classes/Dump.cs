using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;
using System;

namespace Cc.Anba.PhantomOs.VirtualMachine.Classes
{
    public class Dump
    {
        public static string PvmClass(PvmClass pvmClass)
        {
            string output = "";

            //this.Name = name;
            //this.Parent = parent;
            //this.IfaceDefault = ifaceDefault;
            //this.MethodNames = methodNames;
            //this.FieldNames = fieldNames;

            output += "class " + pvmClass.Name.Str + " : ";
            var pvmStringBaseClass = pvmClass.Class;
            output += pvmStringBaseClass.Name.Str + Environment.NewLine;
            output += "{" + Environment.NewLine;
            if (pvmClass.FieldNames != null)
            {
                var fieldNames = pvmClass.FieldNames.Array;
                for (int i = 0; i < fieldNames.Count; i++)
                {
                    var fieldName = fieldNames[i];
                    var pvmString = fieldName as PvmString;
                    if (pvmString != null)
                    {
                        output += "    " + pvmString.Str + "; // Field no. " + i + Environment.NewLine;
                    }
                }
            }
            if (pvmClass.MethodNames != null)
            {
                var methodNames = pvmClass.MethodNames.Array;
                for (int i = 0; i < methodNames.Count; i++)
                {
                    var methodName = methodNames[i];
                    var pvmString = methodName as PvmString;
                    if (pvmString != null)
                    {
                        output += "    " + pvmString.Str + " () {} // Method no. " + i + Environment.NewLine;
                    }
                }
            }
            if (pvmClass.IfaceDefault != null)
            {
                
            }
            output += "}" + Environment.NewLine;

            return output;
        }

        public static string PvmObject(PvmObject pvmObject)
        {
            string output = "";

            //this.Class = pvmClass;
            //this.Iface = pvmClass?.IfaceDefault;
            //this.Fields = fields; ;

            var pvmStringObjectClass = pvmObject.Class;
            output += pvmStringObjectClass.Name.Str + " ";
            output += "obj { " + Environment.NewLine;
            if (pvmObject.Fields != null)
            {
                var fields = pvmObject.Fields.Array;
                for (int i = 0; i < fields.Count; i++)
                {
                    var field = fields[i];
                    var fieldClass = field.Class;
                    string className = fieldClass.Name.Str;
                    output += "    " + className + "; // Field no. " + i + Environment.NewLine;
                }
            }
            if (pvmObject.Iface != null)
            {
                var iface = pvmObject.Iface;
                for (int i = 0; i < iface.Methods.Length; i++)
                {
                    var method = iface.Methods[i];
                    if (method != null)
                    {
                        output += "    code " + method.Code.Length + " length // Method no. " + i + Environment.NewLine;
                    }
                }
            }
            output += "}" + Environment.NewLine;

            return output;
        }
    }
}
