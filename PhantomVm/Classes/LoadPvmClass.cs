using System;
using System.IO;
using System.Text;
using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;

namespace Cc.Anba.PhantomOs.VirtualMachine.Utils
{
    public class LoadPvmClass
    {
        public static PvmClass Load(PvmRoot root, string filePath)
        {
            PvmString className = null;
            PvmInterface iface = null;
            PvmArray<PvmObject> ip2lineMaps = null; // new PvmArray<PvmObject>();
            PvmArray<PvmString> methodNames = null; // new PvmArray<PvmString>();
            PvmArray<PvmString> fieldNames = null; // new PvmArray<PvmString>();
            //PvmArray constPool = new PvmArray();
            PvmClass newClass = null;
            PvmClass baseClass = null;

            bool gotClassHeader = false;

            using (FileStream fs = File.OpenRead(filePath))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    var start0 = reader.BaseStream.Position;
                    var buffer = reader.ReadBytes(5);
                    var header = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    if (!string.Equals(header, "phfr:"))
                    {
                        Console.WriteLine("No record marker");
                        return newClass;
                    }
                    var type = (char)reader.ReadByte();
                    var size = GetInt32(reader);

                    Console.Write(" type {0}, size {1:d}: ", type, size);
                    if (size < 6 + 8)
                    {
                        Console.WriteLine("Invalid record size\n");
                        return newClass;
                    }
                    var dataSize = size - (int)(reader.BaseStream.Position - start0);
                    switch (type)
                    {
                        case 'C': // class
                            Console.Write("Class is: ");
                            var strClassName = GetString(reader);
                            Console.Write("{0}", strClassName);
                            className = root.NewString(strClassName);
                            var nObjectSlots = GetInt32(reader);
                            Console.Write(", {0:d} fields", nObjectSlots);
                            var nMethodSlots = GetInt32(reader);
                            Console.Write(", {0:d} methods", nMethodSlots);
                            Console.WriteLine();    // terminate string
                            var strBaseName = GetString(reader);
                            Console.Write("Based on: {0}", strBaseName);
                            //PvmObject baseClass;
                            baseClass = root.GetClassByName(strBaseName);
                            // TODO turn on later, when we're sure all class collections have it
                            var strVersion = GetString(reader);
                            gotClassHeader = true;

                            iface = root.NewInterface(baseClass);
                            ip2lineMaps = root.NewArrayObject(0);
                            methodNames = root.NewArrayString(nMethodSlots);
                            fieldNames = root.NewArrayString(nObjectSlots);
                            //constPool = new PvmArray();
                            break;
                        case 'M': // method
                            var startM = reader.BaseStream.Position;
                            var strMethodName = GetString(reader);
                            Console.Write("Method is: {0}", strMethodName);
                            var methodName = root.NewString(strMethodName);
                            var myOrdinal = GetInt32(reader);
                            Console.WriteLine(", ordinal {0:d}", myOrdinal);
                            var ip = reader.BaseStream.Position - startM;
                            var codeSize = dataSize - ip;
                            var codeData = reader.ReadBytes((int)codeSize);
                            var code = new OpCode[codeData.GetLength(0)];
                            for (int i = 0; i < codeData.GetLength(0); i++)
                                code[i] = (OpCode)codeData[i];
                            Console.WriteLine("code size {0:d}, IP = {1:d}, in_size = {2:d}",
                                codeSize, ip, dataSize);
                            // Crate code object from loaded data.
                            var myCode = root.NewCode(code);
                            // Insert code into array of interfaces
                            iface.SetOrdinal(myOrdinal, myCode);
                            // Insert name intor array of names //TODO chech decreasing counter of previuously added object
                            methodNames.SetOrdinal(myOrdinal, methodName);
                            break;
                        case 'S': // method signature
                            Console.WriteLine("meth sig");
                            var strSigName = GetString(reader);
                            var mOrdinal = GetInt32(reader);
                            var mnArgs = GetInt32(reader);
                            // pvm_load_type
                            var isContainer = GetInt32(reader);
                            var className2 = GetString(reader);
                            var containedClassName = GetString(reader);
                            Console.Write("Method is: {0} {1}[{2}] ord {3:d} args {4:d} ( ",
                                strSigName, className2, containedClassName, mOrdinal, mnArgs);
                            for (int i = 0; i < mnArgs; i++)
                            {
                                var strArgName = GetString(reader);
                                var argName = root.NewString(strArgName);
                                Console.Write("{0} : ", strArgName);
                                // pvm_load_type
                                var isaContainer = GetInt32(reader);
                                var classaName2 = GetString(reader);
                                var containedaClassName = GetString(reader);
                                if (i < mnArgs - 1) Console.Write(", ");
                            }
                            Console.WriteLine(")");
                            break;
                        case 'l': // IP to line num map
                            var startl = reader.BaseStream.Position;
                            int ordinal = GetInt32(reader);
                            int mapSize = GetInt32(reader);
                            Console.WriteLine(" line num map size {0:d} for ordinal {1:d}",
                                mapSize, ordinal);
                            for (int i = 0; i < mapSize; i++)
                            {
                                int ipN = GetInt32(reader);
                                int line = GetInt32(reader);
                                Console.WriteLine("map l {0:d} -> ip {1:d}", line, ipN);
                            };
                            var fakeL = dataSize - (reader.BaseStream.Position - startl);
                            var fake = reader.ReadBytes((int)fakeL);
                            break;
                        case 'f': // field names
                            Console.WriteLine("fields");
                            var startf = reader.BaseStream.Position;
                            var fName = GetString(reader);
                            var fOrdinal = GetInt32(reader);
                            while (dataSize - (reader.BaseStream.Position - startf) > 0)
                            {
                                Console.Write("Field '{0}' ord {1:d} type: ",
                                    fName, fOrdinal);
                                // pvm_load_type
                                var isfContainer = GetInt32(reader);
                                var classfName2 = GetString(reader);
                                var containedfClassName = GetString(reader);
                                Console.Write("{0}, {1}, {2}",
                                    isfContainer, classfName2, containedfClassName);
                                // Insert code into array of interfaces
                                fieldNames.SetOrdinal(fOrdinal, root.NewString(fName));
                                //fields.SetOrdinal(fOrdinal, fClass);
                            }
                            Console.WriteLine();
                            break;
                        default:
                            break;
                    }

                    if (!gotClassHeader)
                        return newClass;
                }

            }
            newClass = root.NewClass(className, baseClass, iface,
                    methodNames, fieldNames);

            return newClass;
        }

        private static string GetString(BinaryReader reader)
        {
            int len = GetInt32(reader); // pvm_code_get_int32(code);
            //const unsigned char* sp = code->code + code->IP;
            //code->IP += len;
            //pvm_code_check_bounds(code, code->IP - 1, "get_string");
            // after we checked there is a real data accessible we can
            // create string object
            //return pvm_create_string_object_binary((const char*)sp, len);
            var buffer = reader.ReadBytes(len);
            var str = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return str;
        }

        private static Int32 GetInt32(BinaryReader reader)
        {
            //pvm_code_check_bounds(code, code->IP + int_size() - 1, "get_int32");
            //Int32 ret = reader.ReadInt32();
            var data = reader.ReadBytes(4);
            Array.Reverse(data);
            Int32 ret = BitConverter.ToInt32(data, 0);
            //code->IP += int_size();
            return ret;
        }
    }
}
