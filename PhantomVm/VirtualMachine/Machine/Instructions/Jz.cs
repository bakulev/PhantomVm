using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;
using System;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Threading;

namespace Cc.Anba.PhantomOs.VirtualMachine.Machine.Instructions
{
    public class Jz : Instruction
    {
        public int ShiftPos { get; set; }

        public Jz(OpCode opCode, int shiftPos)
            : base((OpCode)Enum.Parse(typeof(OpCode), nameof(Jz)))
        {
            // Additional check against wrong OpCodes decoding.
            if (opCode != OpCode) throw new InvalidCastException();
            // Set instruction parameters.
            ShiftPos = shiftPos;
        }

        public override void Execute(PvmRoot pvmRoot, PvmThread pvmThread)
        {
            uint ip = pvmThread.callFrame.ip;
            var test = pvmThread.callFrame.iStack.Pop();
            if (test == 0)
                pvmThread.callFrame.ip = (uint)(ip + ShiftPos);
        }
    }
}
