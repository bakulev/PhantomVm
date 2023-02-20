using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Cc.Anba.PhantomOs.VirtualMachine.Classes
{
    public class CodeLoader
    {
        public static string Decompile(OpCode[] opCodes)
        {
            string output = "";

            for (uint codePointer = 0; codePointer < opCodes.Length; codePointer++)
            {
                OpCode opCode = opCodes[codePointer];
                uint opcodeAddress;

                if (((byte)opCode & 0xa0) == 0xa0)
                {
                    opcodeAddress = codePointer;
                    int method_index = 0x0F & (byte)opCode;
                    int n_param = (byte)(opCodes[++codePointer]) & 0xff;
                    output += "        " + opcodeAddress + ":\t";
                    output += opCode.ToString() + " " + method_index + " " + n_param + Environment.NewLine;
                    continue;
                }

                switch (opCode)
                {
                    // single opcode

                    case OpCode.Nop:
                    case OpCode.Ret:
                    case OpCode.IsDup:
                    case OpCode.IsDrop:
                    case OpCode.OsDup:
                    case OpCode.OsDrop:
                    case OpCode.Iconst0:
                    case OpCode.Iconst1:
                    case OpCode.SummonThread:
                    case OpCode.SummonThis:
                    case OpCode.SummonNull:
                    case OpCode.i2o:
                    case OpCode.o2i:
                    case OpCode.Isum:
                    case OpCode.Imul:
                    case OpCode.Isubul:
                    case OpCode.Isublu:
                    case OpCode.Idivul:
                    case OpCode.Idivlu:
                    case OpCode.New:
                    case OpCode.Copy:
                    case OpCode.ior:
                    case OpCode.iand:
                    case OpCode.ixor:
                    case OpCode.inot:
                    case OpCode.log_or:
                    case OpCode.log_and:
                    case OpCode.log_xor:
                    case OpCode.log_not:
                    case OpCode.igt:
                    case OpCode.ilt:
                    case OpCode.ige:
                    case OpCode.ile:
                    case OpCode.SummonClassClass:
                    case OpCode.SummonInterfaceClass:
                    case OpCode.SummonCodeClass:
                    case OpCode.SummonIntClass:
                    case OpCode.SummonStringClass:
                    case OpCode.SummonArrayClass:
                    case OpCode.OsDecompose:
                    case OpCode.Throw:
                    case OpCode.PopCatcher:
                    case OpCode.OsEq:
                    case OpCode.OsNeq:
                    //case OpCode.os_push_null :
                    case OpCode.OsIsnull:
                    case OpCode.ShortCall0:
                    case OpCode.ShortCall1:
                    case OpCode.ShortCall2:
                    case OpCode.ShortCall3:
                        {
                            opcodeAddress = codePointer;
                            output += "        " + opcodeAddress + ":\t";
                            output += opCode.ToString() + Environment.NewLine;
                            break;
                        }

                    // int32 argument

                    case OpCode.Jmp:
                    case OpCode.Djnz:
                    case OpCode.Jz:
                        {
                            opcodeAddress = codePointer;
                            int argInt32 = GetInt32(opCodes, codePointer + 1);
                            codePointer += 4;
                            output += "        " + opcodeAddress + ":\t";
                            output += opCode.ToString() + " " + argInt32;
                            output += "  // " + (opcodeAddress + 1 + argInt32) + ":" + Environment.NewLine;
                            break;
                        }

                    case OpCode.Iconst32bit:
                    case OpCode.OsLoad32:
                    case OpCode.OsSave32:
                    case OpCode.OsCompose32:
                    case OpCode.PushCatcher:
                    case OpCode.OsPull32:
                    case OpCode.OsGet32:
                    case OpCode.OsSet32:
                    case OpCode.IsGet32:
                    case OpCode.IsSet32:
                        {
                            opcodeAddress = codePointer;
                            int argInt32 = GetInt32(opCodes, codePointer + 1);
                            codePointer += 4;
                            output += "        " + opcodeAddress + ":\t";
                            output += opCode.ToString() + " " + argInt32 + Environment.NewLine;
                            break;
                        }

                    // byte argument

                    case OpCode.Iconst8bit:
                    case OpCode.OsLoad8:
                    case OpCode.OsSave8:
                        {
                            opcodeAddress = codePointer;
                            byte argByte = GetByte(opCodes, codePointer + 1);
                            codePointer++;
                            output += "        " + opcodeAddress + ":\t";
                            output += opCode.ToString() + " " + argByte + Environment.NewLine;
                            break;
                        }

                    // byte and int32 arguments

                    case OpCode.Call8Bit:
                        {
                            opcodeAddress = codePointer;
                            byte argByte = GetByte(opCodes, codePointer + 1);
                            codePointer++;
                            int argInt32 = GetInt32(opCodes, codePointer + 1);
                            codePointer += 4;
                            output += "        " + opcodeAddress + ":\t";
                            output += opCode.ToString() + " " + argByte + " " + argInt32 + Environment.NewLine;
                            break;
                        }

                    // int32 and int32 arguments

                    case OpCode.Call32Bit:
                        {
                            opcodeAddress = codePointer;
                            int argInt32 = GetInt32(opCodes, codePointer + 1);
                            codePointer += 4;
                            int arg2Int32 = GetInt32(opCodes, codePointer + 1);
                            codePointer += 4;
                            output += "        " + opcodeAddress + ":\t";
                            output += opCode.ToString() + " " + argInt32 + " " + arg2Int32 + Environment.NewLine;
                            break;
                        }

                    case OpCode.Switch:
                        {
                            opcodeAddress = codePointer;
                            int tableSize = GetInt32(opCodes, codePointer + 1);
                            codePointer += 4;
                            int shift = GetInt32(opCodes, codePointer + 1);
                            codePointer += 4;
                            int divisor = GetInt32(opCodes, codePointer + 1);
                            codePointer += 4;
                            output += "        " + opcodeAddress + ":\t";
                            output += opCode.ToString() + " table-size:" + tableSize +
                                    " " + shift + "/" + divisor + Environment.NewLine;
                            for (int i = 0; i < tableSize; i++)
                            {
                                int argInt32 = GetInt32(opCodes, codePointer + 1);
                                codePointer += 4;
                                output += "            " + argInt32 + Environment.NewLine;
                            }
                            break;
                        }

                    case OpCode.SconstBin:                // string argument
                    case OpCode.SummonByName:
                        {
                            opcodeAddress = codePointer;
                            string str = GetString(opCodes, codePointer + 1);

                            codePointer += (uint)(4 + str.Length);
                            output += "        " + opcodeAddress + ":\t";
                            output += opCode.ToString() + " [" + str.Length + "]\"";
                            output += str.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
                            output += "\"" /*+ "  // ["
                            + argBin.length + "] " + getHexStr(argBin)*/  + Environment.NewLine;
                            break;
                        }

                    default:
                        output += "Unknown opcode " + opCodes[codePointer] + Environment.NewLine;
                        break;
                }
            }

            return output;
        }

        internal static Byte GetByte(OpCode[] opCodes, uint codePointer)
        {
            if (codePointer + sizeof(Byte) < opCodes.Length)
            {
                Byte data = (Byte)opCodes[codePointer++];
                return data;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetByte out of range");
            }
        }

        internal static Int32 GetInt32(OpCode[] opCodes, uint codePointer)
        {
            if (codePointer + sizeof(Int32) < opCodes.Length)
            {
                var data = new byte[sizeof(Int32)];
                for (int i = 0; i < sizeof(Int32); i++)
                    data[i] = (byte)opCodes[codePointer + i];
                Array.Reverse(data);
                Int32 ret = BitConverter.ToInt32(data, 0);
                codePointer += sizeof(Int32);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetInt32 out of range");
            }
        }

        internal static UInt32 GetUInt32(OpCode[] opCodes, uint codePointer)
        {
            if (codePointer + sizeof(UInt32) < opCodes.Length)
            {
                var data = new byte[sizeof(UInt32)];
                for (int i = 0; i < sizeof(UInt32); i++)
                    data[i] = (byte)opCodes[codePointer + i];
                Array.Reverse(data);
                UInt32 ret = BitConverter.ToUInt32(data, 0);
                codePointer += sizeof(UInt32);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetUInt32 out of range");
            }
        }

        internal static Int64 GetInt64(OpCode[] opCodes, uint codePointer)
        {
            if (codePointer + sizeof(Int64) < opCodes.Length)
            {
                var data = new byte[sizeof(Int64)];
                for (int i = 0; i < sizeof(Int64); i++)
                    data[i] = (byte)opCodes[codePointer + i];
                Array.Reverse(data);
                Int64 ret = BitConverter.ToInt64(data, 0);
                codePointer += sizeof(Int64);
                return ret;
            }
            else
            {
                throw new IndexOutOfRangeException("CodeGetInt64 out of range");
            }
        }

        internal static string GetString(OpCode[] opCodes, uint codePointer)
        {
            var len = GetInt32(opCodes, codePointer);
            if (codePointer + 4 + len < opCodes.Length)
            {
                var data = new byte[len];
                for (int i = 0; i < len; i++)
                    data[i] = (byte)opCodes[codePointer + 4 + i];
                var str = ASCIIEncoding.ASCII.GetString(data);
                return str;
            }
            else
            {
                throw new IndexOutOfRangeException("Unknown OpCode in code");
            }
        }
    }
}
