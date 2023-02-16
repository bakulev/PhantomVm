
using System.Threading;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    /// <summary>
    /// This is object itself.
    /// NB! See JIT assembly hardcode for object structure offsets
    /// </summary>
    public class PvmObject
    {
        // This structure must be first in any object
        // for garbage collector to work ok
        // TODO add two bytes after flags to assure alignment
        #region Header of all objects

        /// <summary>
        /// Marker at a start is needed to simplify searching an objects in a memory.
        /// </summary>
        private uint _marker = 0x7FAA7F55; // PVM_OBJECT_START_MARKER 0x7FAA7F55

        /// <summary>
        /// for fast dealloc of locally-owned objects. If grows to INT_MAX it will be fixed 
        /// at that value and ignored further. Such objects will be GC'ed in usual way
        /// </summary>
        private uint _refCount;

        /// <summary>
        /// Reference to a type of an object.
        /// </summary>
        public PvmClass Class { get; set; }

        public PvmInterface Iface { get; set; }

        public PvmArray<PvmObject> Fields { get; set; }

        #endregion

        #region Ctor

        public PvmObject(PvmClass pvmClass, PvmArray<PvmObject> fields = null)
        {
            // Create object of specified class.
            this.Class = pvmClass;
            this.Iface = pvmClass?.IfaceDefault;
            this.Fields = fields;
        }
        public override string ToString() =>
            "PvmObject: " + " \"" + "\" of " + Class;

        #endregion

        #region Methods

        virtual public void SysCall(PvmRoot root, int sysIndex, PvmObject thisObject, PvmThread thread)
        {

        }

        #endregion
    }
}
