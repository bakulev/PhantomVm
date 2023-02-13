using System;
using System.Text;

namespace Cc.Anba.PhantomOs.VirtualMachine.PvmObjects
{
    public class PvmCode : PvmObject
    {
        #region Fields specific for PvmInt Class in addition to PvmObject fields

        public OpCode[] Code { get; set; }

        #endregion

        #region Ctor

        public PvmCode(PvmClass objectClass,
            OpCode[] code)
            : base(objectClass)
        {
            // Create object of specified class.
            this.Iface = objectClass.IfaceDefault;

            // Chreate desired array.
            this.Code = code;
        }
        public override string ToString() => "PvmCode:";

        #endregion

        #region Methods

        public OpCode GetOpCodeNext(uint ip)
        {
            if (ip < Code.GetLength(0))
            {
                return Code[ip++];
            }
            else
            {
                throw new IndexOutOfRangeException("Unknown OpCode in code");
            }
        }

        internal Int32 GetInt32(uint ip)
        {
            if (ip + 4 < Code.GetLength(0))
            {
                var data = new byte[4];
                for (int i = 0; i < 4; i++)
                    data[i] = (byte)Code[ip + i];
                Array.Reverse(data);
                Int32 ret = BitConverter.ToInt32(data, 0);

                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("Unknown OpCode in code");
            }
        }

        internal string GetString(uint ip)
        {
            var len = GetInt32(ip);
            if (ip + 4 + len < Code.GetLength(0))
            {
                var data = new byte[len];
                for (int i = 0; i < len; i++)
                    data[i] = (byte)Code[ip + 4 + i];
                var str = ASCIIEncoding.ASCII.GetString(data);
                return str;
            }
            else
            {
                throw new IndexOutOfRangeException("Unknown OpCode in code");
            }
        }

        #endregion
    }
}
