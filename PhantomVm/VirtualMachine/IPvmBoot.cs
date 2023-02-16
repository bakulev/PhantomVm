using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;

namespace Cc.Anba.PhantomOs.VirtualMachine
{
    public interface IPvmBoot
    {
        PvmClass LoadPvmClass(PvmRoot root, string filePath);

        void Print();
    }
}
