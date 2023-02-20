using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;

namespace Cc.Anba.PhantomOs.VirtualMachine.Machine.Instructions
{
    public abstract class Instruction
    {
        public OpCode OpCode { get; }

        public abstract void Execute(PvmRoot root, PvmThread pvmThread);

        protected Instruction(OpCode opCode)
        {
            OpCode = opCode;
        }
    }
}
