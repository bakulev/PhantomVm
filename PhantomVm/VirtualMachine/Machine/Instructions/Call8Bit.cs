using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;
using System;
using System.Reflection.Emit;

namespace Cc.Anba.PhantomOs.VirtualMachine.Machine.Instructions
{
    public class Call8Bit : Instruction
    {
        public byte MethodNum { get; set; }
        public byte NumParams { get; set; }

        public Call8Bit(OpCode opCode, byte methodNum, byte numParams)
            : base(opCode)
        {
            // Additional check against wrong OpCodes decoding.
            switch (opCode) {
                case OpCode.Call00:
                case OpCode.Call01:
                case OpCode.Call02:
                case OpCode.Call03:
                case OpCode.Call04:
                case OpCode.Call05:
                case OpCode.Call06:
                case OpCode.Call07:
                case OpCode.Call08:
                case OpCode.Call09:
                case OpCode.Call0A:
                case OpCode.Call0B:
                case OpCode.Call0C:
                case OpCode.Call0D:
                case OpCode.Call0E:
                case OpCode.Call0F:
                case OpCode.Call10:
                case OpCode.Call11:
                case OpCode.Call12:
                case OpCode.Call13:
                case OpCode.Call14:
                case OpCode.Call15:
                case OpCode.Call16:
                case OpCode.Call17:
                case OpCode.Call18:
                case OpCode.Call19:
                case OpCode.Call1A:
                case OpCode.Call1B:
                case OpCode.Call1C:
                case OpCode.Call1D:
                case OpCode.Call1E:
                case OpCode.Call1F:
                case OpCode.Call8Bit:
                case OpCode.Call32Bit:
                    break;
                default: throw new InvalidCastException();
            }
            // Set instruction parameters.
            MethodNum = methodNum;
            NumParams = numParams;
        }

        public override void Execute(PvmRoot pvmRoot, PvmThread pvmThread)
        {
            // Create call frame
            var newCf = pvmRoot.NewCallFrame(pvmThread.callFrame.thisObject, (int)MethodNum, null);

            // Prepare argiments
            for (uint i = NumParams; i > 0; i--)
            {
                var o = pvmThread.callFrame.oStack.Pop();
                newCf.oStack.Push(o);
            }
            newCf.iStack.Push(NumParams);

            newCf.prev = pvmThread.callFrame; // For ret able to return previous call frame.
            pvmThread.callFrame = newCf; // Swith to new call frame
        }
    }
}
