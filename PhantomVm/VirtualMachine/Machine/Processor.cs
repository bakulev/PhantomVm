using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cc.Anba.PhantomOs.VirtualMachine.Machine
{
    /// <summary>
    /// CPU is never instanciated, but is "always" there...like a real CPU. :)  It holds <see cref="physicalMemory"/> 
    /// and the <see cref="registers"/>.  It also provides a mapping from <see cref="Instruction"/>s to SystemCalls in 
    /// the <see cref="OS"/>.  
    /// </summary>
    public class Processor
    {
        bool phantom_virtual_machine_snap_request;
        bool prefix_long = false;
        bool prefix_float = false;
        bool prefix_double = false;

        public Processor()
        { 
        }

        public void MakeStep(PvmRoot root, PvmThread thread)
        {
                OpCode instruction = thread.CodeGetNext();
                Process(root, thread, instruction);
                Debug.WriteLine("instr 0x{0:X} ", instruction);
        }

        public void Process(PvmRoot root, PvmThread thread, OpCode instruction)
        {
            PvmRoot _root = root;

            if (prefix_long)
            {
                prefix_long = false;
                ProcessPrefixLong(root, thread, instruction);
                // End of long ops
                goto noops;
            } // if(prefix_long)

            if (prefix_float)
            {
                prefix_float = false;
                ProcessPrefixFloat(root, thread, instruction);
                // End of float ops
                goto noops;
            } // if(prefix_float)

            if (prefix_double)
            {
                prefix_double = false;
                ProcessPrefixDouble(root, thread, instruction);
                // End of double ops
                goto noops;
            } // if(prefix_double)

        noprefix:
            ProcessNoPrefix(root, thread, instruction);
        // NB! We have continue after instructions that set prefix (long/float/double) so that
        // control does not reach here and we do not reset prefix and do not print warning

        noops:
            // TODO can't happen
            if (prefix_long || prefix_float || prefix_double)
                Console.WriteLine("Unused type prefix on op code 0x%X\n", instruction);

            prefix_long = prefix_float = prefix_double = false;
        }

        public static void ProcessPrefixLong(PvmRoot root, PvmThread thread, OpCode instruction)
        {
            PvmRoot _root = root;

            switch (instruction)
            {
                default:
                    {/*
                        prefix_long = 1;  // attempt nonprefix impl of inctruction, maybe it checks for a modifier
                        goto noprefix;
                    */
                    }
                    break;

                case OpCode.Ishl:
                    LISTI("l-ishl");
                    {/*
                        int64_t val = ls_pop();
                        ls_push(val << is_pop());
                    */
                    }
                    break;

                case OpCode.Ishr:
                    LISTI("l-ishr");
                    {/*
                        int64_t val = ls_pop();
                        ls_push(val >> is_pop());
                    */
                    }
                    break;

                case OpCode.Ushr:
                    LISTI("l-ushr");
                    {/*
                        uint64_t val = ls_pop();
                        ls_push(val >> is_pop());
                    */
                    }
                    break;

                case OpCode.Isum:
                    LISTI("l-isum");
                    {/*
                        int64_t add = ls_pop();
                        ls_push(ls_pop() + add);
                    */
                    }
                    break;

                case OpCode.Imul:
                    LISTI("l-imul");
                    {/*
                        int64_t mul = ls_pop();
                        ls_push(ls_pop() * mul);
                    */
                    }
                    break;

                case OpCode.Isubul:
                    LISTI("l-isubul");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        ls_push(u - l);
                    */
                    }
                    break;

                case OpCode.Isublu:
                    LISTI("l-isublu");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        ls_push(l - u);
                    */
                    }
                    break;

                case OpCode.Idivul:
                    LISTI("l-idivul");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        ls_push(u / l);
                    */
                    }
                    break;

                case OpCode.Idivlu:
                    LISTI("l-idivlu");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        ls_push(l / u);
                    */
                    }
                    break;

                case OpCode.ior:
                    LISTI("l-ior");
                    {/*
                        int64_t operand = ls_pop();
                        ls_push(ls_pop() | operand);
                    */
                    }
                    break;

                case OpCode.iand:
                    LISTI("l-iand");
                    {/*
                        int64_t operand = ls_pop();
                        ls_push(ls_pop() & operand);
                    */
                    }
                    break;

                case OpCode.ixor:
                    LISTI("l-ixor");
                    {/*
                        int64_t operand = ls_pop();
                        ls_push(ls_pop() ^ operand);
                    */
                    }
                    break;

                case OpCode.inot:
                    LISTI("l-inot");
                    {/*
                        int64_t operand = ls_pop();
                        ls_push(~operand);
                    */
                    }
                    break;

                case OpCode.log_or:
                    LISTI("l-lor");
                    {/*
                        int64_t o1 = ls_pop();
                        int64_t o2 = ls_pop();
                        ls_push(o1 || o2);
                    */
                    }
                    break;

                case OpCode.log_and:
                    LISTI("l-land");
                    {/*
                        int64_t o1 = ls_pop();
                        int64_t o2 = ls_pop();
                        ls_push(o1 && o2);
                    */
                    }
                    break;

                case OpCode.log_xor:
                    LISTI("l-lxor");
                    {/*
                        int64_t o1 = ls_pop() ? 1 : 0;
                        int64_t o2 = ls_pop() ? 1 : 0;
                        ls_push(o1 ^ o2);
                    */
                    }
                    break;

                case OpCode.log_not:
                    LISTI("l-lnot");
                    {/*
                        int64_t operand = ls_pop();
                        ls_push(!operand);
                    */
                    }
                    break;

                // NB! Returns int!
                case OpCode.ige:    // >=
                    LISTI("l-ige");
                    {/*
                        int64_t operand = ls_pop();
                        is_push(ls_pop() >= operand);
                    */
                    }
                    break;
                case OpCode.ile:    // <=
                    LISTI("l-ile");
                    {/* 
                        int64_t operand = ls_pop();
                        is_push(ls_pop() <= operand);
                    */
                    }
                    break;

                case OpCode.igt:    // >
                    LISTI("l-igt");
                    {/*
                        int64_t operand = ls_pop();
                        is_push(ls_pop() > operand);
                    */
                    }
                    break;

                case OpCode.ilt:    // <
                    LISTI("l-ilt");
                    {/*
                        int64_t operand = ls_pop();
                        is_push(ls_pop() < operand);
                    */
                    }
                    break;

                case OpCode.FromL:
                    LISTI("l-froml (nop)");
                    break;

                case OpCode.FromI:
                    LISTI("l-fromi");
                    {/*
                        ls_push(is_pop());
                    */
                    }
                    break;

                case OpCode.FromD:
                    LISTI("l-fromd");
                    {/*
                        long l = ls_pop();
                        double d = TO_DOUBLE(l);
                        ls_push((long)d);
                    */
                    }
                    break;

                case OpCode.FromF:
                    LISTI("l-fromf");
                    {/*
                        int i = is_pop();
                        float f = TO_FLOAT(i);
                        ls_push((long)f);
                    */
                    }
                    break;

                case OpCode.i2o:
                    LISTI("l-i2o");
                    {/*
                        os_push(pvm_create_long_object(ls_pop()));
                    */
                    }
                    break;

                case OpCode.o2i:
                    LISTI("l-o2i");
                    {/*

                        struct pvm_object o = os_pop();
                        //if (o.data == 0) pvm_exec_panic("l-o2i(null)");
                        //ls_push(pvm_get_long(o));
                        //ls_push(((int64_t)(((struct data_area_4_long *)&(o.data->da))->value)));
                        pvm_lstack_push(da->_istack, ((int64_t)(((struct data_area_4_long *)&(o.data->da))->value)));
                        ref_dec_o(o);
                    */
                    }
                    break;
            }
        }

        public void ProcessPrefixFloat(PvmRoot root, PvmThread thread, OpCode instruction)
        {
            PvmRoot _root = root;

            switch (instruction)
            {
                default: // Try classic implementation of that op
                    {/*
                        prefix_float = 1; // attempt nonprefix impl of inctruction, maybe it checks for a modifier
                        goto noprefix;
                        //pvm_exec_panic("invalid double op");
                    */
                    }
                    break;

                case OpCode.Ishl: // Not defined for float, throw exception
                case OpCode.Ishr:
                case OpCode.Ushr:
                case OpCode.ior:
                case OpCode.iand:
                case OpCode.ixor:
                case OpCode.inot:
                case OpCode.log_or:
                case OpCode.log_and:
                case OpCode.log_xor:
                case OpCode.log_not:
                    //pvm_exec_panic("invalid float op");
                    break;

                case OpCode.Isum:
                    LISTI("f-isum");
                    {/*
                        //FLOAT_STACK_OP(+);
                        do
                        {
                            int32_t a1 = is_pop();
                            int32_t a2 = is_pop();
                            //float r = AS_FLOAT(a1, a2, +); 
                            float r = (*((float*)&(a1))) + (*((float*)&(a2)));
                            //is_push(TO_INT(r));
                            //is_push((*((u_int32_t *)&(r))));
                            pvm_istack_push(da->_istack, (*((uint32_t*)&(r))));
                        } while (0);
                    */
                    }
                    break;

                case OpCode.Imul:
                    LISTI("f-imul");
                    {/*
                        FLOAT_STACK_OP(*);
                    */
                    }
                    break;

                case OpCode.Isubul:
                    LISTI("f-isubul");
                    {/*
                        int32_t u = is_pop();
                        int32_t l = is_pop();
                        float r = AS_FLOAT(u, l, -);
                        is_push(TO_INT(r));
                    */
                    }
                    break;

                case OpCode.Isublu:
                    LISTI("f-isublu");
                    {/*
                        int32_t u = is_pop();
                        int32_t l = is_pop();
                        float r = AS_FLOAT(l, u, -);
                        is_push(TO_INT(r));
                    */
                    }
                    break;

                case OpCode.Idivul:
                    LISTI("f-idivul");
                    {/*
                        int32_t u = is_pop();
                        int32_t l = is_pop();
                        float r = AS_FLOAT(u, l, / );
                        is_push(TO_INT(r));
                    */
                    }
                    break;

                case OpCode.Idivlu:
                    LISTI("f-idivlu");
                    {/*
                        int32_t u = is_pop();
                        int32_t l = is_pop();
                        float r = AS_FLOAT(l, u, / );
                        is_push(TO_INT(r));
                    */
                    }
                    break;

                // NB! Returns int!
                case OpCode.ige:
                    LISTI("f-ige");
                    {/*
                        int32_t u = is_pop();
                        int32_t l = is_pop();
                        int r = AS_FLOAT(l, u, >= );
                        is_push(r);
                    */
                    }
                    break;
                case OpCode.ile:
                    LISTI("f-ile");
                    {/*
                        int32_t u = is_pop();
                        int32_t l = is_pop();
                        int r = AS_FLOAT(l, u, <= );
                        is_push(r);
                    */
                    }
                    break;
                case OpCode.igt:
                    LISTI("f-igt");
                    {/*
                        int32_t u = is_pop();
                        int32_t l = is_pop();
                        int r = AS_FLOAT(l, u, >);
                        is_push(r);
                    */
                    }
                    break;
                case OpCode.ilt:
                    LISTI("f-ilt");
                    {/*
                        int32_t u = is_pop();
                        int32_t l = is_pop();
                        int r = AS_FLOAT(l, u, <);
                        is_push(r);
                    */
                    }
                    break;


                case OpCode.FromF:
                    LISTI("f-fromf (nop)");
                    break;

                case OpCode.FromI:
                    LISTI("f-fromi");
                    {/*
                        float i = is_pop();
                        is_push(TO_INT(i));
                    */
                    }
                    break;

                case OpCode.FromL:
                    LISTI("f-froml");
                    {/*
                        float l = ls_pop();
                        is_push(TO_INT(l));
                    */
                    }
                    break;

                case OpCode.FromD:
                    LISTI("f-fromd");
                    {/*
                        int64_t l = ls_pop();
                        float f = (float)TO_DOUBLE(l);
                        is_push(TO_INT(f));
                    */
                    }
                    break;

                case OpCode.i2o:
                    LISTI("f-i2o");
                    {/*
                        //pvm_exec_panic("unimpl float i2o");
                        int32_t d = is_pop();
                        os_push(pvm_create_float_object(TO_FLOAT(d)));
                    */
                    }
                    break;

                case OpCode.o2i:
                    LISTI("f-o2i");
                    //pvm_exec_panic("unimpl float o2i");
                    {/*

                        struct pvm_object o = os_pop();
                        //if (o.data == 0) pvm_exec_panic("f-o2i(null)");
                        //float d = pvm_get_float(o);
                        float d = ((float)(((struct data_area_4_float *)&(o.data->da))->value));
						                is_push(TO_INT(d));
                        ref_dec_o(o);
                    */
                    }
                    break;
            }
        }

        public void ProcessPrefixDouble(PvmRoot root, PvmThread thread, OpCode instruction)
        {
            PvmRoot _root = root;

            switch (instruction)
            {
                default:
                    {/*
                    prefix_double = 1;  // attempt nonprefix impl of inctruction, maybe it checks for a modifier
                    goto noprefix;
                    */
                    }
                    break;

                case OpCode.Ishl: // Not defined for double, throw exception
                case OpCode.Ishr:
                case OpCode.Ushr:
                case OpCode.ior:
                case OpCode.iand:
                case OpCode.ixor:
                case OpCode.inot:
                case OpCode.log_or:
                case OpCode.log_and:
                case OpCode.log_xor:
                case OpCode.log_not:
                    //pvm_exec_panic("invalid double op");
                    break;


                case OpCode.Isum:
                    LISTI("d-isum");
                    {
                        //DOUBLE_STACK_OP(+);
                        Int64 a1 = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 a2 = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = (Int32)a1 + (Int32)a2;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                case OpCode.Imul:
                    LISTI("d-imul");
                    {
                        //DOUBLE_STACK_OP(*);
                        Int64 a1 = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 a2 = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = (Int32)a1 * (Int32)a2;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                case OpCode.Isubul:
                    LISTI("d-isubul");
                    {
                        //int64_t u = ls_pop();
                        //int64_t l = ls_pop();
                        //double r = AS_DOUBLE(u, l, -);
                        //ls_push(TO_LONG(r));
                        Int64 u = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 l = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = (Int32)u - (Int32)l;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                case OpCode.Isublu:
                    LISTI("d-isublu");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        double r = AS_DOUBLE(l, u, -);
                        ls_push(TO_LONG(r));
                    */
                        Int64 u = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 l = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = (Int32)l - (Int32)u;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                case OpCode.Idivul:
                    LISTI("d-idivul");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        double r = AS_DOUBLE(u, l, / );
                        ls_push(TO_LONG(r));
                    */
                        Int64 u = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 l = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = (Int32)u / (Int32)l;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                case OpCode.Idivlu:
                    LISTI("d-idivlu");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        double r = AS_DOUBLE(l, u, / );
                        ls_push(TO_LONG(r));
                    */
                        Int64 u = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 l = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = (Int32)l / (Int32)u;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                // NB! Returns int!
                case OpCode.ige:
                    LISTI("d-ige");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        int r = AS_DOUBLE(l, u, >= );
                        is_push(r);
                    */
                        Int64 u = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 l = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = 0;
                        if ((Int32)u >= (Int32)l) r = 1;
                        if ((Int32)u <= (Int32)l) r = -1;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                case OpCode.ile:
                    LISTI("d-ile");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        int r = AS_DOUBLE(l, u, <= );
                        is_push(r);
                    */
                        Int64 u = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 l = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = 0;
                        if ((Int32)u <= (Int32)l) r = 1;
                        if ((Int32)u >= (Int32)l) r = -1;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                case OpCode.igt:
                    LISTI("d-igt");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        int r = AS_DOUBLE(l, u, >);
                        is_push(r);
                    */
                        Int64 u = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 l = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = 0;
                        if ((Int32)u > (Int32)l) r = 1;
                        if ((Int32)u < (Int32)l) r = -1;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;

                case OpCode.ilt:
                    LISTI("d-ilt");
                    {/*
                        int64_t u = ls_pop();
                        int64_t l = ls_pop();
                        int r = AS_DOUBLE(l, u, <);
                        is_push(r);
                    */
                        Int64 u = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int64 l = thread.callFrame.iStack.Pop(); // ls_pop();
                        Int32 r = 0;
                        if ((Int32)u < (Int32)l) r = 1;
                        if ((Int32)u > (Int32)l) r = -1;
                        thread.callFrame.iStack.Push(r); // ls_push((Int64)r);
                    }
                    break;


                case OpCode.FromD:
                    LISTI("d-fromd (nop)");
                    break;

                case OpCode.FromI:
                    LISTI("d-fromi");
                    {/*
                        double i = is_pop();
                        ls_push(TO_LONG(i));
                    */
                    }
                    break;

                case OpCode.FromL:
                    LISTI("d-froml");
                    {/*
                        double l = ls_pop();
                        ls_push(TO_LONG(l));
                    */
                    }
                    break;

                case OpCode.FromF:
                    LISTI("d-fromf");
                    {/*
                        int i = is_pop();
                        double f = TO_FLOAT(i);
                        ls_push(TO_LONG(f));
                    */
                    }
                    break;

                case OpCode.i2o: // ERROR IMPLEMENT ME
                    LISTI("d-i2o");
                    {/*
                        //pvm_exec_panic("unimpl double i2o");
                        int64_t d = ls_pop();
                        os_push(pvm_create_double_object(TO_DOUBLE(d)));
                    */
                    }
                    break;

                case OpCode.o2i: // ERROR IMPLEMENT ME
                    LISTI("d-o2i");
                    //pvm_exec_panic("unimpl double o2i");
                    {/*

                        struct pvm_object o = os_pop();
                        //if (o.data == 0) pvm_exec_panic("d-o2i(null)");
                        //double d = pvm_get_double(o);
                        double d = ((double)(((struct data_area_4_double *)&(o.data->da))->value));
						                ls_push(TO_LONG(d));
                        ref_dec_o(o);
                    */
                    }
                    break;
            }
        }

        public void ProcessNoPrefix(PvmRoot root, PvmThread thread, OpCode instruction)
        {
            PvmRoot _root = root;

            switch (instruction)
            {

                // type switch prefixes --------------------------------

                // NB! Not break, continue, or else code after the main switch will 
                // reset prefix and print warning

                case OpCode.PrefixLong:
                    {
                        prefix_long = true;
                        //continue; ?
                    }
                    break;

                case OpCode.PrefixFloat:
                    {
                        prefix_float = true;
                        //continue; ?
                    }
                    break;

                case OpCode.PrefixDouble:
                    {
                        prefix_double = true;
                        //continue; ?
                    }
                    break;

                // special opcodes -------------------------------------

                // No operation. This operation does completely nothing. 
                case OpCode.Nop:
                    Debug.WriteLine("nop");
                    break;

                // Prints debug. Special parameters.
                case OpCode.Debug:
                    OpCodeDebug(thread);
                    break;

                // sync ops ---------------------------------------

                case OpCode.GeneralLock:
                    LISTI("lock");
                    {/*
                        // This is java monitor, arbitrary object
                        struct pvm_object lock_obj = os_pop();
                        // TODO impl me
                        ref_dec_o(lock_obj);
                    */
                    }
                    break;

                case OpCode.GeneralUnlock:
                    LISTI("unlock");
                    {/*
					    // This is java monitor, arbitrary object
					    struct pvm_object lock_obj = os_pop();
                        // TODO impl me
                        ref_dec_o(lock_obj);
                    */
                    }
                    break;

                // int stack ops ---------------------------------------

                case OpCode.IsDup:
                    LISTI("is dup");
                    {/*
					    if (DO_TWICE)
					    {
						    int i1 = is_pop();
                            int i2 = is_pop();
                            is_push(i1); is_push(i2);
                            is_push(i1); is_push(i2);
					    }
					    else
						    is_push(is_top());

                    */
                    }
                    break;

                case OpCode.IsDrop:
                    LISTI("is drop");
                    {/*
                        is_pop(); if (DO_TWICE) is_pop();
                    */
                    }
                    break;

                case OpCode.Iconst0:
                    OpCodeIConst0(thread, prefix_long || prefix_double);
                    break;
                case OpCode.Iconst1:
                    OpCodeIConst1(thread, prefix_long || prefix_double);
                    break;
                case OpCode.Iconst8bit:
                    OpCodeIConst8Bit(thread, prefix_long || prefix_double);
                    break;
                case OpCode.Iconst32bit:
                    OpCodeIConst32Bit(thread, prefix_long || prefix_double);
                    break;
                case OpCode.Iconst64bit:
                    OpCodeIConst64Bit(thread);
                    break;
                case OpCode.ConstPool:
                    OpCodeConstPool(thread);
                    break;

                case OpCode.Cast:
                    {/*
				        pvm_object_t target_class = os_pop();
                        pvm_object_t o = os_pop();

                        // TODO cast here!

                        os_push(o);
                        LISTIA("cast %s", "unimplemented!");
			        */
                    }
                    break;

                case OpCode.Ishl:
                    LISTI("ishl");
                    {/*
					    int val = is_pop();
                        is_push(val << is_pop());
				    */
                    }
                    break;

                case OpCode.Ishr:
                    LISTI("ishr");
                    {/*
					    int val = is_pop();
                        is_push(val >> is_pop());
				    */
                    }
                    break;

                case OpCode.Ushr:
                    LISTI("ushr");
                    {/*
					    unsigned val = is_pop();
                        is_push(val >> is_pop());
				    */
                    }
                    break;


                case OpCode.Isum:
                    LISTI("isum");
                    {/*
					    int add = is_pop();
                        //is_top() += add;
                        is_push(is_pop() + add);
				    */
                    }
                    break;

                case OpCode.Imul:
                    LISTI("imul");
                    {/*
					    int mul = is_pop();
                        //is_top() *= mul;
                        is_push(is_pop() * mul);
				    */
                    }
                    break;

                case OpCode.Isubul:
                    LISTI("isubul");
                    {/*
					    int u = is_pop();
                        int l = is_pop();
                        is_push(u - l);
				    */
                    }
                    break;

                case OpCode.Isublu:
                    {
                        /*
					    int u = is_pop();
                        int l = is_pop();
                        is_push(l - u);
				        */
                        var ip = thread.ip;
                        Debug.Write(string.Format("isublu @ {0:d}", ip - 1));
                        Int32 u = (Int32)thread.callFrame.iStack.Pop();
                        Int32 l = (Int32)thread.callFrame.iStack.Pop();
                        Int32 r = l - u;
                        thread.callFrame.iStack.Push(r);
                        Debug.WriteLine(" // pop u={0:d}, pop l={1:d}, push l-u={2:d}", u, l, r);
                    }
                    break;

                case OpCode.Idivul:
                    LISTI("idivul");
                    {/*
					    int u = is_pop();
                        int l = is_pop();
                        is_push(u / l);
				    */
                    }
                    break;

                case OpCode.Idivlu:
                    LISTI("idivlu");
                    {/*
					    int u = is_pop();
                        int l = is_pop();
                        is_push(l / u);
				    */
                    }
                    break;

                case OpCode.ior:
                    LISTI("ior");
                    {/* int operand = is_pop(); is_push(is_pop() | operand); */}
                    break;

                case OpCode.iand:
                    LISTI("iand");
                    {/* int operand = is_pop(); is_push(is_pop() & operand); */}
                    break;

                case OpCode.ixor:
                    LISTI("ixor");
                    {/* int operand = is_pop(); is_push(is_pop() ^ operand); */}
                    break;

                case OpCode.inot:
                    LISTI("inot");
                    {/* int operand = is_pop(); is_push(~operand); */}
                    break;

                case OpCode.log_or:
                    LISTI("lor");
                    {/*
					    int o1 = is_pop();
                        int o2 = is_pop();
                        is_push(o1 || o2);
				    */
                    }
                    break;

                case OpCode.log_and:
                    LISTI("land");
                    {/*
					    int o1 = is_pop();
                        int o2 = is_pop();
                        is_push(o1 && o2);
				    */
                    }
                    break;

                case OpCode.log_xor:
                    LISTI("lxor");
                    {/*
					    int o1 = is_pop() ? 1 : 0;
                        int o2 = is_pop() ? 1 : 0;
                        is_push(o1 ^ o2);
				    */
                    }
                    break;

                case OpCode.log_not:
                    LISTI("lnot");
                    {/*
					    int operand = is_pop();
                        is_push(!operand);
				    */
                    }
                    break;

                case OpCode.ige:    // >=
                    LISTI("ige");
                    {/* int operand = is_pop(); is_push(is_pop() >= operand); */}
                    break;
                case OpCode.ile:    // <=
                    LISTI("ile");
                    {/* int operand = is_pop(); is_push(is_pop() <= operand); */}
                    break;
                case OpCode.igt:    // >
                    LISTI("igt");
                    {/* int operand = is_pop(); is_push(is_pop() > operand); */}
                    break;
                case OpCode.ilt:    // <
                    LISTI("ilt");
                    {/* int operand = is_pop(); is_push(is_pop() < operand); */}
                    break;

                case OpCode.FromI:
                    LISTI("i-fromi (nop)");
                    break;

                case OpCode.FromL:
                    LISTI("i-froml");
                    {/*
					    is_push((int) ls_pop());
				    */
                    }
                    break;

                case OpCode.FromD:
                    LISTI("i-fromd");
                    {/*
					    long l = ls_pop();
                        double d = TO_DOUBLE(l);
                        is_push((int) d);
				    */
                    }
                    break;

                case OpCode.FromF:
                    LISTI("i-fromf");
                    {/*
					    int i = is_pop();
                        float f = TO_FLOAT(i);
                        is_push((int) f);
				    */
                    }
                    break;

                case OpCode.i2o:
                    LISTI("i2o");
                    {/*
                        os_push(pvm_create_int_object(is_pop()));
                    */
                        var ip = thread.ip;
                        Debug.WriteLine("i2o @ {0:d}", ip - 1);
                        /*
                        os_push(pvm_get_null_object()); // so what OpCode.os_push_null is for then?
                        */
                        var i = thread.callFrame.iStack.Pop();
                        thread.callFrame.oStack.Push(_root.NewInt((int)i));
                    }
                    break;

                case OpCode.o2i:
                    LISTI("o2i");
                    {/*
					    struct pvm_object o = os_pop();
                        //if (o.data == 0) pvm_exec_panic("o2i(null)");
                        is_push(pvm_get_int(o));
                        ref_dec_o(o);
				    */
                    }
                    break;


                case OpCode.OsEq:
                    LISTI("os eq");
                    {/*
					    struct pvm_object o1 = os_pop();
                        struct pvm_object o2 = os_pop();
                        is_push(o1.data == o2.data);
                        ref_dec_o(o1);
                        ref_dec_o(o2);
				    */
                    }
                    break;

                case OpCode.OsNeq:
                    LISTI("os neq");
                    {/*
					    struct pvm_object o1 = os_pop();
                        struct pvm_object o2 = os_pop();
                        is_push(o1.data != o2.data);
                        ref_dec_o(o1);
                        ref_dec_o(o2);
				    */
                    }
                    break;

                case OpCode.OsIsnull:
                    LISTI("isnull");
                    {/*
					    struct pvm_object o1 = os_pop();
                        is_push(pvm_is_null(o1));
                        ref_dec_o(o1);
				    */
                    }
                    break;
                /*
				case OpCode.os_push_null:
				    LISTI("push null");
				    {
				        os_push( pvm_get_null_object() );
                    }
				    break;
				*/

                // summoning, special ----------------------------------------------------

                // Put null object on stack. 
                case OpCode.SummonNull:
                    OpCodeSummonNull(thread);
                    break;
                // Puts current thread object reference on stack.
                case OpCode.SummonThread:
                    OpCodeSummonThread(thread);
                    break;
                // Puts this object reference on stack.
                case OpCode.SummonThis:
                    OpCodeSummonThis(thread);
                    break;

                case OpCode.SummonClassClass:
                    LISTI("summon class class");
                    {/*
                        // it has locked refcount
                        os_push(pvm_get_class_class());
                    */
                    }
                    break;

                case OpCode.SummonInterfaceClass:
                    LISTI("summon interface class");
                    {/*
                        // locked refcnt
                        os_push(pvm_get_interface_class());
                    */
                    }
                    break;

                case OpCode.SummonCodeClass:
                    LISTI("summon code class");
                    {/*
                        // locked refcnt
                        os_push(pvm_get_code_class());
                    */
                    }
                    break;

                case OpCode.SummonIntClass:
                    LISTI("summon int class");
                    {/*
                        // locked refcnt
                        os_push(pvm_get_int_class());
                    */
                    }
                    break;

                case OpCode.SummonStringClass:
                    OpCodeSummonStringClass(thread);
                    break;

                case OpCode.SummonArrayClass:
                    LISTI("summon array class");
                    {/*
                        // locked refcnt
                        os_push(pvm_get_array_class());
                    */
                    }
                    break;

                case OpCode.SummonByName:
                    {/*
				        LISTI("summon by name");
                        struct pvm_object name = pvm_code_get_string(&(da->code));
				        struct pvm_object cl = pvm_exec_lookup_class_by_name(name);
                        ref_dec_o(name);
				        // TODO: Need throw here?
				        if (pvm_is_null(cl)) {/*
					        //pvm_exec_panic("summon by name: null class");
					        //printf("summon by name: null class");
					        //pvm_exec_do_throw(da);
					        break;
				        }
				        os_push(cl);  // cl popped from stack - don't increment
                    */
                    }
                    break;

                /**
                * TODO A BIG NOTE for object creation
                *
                * We must be SURE that it is NOT ever possible to pass
                * non-internal object as init data to internal and vice versa!
                * It would be a securily hole!
                *
                **/


                case OpCode.New:
                    LISTI("new");
                    {/*
					    pvm_object_t cl = os_pop();
                        os_push(pvm_create_object(cl));
					    //ref_dec_o( cl );  // object keep class ref
				    */
                    }
                    break;

                case OpCode.Copy:
                    LISTI("copy");
                    {/*
					    pvm_object_t o = os_pop();
                        os_push(pvm_copy_object(o));
                        ref_dec_o(o);
				    */
                    }
                    break;

                // if you want to enable these, work out refcount
                // and security issues first!
                // compose/decompose
#if false
			    case OpCode.os_compose32:
				    LISTI(" compose32");
				    {/*
					    int num = pvm_code_get_int32(&(da->code));
					    struct pvm_object in_class = os_pop();
					    os_push(pvm_exec_compose_object(in_class, da->_ostack, num));
				    */}
				    break;

			    case OpCode.os_decompose:
				    LISTI(" decompose");
				    {/*
					    struct pvm_object to_decomp = os_pop();
					    int num = da_po_limit(to_decomp.data);
					    is_push(num);
					    while (num)
					    {/*
						    num--;
						    struct pvm_object o = pvm_get_ofield(to_decomp, num);
						    os_push(ref_inc_o(o));
					    */}
					    os_push(to_decomp.data->_class);
				    */}
				    break;
#endif
                // string ----------------------------------------------------------------

                case OpCode.SconstBin:
                    LISTI("sconst bin");
                    {
                        /* 
                        os_push(pvm_code_get_string(&(da->code))); 
                        */
                        var bin = thread.code.GetString(thread.ip);
                        thread.ip += 4 + (uint)bin.Length;
                        var o = _root.NewString(bin);
                        thread.callFrame.oStack.Push(o);
                    }
                    break;


                // flow ------------------------------------------------------------------

                case OpCode.Jmp: OpCodeJmp(thread); break;
                case OpCode.Djnz: OpCodeDjnz(thread); break;
                case OpCode.Jz: OpCodeJz(thread); break;
                case OpCode.Switch: OpCodeSwitch(thread); break;

                case OpCode.Ret: OpCodeRet(thread); break;

                // exceptions are like ret ---------------------------------------------------

                // Moves ip to catcher location.
                case OpCode.Throw: OpCodeThrow(thread); break;
                // Places chatcher location on exception stack.
                case OpCode.PushCatcher: OpCodePushCatcher(thread); break;
                // Removews chatcher.
                case OpCode.PopCatcher: OpCodePopCatcher(thread); break;

                // ok, now method calls ------------------------------------------------------

                case OpCode.DynamicInvoke: OpCodeDynamicInvoke(thread); break;

                // object stack --------------------------------------------------------------

                case OpCode.OsDup: OpCodeOsDup(thread); break;
                case OpCode.OsDrop: OpCodeOsDrop(thread); break;
                case OpCode.OsPull32: OpCodeOsPull32(thread); break;
                case OpCode.OsLoad8: OpCodeOsLoad8(thread); break;
                case OpCode.OsLoad32: OpCodeOsLoad32(thread); break;
                case OpCode.OsSave8: OpCodeOsSave8(thread); break;
                case OpCode.OsSave32: OpCodeOsSave32(thread); break;
                case OpCode.IsLoad8: OpCodeIsLoad8(thread); break;
                case OpCode.IsSave8: OpCodeIsSave8(thread); break;
                // Gets object from stack at position.
                case OpCode.OsGet32: OpCodeOsGet32(thread); break;
                case OpCode.OsSet32: OpCodeOsSet32(thread); break;
                case OpCode.IsGet32: OpCodeIsGet32(thread); break;
                case OpCode.IsSet32: OpCodeIsSet32(thread); break;

                case OpCode.Sys0: OpCodeSysX(thread, 0); break;
                case OpCode.Sys1: OpCodeSysX(thread, 1); break;
                case OpCode.Sys2: OpCodeSysX(thread, 2); break;
                case OpCode.Sys3: OpCodeSysX(thread, 3); break;
                case OpCode.Sys4: OpCodeSysX(thread, 4); break;
                case OpCode.Sys5: OpCodeSysX(thread, 5); break;
                case OpCode.Sys6: OpCodeSysX(thread, 6); break;
                case OpCode.Sys7: OpCodeSysX(thread, 7); break;
                case OpCode.Sys8: OpCodeSysX(thread, 8); break;
                case OpCode.Sys9: OpCodeSysX(thread, 9); break;
                case OpCode.SysA: OpCodeSysX(thread, 10); break;
                case OpCode.SysB: OpCodeSysX(thread, 11); break;
                case OpCode.SysC: OpCodeSysX(thread, 12); break;
                case OpCode.SysD: OpCodeSysX(thread, 13); break;
                case OpCode.SysE: OpCodeSysX(thread, 14); break;
                case OpCode.SysF: OpCodeSysX(thread, 15); break;
                case OpCode.Sys8Bit: OpCodeSys8Bit(thread); break;

                // these 4 are parameter-less calls!
                case OpCode.ShortCall0: OpCodeCallXX(thread, 0); break;
                case OpCode.ShortCall1: OpCodeCallXX(thread, 1); break;
                case OpCode.ShortCall2: OpCodeCallXX(thread, 2); break;
                // pvm_exec_call(da, 3, 0, 1, pvm_get_null_object());
                case OpCode.ShortCall3: OpCodeCallXX(thread, 3); break;
                case OpCode.Call00: OpCodeCallXX(thread, 0); break;
                case OpCode.Call01: OpCodeCallXX(thread, 1); break;
                case OpCode.Call02: OpCodeCallXX(thread, 2); break;
                case OpCode.Call03: OpCodeCallXX(thread, 3); break;
                case OpCode.Call04: OpCodeCallXX(thread, 4); break;
                case OpCode.Call05: OpCodeCallXX(thread, 5); break;
                case OpCode.Call06: OpCodeCallXX(thread, 6); break;
                case OpCode.Call07: OpCodeCallXX(thread, 7); break;
                case OpCode.Call08: OpCodeCallXX(thread, 8); break;
                case OpCode.Call09: OpCodeCallXX(thread, 9); break;
                case OpCode.Call0A: OpCodeCallXX(thread, 10); break;
                case OpCode.Call0B: OpCodeCallXX(thread, 11); break;
                case OpCode.Call0C: OpCodeCallXX(thread, 12); break;
                case OpCode.Call0D: OpCodeCallXX(thread, 13); break;
                case OpCode.Call0E: OpCodeCallXX(thread, 14); break;
                case OpCode.Call0F: OpCodeCallXX(thread, 15); break;
                case OpCode.Call10: OpCodeCallXX(thread, 16); break;
                case OpCode.Call11: OpCodeCallXX(thread, 17); break;
                case OpCode.Call12: OpCodeCallXX(thread, 18); break;
                case OpCode.Call13: OpCodeCallXX(thread, 19); break;
                case OpCode.Call14: OpCodeCallXX(thread, 20); break;
                case OpCode.Call15: OpCodeCallXX(thread, 21); break;
                case OpCode.Call16: OpCodeCallXX(thread, 22); break;
                case OpCode.Call17: OpCodeCallXX(thread, 23); break;
                case OpCode.Call18: OpCodeCallXX(thread, 24); break;
                case OpCode.Call19: OpCodeCallXX(thread, 25); break;
                case OpCode.Call1A: OpCodeCallXX(thread, 26); break;
                case OpCode.Call1B: OpCodeCallXX(thread, 27); break;
                case OpCode.Call1C: OpCodeCallXX(thread, 28); break;
                case OpCode.Call1D: OpCodeCallXX(thread, 29); break;
                case OpCode.Call1E: OpCodeCallXX(thread, 30); break;
                case OpCode.Call1F: OpCodeCallXX(thread, 31); break;
                case OpCode.Call8Bit: OpCodeCall8Bit(thread); break;
                case OpCode.Call32Bit: OpCodeCall32Bit(thread); break;

                default:
                    {
                        Debug.WriteLine("Unknown op code {0:X}", instruction);
                        throw new InvalidOperationException("thread exec: unknown opcode");
                    }
            } // outer switch(instruction)
        }

        // OpCodes documentation
        // https://github.com/dzavalishin/phantomuserland/wiki/ByteCode

        // debug 
        //   debug; // just print stacks
        //   debug "we're busted"; // show message too
        //   debug 33; // no meaning yet
        //   debug 33 "we're busted"; // together

        /// <summary>
        /// Prints string argument(if any), and top values of integer 
        /// and object stack. Integer argument (mode) is one byte and 
        /// upper bit is used internally, so don’t pass anything 
        /// grater than 0x7F or lower than 0 – will be stripped.
        /// Bit 0 (& 0x01) of mode byte turns on debug instruction printing.
        /// Bit 1 (& 0x02) turns it off.
        /// </summary>
        /// <param name="thread"></param>
        private void OpCodeDebug(PvmThread thread)
        {
            /*
            int type = pvm_code_get_byte(&(da->code)); //cf->cs.get_instr( cf->IP );
            printf("\n\nDebug 0x%02X", type);
            if (type & 0x80)
            {
                printf(" (");
                //cf->cs.get_string( cf->IP ).my_data()->print();
                //get_string().my_data()->print();
                pvm_object_t o = pvm_code_get_string(&(da->code));
                pvm_object_print(o);
                ref_dec_o(o);
                printf(")");
            }

            if (type & 0x01) debug_print_instr = 1;
            if (type & 0x02) debug_print_instr = 0;

            //if( cf->istack.empty() )printf(", istack empty");
            if (is_empty()) printf(", istack empty");
            else printf(",\n\tistack top = %d", is_top());
            if (os_empty()) printf(", ostack empty");
            else
            {
                printf(",\n\tostack top = {/*");
                //pvm_object_print(os_top());
                printf("}");
            }
            printf(";\n\n");
        */
        }

        // const opcode
        //    const “hi there”;  // top of object stack is string object now
        //    const 32656; // top of integer stack has 32656 now
        // Push constant on stack top.S
        // pecial shortcuts for 0 and 1 exist.
        // Strings are Unicode UTF-8. MUST BE PLAIN 2-BYTE UNICODE?

        /// <summary>
        /// puts 0 on top of integer stack.
        /// In case of long modificatior puts two integers on stack.
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="isLong"></param>
        private void OpCodeIConst0(PvmThread thread, bool isLong)
        {
            /*
            is_push(0); if (DO_TWICE) is_push(0);
            */
            var ip = thread.ip;
            if (isLong)
                thread.callFrame.iStack.Push(1);
            else
                thread.callFrame.iStack.Push(1);
            Debug.WriteLine("iconst 0 @ {1:d}", ip);
        }
        /// <summary>
        /// puts 1 on top of integer stack.
        /// In case of long modificatior puts two integers on stack.
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="isLong"></param>
        private void OpCodeIConst1(PvmThread thread, bool isLong)
        {
            /*
            is_push(1); if (DO_TWICE) is_push(1);
            */
            var ip = thread.ip;
            if (isLong)
                thread.callFrame.iStack.Push(1);
            else
                thread.callFrame.iStack.Push(1);
            Debug.WriteLine("iconst 1 @ {1:d}", ip);
        }
        /// <summary>
        /// Gets from code one bytes and puts it on top of integer stack.
        /// In case of long modificatior puts two integers on stack.
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="isLong"></param>
        private void OpCodeIConst8Bit(PvmThread thread, bool isLong)
        {
            /*
			int v = pvm_code_get_byte(&(da->code));
			if (DO_TWICE) ls_push(v);
			else is_push(v);
            LISTIA("iconst8 = %d", v);
			*/
            var ip = thread.ip;
            Byte v = thread.CodeGetByte();
            if (isLong)
                thread.callFrame.iStack.Push((Int64)v);
            else
                thread.callFrame.iStack.Push(v);
            Debug.WriteLine("iconst8 {0:d} @ {1:d}", v, ip);
        }
        /// <summary>
        /// Gets from code 4 bytes and puts them on top of integer stack.
        /// In case of long modificatior puts two integers on stack.
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="isLong"></param>
        private void OpCodeIConst32Bit(PvmThread thread, bool isLong)
        {
            /*
			int v = pvm_code_get_int32(&(da->code));
			if (DO_TWICE) ls_push(v);
			else         is_push(v);
            LISTIA("iconst32 = %d", v);
			*/
            var ip = thread.ip;
            Int32 v = thread.CodeGetInt32();
            if (isLong)
                thread.callFrame.iStack.Push((Int64)v);
            else
                thread.callFrame.iStack.Push(v);
            Debug.WriteLine("iconst32 {0:d} @ {1:d}", v, ip - 1);
        }
        /// <summary>
        /// Gets from code 8 bytes and puts them on top of integer stack.
        /// Actually puts it as two integers on stack.
        /// </summary>
        /// <param name="thread"></param>
        private static void OpCodeIConst64Bit(PvmThread thread)
        {
            /*
            int64_t v = pvm_code_get_int64(&(da->code));
            ls_push(v);
            LISTIA("iconst64 = %Ld", v);
            */
            var ip = thread.ip;
            Int64 v = thread.CodeGetInt64();
            thread.callFrame.iStack.Push((Int64)v);
            Debug.WriteLine("iconst64 {0:d} @ {1:d}", v, ip);
        }
        // const_pool 
        //   const_pool 22;
        /// <summary>
        /// Load constand numbed 22 from constants pool.
        /// Constants pool is loaded from class file. 
        /// </summary>
        /// <param name="thread"></param>
        private static void OpCodeConstPool(PvmThread thread)
        {
            /*
			pvm_object_t oc = pvm_get_class(this_object());
            //struct data_area_4_class *cda = pvm_object_da(oc, class);
            struct data_area_4_class * cda = ((struct data_area_4_class *)&(oc.data->da));

			int32_t id = pvm_code_get_int32(&(da->code));
            pvm_object_t co = pvm_get_ofield(cda->const_pool, id);
            os_push(co);
            LISTIA("const_pool id %d", id);
			*/
            throw new NotImplementedException("ConstPool");
        }

        private void OpCodeJmp(PvmThread thread)

        {
            //BA LISTIA("jmp %d", da->code.IP);
            /* 
            da->code.IP = pvm_code_get_rel_IP_as_abs(&(da->code));
            */
        }

        private void OpCodeDjnz(PvmThread thread)
        {
            /*
			int new_IP = pvm_code_get_rel_IP_as_abs(&(da->code));
            //is_top()--;
            is_push(is_pop() - 1);
			if (is_top()) da->code.IP = new_IP;

			LISTIA("djnz (%d)", is_top());
            LISTIA("djnz -> %d", new_IP);
		    */
        }

        // jz 
        //   repeat:
        //   // calculate while condition
        //   jz done;
        //   // loop body
        //   jmp repeat;
        //   done:

        /// <summary>
        /// While-style loop operator. Jump if top of int stack is zero.
        /// NB! This operation does pop integer stack! 
        /// </summary>
        /// <param name="thread">current thread object</param>
        private static void OpCodeJz(PvmThread thread)
        {
            /*
			int new_IP = pvm_code_get_rel_IP_as_abs(&(da->code));
            int test = is_pop();
			if (!test) da->code.IP = new_IP;

			LISTIA("jz (%d)", test);
            LISTIA("jz -> %d", new_IP);
			*/
            var ip = thread.ip;
            Int32 jump = thread.CodeGetInt32();
            Debug.Write(string.Format("jz {0:d} @ {1:d}", jump, ip - 1));
            // (int) to make it signed to get bidirectional displacement
            var test = thread.callFrame.iStack.Pop();
            if (test == 0)
                thread.ip = (uint)((int)ip + jump);
            Debug.WriteLine(" // pop test={0:d} == 0 -> (@{1:d})", test, thread.ip);
        }

        private void OpCodeSwitch(PvmThread thread)
        {
            /*
			unsigned int tabsize = pvm_code_get_int32(&(da->code));
            int shift = pvm_code_get_int32(&(da->code));
            unsigned int divisor = pvm_code_get_int32(&(da->code));
            int stack_top = is_pop();

            //LISTIA("switch (%d+%d)/%d, ", stack_top, shift, divisor );
            LISTIA("switch (%d)", stack_top);


            unsigned int start_table_IP = da->code.IP;
            unsigned int displ = (stack_top + shift) / divisor;
            unsigned int new_IP = start_table_IP + (tabsize * 4); // default

			//LISTIA("displ %d, etab addr %d ", displ, new_IP );


			if (displ<tabsize)
			{
				da->code.IP = start_table_IP + (displ* 4); // TODO BUG! 4!
				LISTIA("load from %d, ", da->code.IP);
                new_IP = pvm_code_get_rel_IP_as_abs(&(da->code));
			}
			da->code.IP = new_IP;

			//LISTIA("switch(%d) ->%d", displ, new_IP );
			LISTIA("switch ->%d", new_IP);
            */
        }

        // ret
        //   ret;
        /// <summary>
        /// Top of object stack is popped and pushed on caller’s stack.
        /// If stack is empty null object is pushed to caller.
        /// All the exception catchers pushed in this call are discarded.
        /// </summary>
        /// <param name="thread">current thread object</param>
        private void OpCodeRet(PvmThread thread)
        {
            /*
			if (DEB_CALLRET || debug_print_instr) printf("\nret     (stack_depth %d -> ", da->stack_depth);
            //struct pvm_object ret = pvm_object_da(da->call_frame, call_frame)->prev;
            struct pvm_object ret = ((struct data_area_4_call_frame *)&(da->call_frame.data->da))->prev;

			if (pvm_is_null(ret))
			{
				if (DEB_CALLRET || debug_print_instr) printf("exit thread)\n");
				return;  // exit thread
			}
			pvm_exec_do_return(da);
			if (DEB_CALLRET || debug_print_instr) printf("%d)", da->stack_depth);
            */
            var ip = thread.ip;
            var ret = thread.callFrame.prev;
            Debug.Write(string.Format("ret @ {0:d}", ip - 1));
            if (ret == null)
                throw new Exception("exit thread");
            var returnVal = _root.NewNull();
            //_root.RefDec(thread.callFrame);
            thread.callFrame = ret;
            //thread.PvmExecLoadFastAcc();
            thread.callFrame.oStack.Push(returnVal);
        }
        // push catcher label 
        //   summon “internal.class.string”;
        //   push catcher string_thrown;
        //   // code
        //   ret;
        //   string_thrown:
        //   // on exception of string type we’ll get here
        //   // do cleanup
        //   ret;
        /// <summary>
        /// Top of stack must contain class object. 
        /// Exceptions of that class will be catched here and control 
        /// will be passed to a label if such exception is thrown.
        /// Thrown object will be on stack top in this case. In general 
        /// no other assumptions about stack state can be done.
        /// </summary>
        /// <param name="thread">current thread object</param>
        private void OpCodePushCatcher(PvmThread thread)
        {
            /*
			unsigned addr = pvm_code_get_rel_IP_as_abs(&(da->code));
            LISTIA("push catcher %u", addr);
			//cf->push_catcher( addr, os_pop() );
			//call_frame.estack().push(exception_handler(os_pop(),addr));

			/*struct pvm_exception_handler eh;
			eh.object = os_pop();
			eh.jump = addr;

			//ref_inc_o( eh.object );

			es_push(eh); 
            */
            var ip = thread.ip;
            UInt32 addr = thread.CodeGetUInt32();
            Debug.WriteLine("push catcher {0:d} @ {1:d}", addr, ip - 1);
            var eh = new Tuple<PvmObject, uint>(null, addr); //TODO what object ?
            thread.callFrame.eStack.Push(eh);
        }
        // pop catcher
        //   pop catcher;
        /// <summary>
        /// Last pushed catcher will be deleted.Note that all method's 
        /// catchers will be automatically popped on return. 
        /// </summary>
        /// <param name="thread">current thread object</param>
        private void OpCodePopCatcher(PvmThread thread)
        {
            /*
			//cf->pop_catcher();
			//call_frame.estack().pop();
			//ref_dec_o(es_pop().object);
            */
            Debug.WriteLine("pop catcher");
        }
        // throw 
        //   const "we have a problem here";
        //   throw;
        /// <summary>
        /// Object on top of stack is thrown down.If stack is empty - 
        /// will throw special system-wide object (null?), 
        /// if already on top of call stack - will kill 
        /// current thread in a bad way.
        /// </summary>
        /// <param name="thread">current thread object</param>
        private void OpCodeThrow(PvmThread thread)
        {
            /*
			if (DEB_CALLRET || debug_print_instr) printf("\nthrow     
            (stack_depth %d -> ", da->stack_depth);
            pvm_exec_do_throw(da);
			if (DEB_CALLRET || debug_print_instr) printf("%d)", 
            da->stack_depth);
            */
            Debug.WriteLine("throw");
            throw new Exception("exception");
        }

        // summon null 
        //   summon null
        //   // stack top has null object
        /// <summary>
        /// Put null object on stack.
        /// </summary>
        /// <param name="thread">current thread object</param>
        private void OpCodeSummonNull(PvmThread thread)
        {
            var ip = thread.ip;
            Debug.WriteLine("push null @ {0:d}", ip - 1);
            /*
            os_push(pvm_get_null_object()); // so what OpCode.os_push_null is for then?
            */
            thread.callFrame.oStack.Push(_root.NewNull());
        }

        // summon thread  
        //   summon thread 
        //   // current thread object is on stack top now
        /// <summary>
        /// Puts current thread object reference on stack. 
        /// </summary>
        /// <param name="thread">current thread object</param>
        private void OpCodeSummonThread(PvmThread thread)
        {
            Debug.WriteLine("summon thread");
            /*
            os_push(ref_inc_o(current_thread));
			//printf("ERROR: summon thread");
            */
            thread.callFrame.oStack.Push(thread);
            //_root.RefInc(thread);
            Debug.WriteLine("ERROR: summon thread");
        }

        // summon this 
        //   summon this
        //   // this object is on stack top now
        /// <summary>
        /// Puts this object reference on stack. 
        /// </summary>
        /// <param name="thread">current thread object</param>
        private void OpCodeSummonThis(PvmThread thread)
        {
            var ip = thread.ip;
            Debug.WriteLine("summon this @ {0:d}", ip - 1);
            /*
            os_push(ref_inc_o(this_object()));
            */
            thread.callFrame.oStack.Push(thread.callFrame.thisObject);
            //_root.RefInc(thread.callFrame.thisObject);
        }

        /// <summary>
        /// Puts string class on stack. 
        /// </summary>
        /// <param name="thread">current thread object</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OpCodeSummonStringClass(PvmThread thread)
        {
            var ip = thread.ip;
            Debug.WriteLine("summon string class @ {0:d}", ip - 1);
            /*
            // locked refcnt
            os_push(pvm_get_string_class());
            */
            thread.callFrame.oStack.Push(_root.stringClass);
        }

        private void OpCodeDynamicInvoke(PvmThread thread)
        {/*
			dynamic_method_info_t mi;

            mi.method_name = os_pop();
            mi.new_this = os_pop();
            / *
            pvm_object_t np = os_pop();
            if( !IS_PHANTOM_INT(np) )
            {
            printf("dyn call n_param not int: " );
            pvm_object_dump( np );
            mi.n_param = 0;
            }
            else
            mi.n_param = pvm_get_int(np );
            ref_dec_o(np );
            * /
            mi.n_param = is_pop();
#if DEB_DYNCALL
			printf("dyn call %d param, name = '", mi.n_param);
			//pvm_object_dump( mi.method_name );
			pvm_puts(mi.method_name);
			printf("', this class = ");
			//pvm_object_dump( mi.new_this );
			pvm_object_t cn = pvm_get_class_name(mi.new_this);
			pvm_puts(cn);
			ref_dec_o(cn);
			printf("\n");
#endif // DEB_DYNCALL

			if (find_dynamic_method(&mi))
				pvm_exec_panic("dynamic invoke failed");

            pvm_exec_call(da, mi.method_ordinal, mi.n_param, 1, mi.new_this);

        */
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OpCodeOsDup(PvmThread thread)
        {
            LISTI("os dup");
            /*
            //pvm_object_t o = os_top();
            pvm_object_t o = pvm_ostack_top(da->_ostack);
            os_push(ref_inc_o(o));
            */
            var o = thread.callFrame.oStack.GetTop();
            thread.callFrame.oStack.Push(o);
        }

        private void OpCodeOsDrop(PvmThread thread)
        {
            LISTI("os drop");
            /*
            ref_dec_o(os_pop()); 
            */
            var val = thread.callFrame.oStack.Pop();
        }

        private void OpCodeOsPull32(PvmThread thread)
        {
            LISTI("os pull");
            /*
			pvm_object_t o = os_pull(pvm_code_get_int32(&(da->code)));
            os_push(ref_inc_o(o));
		    */
        }

        private void OpCodeOsLoad8(PvmThread thread)
        {
            /* 
            pvm_exec_load(da, pvm_code_get_byte(&(da->code))); 
            */
            Byte i = thread.CodeGetByte();
            Debug.WriteLine("os load {0:d}", i);
            var o = thread.callFrame.thisObject.Fields.GetOrdinal(i);
            //_root.RefInc(o);
            thread.callFrame.oStack.Push(o);
        }

        private void OpCodeOsLoad32(PvmThread thread)
        {
            /*
            pvm_exec_load(da, pvm_code_get_int32(&(da->code)));
            */
            Byte i = thread.CodeGetByte();
            Debug.WriteLine("os load {0:d}", i);
            var o = thread.callFrame.thisObject.Fields.GetOrdinal(i);
            //_root.RefInc(o);
            thread.callFrame.oStack.Push(o);
        }

        private void OpCodeOsSave8(PvmThread thread)
        {
            /*
            pvm_exec_save(da, pvm_code_get_byte(&(da->code)));
            */
            Byte i = thread.CodeGetByte();
            Debug.WriteLine("os save {0:d}", i);
            var o = thread.callFrame.oStack.Pop();
            thread.callFrame.thisObject.Fields.SetOrdinal(i, o);
        }

        private void OpCodeOsSave32(PvmThread thread)
        {
            /*
            pvm_exec_save(da, pvm_code_get_int32(&(da->code)));
            */
        }

        private void OpCodeIsLoad8(PvmThread thread)
        {
            /*
            pvm_exec_iload(da, pvm_code_get_byte(&(da->code)));
            */
        }

        private void OpCodeIsSave8(PvmThread thread)
        {
            /*
            pvm_exec_isave(da, pvm_code_get_byte(&(da->code)));
            */
        }

        private void OpCodeOsGet32(PvmThread thread)
        {
            /*
            pvm_exec_get(da, pvm_code_get_int32(&(da->code)));
            */
            var ip = thread.ip;
            UInt32 pos = thread.CodeGetUInt32();
            Debug.WriteLine("os stack get {1:d} @ {0:d} // push o", ip - 1, pos);
            var o = thread.callFrame.oStack.GetPos(pos);
            //_root.RefInc(o);
            thread.callFrame.oStack.Push(o);
        }

        private void OpCodeOsSet32(PvmThread thread)
        {
            /*
            pvm_exec_set(da, pvm_code_get_int32(&(da->code)));
            */
        }

        private void OpCodeIsGet32(PvmThread thread)
        {
            /*
            pvm_exec_iget(da, pvm_code_get_int32(&(da->code)));
            */
        }

        private void OpCodeIsSet32(PvmThread thread)
        {
            /*
            pvm_exec_iset(da, pvm_code_get_int32(&(da->code)));
            */
        }

        // sys syscall_number 
        //   sys 0;
        // Executes this (and only this!) object’s embedded method.
        // NB – it is not possible to execute sys for other object than 
        // this because it will render its specialness visible from outside.
        // This operation is valid for a very limited number of classes.
        // Nothing can be guaranteed about sys. As for now all of them return 
        // something, but it is not enforced by interpreter. Though, for normal 
        // operation sys must return some object or throw an exception. 
        // It is possible to pass parameters to sys on int stack and receive some 
        // return there as well, which is differs from call, which passes data 
        // on object stack only.That is so because sys does not create a call frame 
        // and thus all the current stack data is available to its code. 
        // Generally, sys is used as the only content of internal class methods. 

        /// <summary>
        /// Executes internal object's embedded (system) method. 
        /// Deal only with this object of current thread.
        /// </summary>
        /// <param name="thread">current thread object</param>
        /// <param name="sysIndex">system call number</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OpCodeSysX(PvmThread thread, uint sysIndex)
        {
            var ip = thread.ip; ;
            Debug.WriteLine("sys {0:d} @ {2:d}", sysIndex, ip);
            thread.callFrame.thisObject.SysCall(
                sysIndex, thread.callFrame.thisObject, thread);
            //sys_sleep:
#if OLD_VM_SLEEP
			// Only sys can put thread asleep
			// If we are snapped here we, possibly, will continue from
			// the entry to this func. So save fast acc and recheck
			// sleep condition on the func entry.
			if (da->sleep_flag)
			{/*
				pvm_exec_save_fast_acc(da); // Before snap
				phantom_thread_sleep_worker(da);
			}
#endif
        }

        /// <summary>
        /// Executes internal object's embedded (system) method. 
        /// Takes method number from stack
        /// Deal only with this object of current thread.
        /// </summary>
        /// <param name="thread">current thread object</param>
        /// <param name="sysIndex">system call number</param>
        private void OpCodeSys8Bit(PvmThread thread)
        {
            var ip = thread.ip; ;
            var sysIndex = thread.CodeGetByte();
            Debug.WriteLine("sys {1:d} @ {0:d}", ip - 1, sysIndex);
            thread.callFrame.thisObject.SysCall(
                sysIndex, thread.callFrame.thisObject, thread);
        }

        // call methodIndex numberOfArgs 
        //   call 0 2;
        // Pops numberOfArgs arguments from object stack, 
        // than pops object to call method for. 

        /// <summary>
        /// Calls selected method passing given number of args.
        /// Top of integer stack of called method will have 
        /// number of arguments passed. After the return exactly 
        /// one object (possibly null) will be on the object stack.
        /// </summary>
        /// <param name="thread">current thread</param>
        /// <param name="callIndex">called method number</param>
        private void OpCodeCallXX(PvmThread thread, uint callIndex)
        {
            var ip = thread.ip;
            var nArgs = thread.CodeGetByte();
            Debug.WriteLine("call {0:d} {1:d} @ {2:d}", callIndex, nArgs, ip - 1);
            //no optimization for soon return
            ExecCall(thread, callIndex, nArgs, false, thread.callFrame.thisObject); // _root.NewNull());
        }

        /// <summary>
        /// Calls method taking index from stack and passing given number of args.
        /// </summary>
        /// <param name="thread">current thread</param>
        private void OpCodeCall8Bit(PvmThread thread)
        {
            /*
			unsigned int method_index = pvm_code_get_byte(&(da->code));
            unsigned int n_param = pvm_code_get_int32(&(da->code));
            pvm_exec_call(da, method_index, n_param, 1, pvm_get_null_object());
			*/
            var ip = thread.ip; ;
            var callIndex = thread.CodeGetByte();
            var nArgs = thread.CodeGetByte();
            Debug.WriteLine("call {0:d} {1:d} @ {2:d}", callIndex, nArgs, ip);
            //no optimization for soon return
            ExecCall(thread, callIndex, nArgs, false, _root.NewNull());
        }

        /// <summary>
        /// Calls method taking index from stack and passing given number of args.
        /// </summary>
        /// <param name="thread">current thread</param>
        private void OpCodeCall32Bit(PvmThread thread)
        {
            /*
			unsigned int method_index = pvm_code_get_int32(&(da->code));
            unsigned int n_param = pvm_code_get_int32(&(da->code));
            pvm_exec_call(da, method_index, n_param, 1, pvm_get_null_object());
		    */
            var ip = thread.ip; ;
            var callIndex = thread.CodeGetInt32();
            var nArgs = thread.CodeGetByte();
            Debug.WriteLine("call {0:d} {1:d} @ {2:d}", callIndex, nArgs, ip);
            //no optimization for soon return
            ExecCall(thread, (uint)callIndex, nArgs, false, _root.NewNull());
        }

        private void ExecCall(PvmThread thread, uint callIndex, uint nParams,
            bool isOptimize, PvmObject thisObject)
        {
            Debug.WriteLine(" exec call {0:d} (stack_depth {1:d} -> ", callIndex, 1);
            //thread.PvmExecSaveFastAcc();

            // Find start method
            var code = thisObject.Iface.GetMethod((int)callIndex);

            // Create call frame
            var newCf = _root.NewCallFrame(thisObject);
            newCf.ip = 0;
            newCf.ipMax = (uint)code.Code.GetLength(0); // code_size
            newCf.code = code;
            newCf.thisObject = thisObject;
            //_root.RefInc(thisObject);

            // Prepare argiments
            for (uint i = nParams; i > 0; i--)
            {
                var o = thread.callFrame.oStack.Pop();
                newCf.oStack.Push(o);
            }
            newCf.iStack.Push(nParams);

            newCf.prev = thread.callFrame; // For ret able to return previous call frame.
            thread.callFrame = newCf; // Swith to new call frame
            //thread.PvmExecLoadFastAcc();
        }

        private static void LISTI(string str)
        {
            Debug.WriteLine(str);
        }
    }
}
