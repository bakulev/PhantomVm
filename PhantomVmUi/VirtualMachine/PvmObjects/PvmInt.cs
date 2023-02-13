
namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmInt : PvmObject
    {
        #region Fields specific for PvmInt Class in addition to PvmObject fields

        public int Value { get; set; }

        #endregion

        #region Ctor

        public PvmInt(
            PvmClass objectClass,
            int value)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = Class.IfaceDefault;

            // Fill desired fields. 
            this.Value = value;
        }

        public override string ToString() =>
            "PvmInt:" + " \"" + Value + "\"";

        #endregion
    }
}
