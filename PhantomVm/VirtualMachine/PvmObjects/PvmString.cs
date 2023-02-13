
namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmString : PvmObject
    {
        #region Fields specific for PvmString Class in addition to PvmObject fields

        public string Str { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Create an object of PvmString class.
        /// classObject should be a PvmClass of string class.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="classObject"></param>
        public PvmString(PvmClass objectClass, string str)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;
            // Fill desired fields. 
            this.Str = str;
        }

        public override string ToString() =>
            "PvmString:" + " \"" + Str + "\"";

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            return this.Equals(obj as PvmString);
        }

        public bool Equals(PvmString other)
        {
            if (other == null)
                return false;
            //if (object.ReferenceEquals(this, other))
            //    return true;
            //if (this.GetType() != other.GetType())
            //    return false;
            if (this.Str.Equals(other.Str))
                return true;
            else
                return false;
        }

        public bool Equals(string str)
        {
            if (str == null)
                return false;
            //if (object.ReferenceEquals(this, other))
            //    return true;
            //if (this.GetType() != other.GetType())
            //    return false;
            if (this.Str.Equals(str))
                return true;
            else
                return false;
        }

        #endregion
    }
}
