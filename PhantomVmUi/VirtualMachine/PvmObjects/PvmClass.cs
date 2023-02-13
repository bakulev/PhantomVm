
namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    /// <summary>
    /// Class differs from object by that it no contains exact method
    /// implementations or exact values of fields. It countains only 
    /// it's names.
    /// When object of that class is created it receives separate memory
    /// and defined implementations of methods.
    /// </summary>
    public class PvmClass : PvmObject
    {
        #region Fields specific for PvmString Class in addition to PvmObject fields

        /// <summary>
        /// default interface of object of this class
        /// </summary>
        public PvmInterface IfaceDefault;

        /// <summary>
        /// Name of a Class
        /// </summary>
        public PvmString Name;

        /// <summary>
        /// Reference to a parent Class.
        /// </summary>
        public PvmClass Parent;

        /// <summary>
        /// Array of method names
        /// </summary>
        public PvmArray<PvmString> MethodNames;

        /// <summary>
        /// Array of field names
        /// </summary>
        public PvmArray<PvmString> FieldNames;

        #endregion

        #region Ctor

        /// <summary>
        /// Constructor only for PvmRoot initial process
        /// </summary>
        /// <param name="iface"></param>
        public PvmClass(PvmInterface iface)
            : base(null)
        {
            this.Iface = iface;
        }

        /// <summary>
        /// Standart constructor
        /// Create an object of PvmClass class
        /// parent should be a class of PvmClass.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        public PvmClass(PvmClass objectClass,
            PvmString name, PvmClass parent,
            PvmInterface ifaceDefault)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;
            // Fill desired fields.         
            this.Name = name;
            this.Parent = parent;
            this.IfaceDefault = ifaceDefault;
        }

        /// <summary>
        /// Extended standart constructor
        /// </summary>
        /// <param name="objectClass"></param>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="ifaceDefault"></param>
        /// <param name="methodNames"></param>
        /// <param name="fieldNames"></param>
        public PvmClass(
            PvmClass objectClass,
            PvmString name, PvmClass parent,
            PvmInterface ifaceDefault,
            PvmArray<PvmString> methodNames,
            PvmArray<PvmString> fieldNames)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;
            // Fill desired fields.         
            this.Name = name;
            this.Parent = parent;
            this.IfaceDefault = ifaceDefault;
            this.MethodNames = methodNames;
            this.FieldNames = fieldNames;
        }

        public override string ToString() =>
            "PvmClass:" + " \"" + Name + "\"";

        #endregion
    };
}
