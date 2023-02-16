using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmBoot : PvmObject
    {
        //private PvmBootSc _scBoot;
        public PvmBoot(PvmClass objectClass, IPvmBoot systemCalls)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;
            // Fill desired fields. 
            // System calls.
            //_systemCalls = systemCalls;
            /*
            if (sysCalls.GetLength(0) < 17)
            {
                var sysCallsNew = new Func<PvmObject, PvmThread, bool>[17];
                Array.Copy(sysCalls, sysCallsNew, sysCalls.GetLength(0));
                sysCalls = sysCallsNew;
            }
            sysCalls[5] = _systemCalls.ToString;
            sysCalls[8] = _systemCalls.LoadClass;
            sysCalls[16] = _systemCalls.Print;
            */
        }

        public override string ToString() => "PvmBoot";
    };
}
