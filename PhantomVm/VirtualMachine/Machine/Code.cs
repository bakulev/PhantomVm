using Cc.Anba.PhantomOs.VirtualMachine.Machine.Instructions;
using System.Collections.Generic;
using System;

namespace Cc.Anba.PhantomOs.VirtualMachine.Machine
{
    public class Code
    {
        public static List<Instruction> Decode(OpCode[] opCodes)
        {
            var instructions = new List<Instruction>();

            uint codePointer = 141;

            while (codePointer < opCodes.Length)
            {
                var instruction = GetInstruction(opCodes, ref codePointer);
                instructions.Add(instruction);
            }

            return instructions;
        }

        public static Instruction GetInstruction(OpCode[] opCodes, ref uint codePointer)
        {
            Instruction instruction = null;

            uint opcodeAddress = codePointer;
            var opCode = opCodes[codePointer++];

            switch (opCode)
            {
                case OpCode.Call11:
                    {
                        byte numParams = GetByte(opCodes, ref codePointer);
                        return new Call8Bit(opCode, 0x11, numParams);
                    }
                case OpCode.Call8Bit:
                    {
                        byte methodNum = GetByte(opCodes, ref codePointer);
                        byte numParams = GetByte(opCodes, ref codePointer);
                        return new Call8Bit(opCode, methodNum, numParams);
                    }
                default: return null;
            }

            //return new Command();
        }

        internal static Byte GetByte(OpCode[] opCodes, ref uint codePointer)
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
    }
}
