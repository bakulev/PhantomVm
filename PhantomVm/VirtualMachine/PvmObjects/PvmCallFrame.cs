using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmCallFrame : PvmObject
    {
        public PvmStack<Int64> iStack; // integer (fast calc) stack
        public PvmStack<PvmObject> oStack;   // main object stack
        public PvmStack<Tuple<PvmObject, uint>> eStack;   // exception catchers

        public uint ipMax;    // size of code in bytes
        public PvmCode code;    // (byte)code itself
        public uint ip;
        public PvmObject thisObject;
        public PvmCallFrame prev; // where to return!
        int ordinal;    // num of method we run

        public PvmCallFrame(PvmClass objectClass,
            PvmStack<Int64> iStack, PvmStack<PvmObject> oStack,
            PvmStack<Tuple<PvmObject, uint>> eStack
            )
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;

            // Fill desired fields. 
            this.iStack = iStack;
            this.oStack = oStack;
            this.eStack = eStack;
        }

        public override string ToString() => "PvmCallFrame";
    };
}
