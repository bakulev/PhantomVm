using System.Collections.Generic;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    /// <summary>
    /// In original implementation array class stores elements of
    /// array as fields of that class. I.e. field on N'th position
    /// is N'th element of array.
    /// Array stores values in pages. If volume of used page is
    /// unsufficient then new page with bigger volume will be created.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PvmArray<T> : PvmObject
    {
        #region Fields specific for PvmInterface Class in addition to PvmObject fields

        public List<T> Array { get; set; }

        #endregion

        #region Ctor

        public PvmArray(PvmClass objectClass,
            int nSlots)
            : base(objectClass, null)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;
            // Fill desired fields. 
            Array = new List<T>(nSlots);
        }

        public override string ToString() => "PvmArray";

        #endregion

        #region Methods

        internal int GetLength()
        {
            return Array.Count;
        }

        internal T GetOrdinal(int ordinal)
        {
            return Array[ordinal];
        }

        public void SetOrdinal(int ordinal, T value)
        {
            if (Array.Count <= ordinal)
                for (int i = Array.Count; i <= ordinal; i++)
                    Array.Add(value);
            else
                Array[ordinal] = value;
        }

        internal void Add(T element)
        {
            Array.Add(element);
        }

        internal void Remove(T element)
        {
            Array.Remove(element);
        }

        #endregion
    };
}
