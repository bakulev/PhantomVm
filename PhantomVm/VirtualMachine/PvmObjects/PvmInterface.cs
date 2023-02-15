using System;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmInterface : PvmObject
    {
        #region Fields specific for PvmInterface Class in addition to PvmObject fields

        /// <summary>
        /// Code of methods
        /// </summary>
        public PvmCode[] Methods { get; set; }

        #endregion

        #region Ctor

        public PvmInterface(PvmClass objectClass, int nMethods)
            : base(objectClass)

        {
            // Create object of specified class.
            this.Iface = objectClass?.IfaceDefault;
            // Chreate desired array.
            Methods = new PvmCode[nMethods];
        }

        public PvmInterface(PvmClass objectClass, PvmClass baseClass)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass?.IfaceDefault;
            // Chreate desired array.
            Methods = new PvmCode[baseClass.Iface.Methods.Length];
            // Copy methods code from a base class
            Array.Copy(baseClass.Iface.Methods, Methods, baseClass.Iface.Methods.Length);
        }

        public override string ToString() => "PvmInterface";

        #endregion

        #region Methods

        public PvmCode GetMethod(int method)
        {
            return Methods[method];
        }

        public void SetOrdinal(int i, PvmCode pvmCode)
        {
            Methods[i] = pvmCode;
        }

        #endregion
    }
}
