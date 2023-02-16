using System;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmThread : PvmObject
    {
        public PvmCallFrame callFrame;   // current

        // misc data
        int stack_depth;    // number of frames

        public PvmThread(PvmClass objectClass,
            PvmCallFrame newCf)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;
            
            // Fill desired fields.
            callFrame = newCf;
            stack_depth = 1;
        }

        public override string ToString() => "PvmThread";

        public OpCode CodeGetNext()
        {
            if (callFrame.ip < callFrame.code.Code.GetLength(0))
            {
                return callFrame.code.Code[callFrame.ip++];
            }
            else
            {
                throw new IndexOutOfRangeException("Unknown OpCode in code");
            }
        }

        internal Byte CodeGetByte()
        {
            if (callFrame.ip + sizeof(Byte) < callFrame.code.Code.GetLength(0))
            {
                Byte data = (Byte)callFrame.code.Code[callFrame.ip++];
                return data;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetByte out of range");
            }
        }

        internal Int32 CodeGetInt32()
        {
            if (callFrame.ip + sizeof(Int32) < callFrame.code.Code.GetLength(0))
            {
                var data = new byte[sizeof(Int32)];
                for (int i = 0; i < sizeof(Int32); i++)
                    data[i] = (byte)callFrame.code.Code[callFrame.ip + i];
                Array.Reverse(data);
                Int32 ret = BitConverter.ToInt32(data, 0);
                callFrame.ip += sizeof(Int32);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetInt32 out of range");
            }
        }

        internal UInt32 CodeGetUInt32()
        {
            if (callFrame.ip + sizeof(UInt32) < callFrame.code.Code.GetLength(0))
            {
                var data = new byte[sizeof(UInt32)];
                for (int i = 0; i < sizeof(UInt32); i++)
                    data[i] = (byte)callFrame.code.Code[callFrame.ip + i];
                Array.Reverse(data);
                UInt32 ret = BitConverter.ToUInt32(data, 0);
                callFrame.ip += sizeof(UInt32);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetUInt32 out of range");
            }
        }

        internal Int64 CodeGetInt64()
        {
            if (callFrame.ip + sizeof(Int64) < callFrame.code.Code.GetLength(0))
            {
                var data = new byte[sizeof(Int64)];
                for (int i = 0; i < sizeof(Int64); i++)
                    data[i] = (byte)callFrame.code.Code[callFrame.ip + i];
                Array.Reverse(data);
                Int64 ret = BitConverter.ToInt64(data, 0);
                callFrame.ip += sizeof(Int64);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetInt64 out of range");
            }
        }
    };
}
