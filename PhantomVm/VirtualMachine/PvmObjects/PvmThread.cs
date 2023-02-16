using System;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmThread : PvmObject
    {
        public PvmCode code;
        public uint ip;   // Instruction Pointer 
        public uint ipMax;

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
            if (ip < code.Code.GetLength(0))
            {
                return code.Code[ip++];
            }
            else
            {
                throw new IndexOutOfRangeException("Unknown OpCode in code");
            }
        }

        internal Byte CodeGetByte()
        {
            if (ip + sizeof(Byte) < code.Code.GetLength(0))
            {
                Byte data = (Byte)code.Code[ip++];
                return data;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetByte out of range");
            }
        }

        internal Int32 CodeGetInt32()
        {
            if (ip + sizeof(Int32) < code.Code.GetLength(0))
            {
                var data = new byte[sizeof(Int32)];
                for (int i = 0; i < sizeof(Int32); i++)
                    data[i] = (byte)code.Code[ip + i];
                Array.Reverse(data);
                Int32 ret = BitConverter.ToInt32(data, 0);
                ip += sizeof(Int32);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetInt32 out of range");
            }
        }

        internal UInt32 CodeGetUInt32()
        {
            if (ip + sizeof(UInt32) < code.Code.GetLength(0))
            {
                var data = new byte[sizeof(UInt32)];
                for (int i = 0; i < sizeof(UInt32); i++)
                    data[i] = (byte)code.Code[ip + i];
                Array.Reverse(data);
                UInt32 ret = BitConverter.ToUInt32(data, 0);
                ip += sizeof(UInt32);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetUInt32 out of range");
            }
        }

        internal Int64 CodeGetInt64()
        {
            if (ip + sizeof(Int64) < code.Code.GetLength(0))
            {
                var data = new byte[sizeof(Int64)];
                for (int i = 0; i < sizeof(Int64); i++)
                    data[i] = (byte)code.Code[ip + i];
                Array.Reverse(data);
                Int64 ret = BitConverter.ToInt64(data, 0);
                ip += sizeof(Int64);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetInt64 out of range");
            }
        }
    };
}
