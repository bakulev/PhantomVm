
namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmNull : PvmObject
    {
        #region Ctor

        public PvmNull() : base(null)
        {
        }

        public override string ToString() => "PvmNull";

        #endregion
    }
}
