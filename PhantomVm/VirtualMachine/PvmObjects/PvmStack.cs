using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmStack<T> : PvmObject
    {
        // Page array has this many elements.
        private const int _sSize = 64;

        Stack<T> Stack = new Stack<T>(_sSize); // PVM_INTEGER_STACK_SIZE

        public PvmStack(PvmClass objectClass)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;
            // Fill desired fields. 
        }

        public override string ToString() => "PvmStack";

        // Ok. All these methods receive ptr to root stack object da,
        // which curr_da field points to the active page data area.
        // so rootda is root page da, and s is curr page da.

        public void Push(T o)
        {
            Stack.Push(o);
        }

        public T Pop()
        {
            return Stack.Pop();
        }

        internal T GetTop()
        {
            return Stack.Peek();
        }

        internal T GetPos(uint pos)
        {
            return Stack.ElementAt((int)pos);
        }

    };
}
