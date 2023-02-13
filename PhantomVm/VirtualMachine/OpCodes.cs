
namespace Cc.Anba.PhantomOs.VirtualMachine
{
    public enum OpCode
    {
        ///<summary>No operation</summary>
        Nop = 0x00,
        ///<summary>Special debug command</summary>
        Debug = 0x01,
        //id(skipz,= 0x02)  // not impl - and will not be...
        //id(skipnz,= 0x03) // not impl - and will not be...
        ///<summary>Jum if not zero</summary>
        Djnz = 0x04,
        ///<summary></summary>
        Jz = 0x05,
        ///<summary></summary>
        Jmp = 0x06,
        ///<summary></summary>
        Switch = 0x07,
        ///<summary></summary>
        Ret = 0x08,
        ///<summary>shortcut calls with 0 parameters</summary>
        ShortCall0 = 0x09,
        ///<summary>shortcut calls with 0 parameters</summary> 
        ShortCall1 = 0x0A,
        ///<summary>shortcut calls with 0 parameters</summary>
        ShortCall2 = 0x0B,
        ///<summary>shortcut calls with 0 parameters</summary>
        ShortCall3 = 0x0C,
        ///<summary></summary>
        Call8Bit = 0x0D,
        ///<summary></summary>
        Call32Bit = 0x0E,
        ///<summary></summary>
        Sys8Bit = 0x0F,
        ///<summary></summary>
        IsDup = 0x10,
        ///<summary></summary>
        IsDrop = 0x11,
        ///<summary></summary>
        OsDup = 0x12,
        ///<summary></summary>
        OsDrop = 0x13,
        ///<summary>load (push) this object's field on stack top</summary>
        OsLoad8 = 0x14,
        ///<summary>save (pop) stack top to this object's field</summary>
        OsSave8 = 0x15,
        ///<summary></summary>
        OsLoad32 = 0x16,
        ///<summary></summary>
        OsSave32 = 0x17,
        ///<summary>create new object, class must be on stack</summary>
        New = 0x18,
        ///<summary>create new object, copy of stack top (just copy of data area as is)</summary>
        Copy = 0x19,
        ///<summary>n objects from ostack combine into the object. topmost is a class</summary>
        OsCompose32 = 0x1A,
        ///<summary>decompose topmost object on stack. deprecated?</summary>
        OsDecompose = 0x1B, // 
        ///<summary>
        /// copy opbject n steps down the ostack on top. pull 0 is dup;
        /// deprecated and was not implemented
        /// id(Osassign32,= 0x1D) // copy stack top opbject n steps down the ostack. pull 0 is nop 
        /// this is for local vars
        ///</summary>
        OsPull32 = 0x1C,
        ///<summary>get value from stack absolute-addressed slot, push on top</summary>
        OsGet32 = 0x1E,
        ///<summary>pop stack top, set value in stack absolute-addressed slot</summary>
        OsSet32 = 0x1F,
        ///<summary></summary>
        Iconst0 = 0x20,
        ///<summary></summary>
        Iconst1 = 0x21,
        ///<summary></summary>
        Iconst8bit = 0x22,
        ///<summary></summary>
        Iconst32bit = 0x23,
        ///<summary></summary>
        SconstBin = 0x24,
        ///<summary></summary>
        Iconst64bit = 0x25,
        // this is for integer local vars
        ///<summary>get value from stack absolute-addressed slot, push on top</summary>
        IsGet32 = 0x26,
        ///<summary>pop stack top, set value in stack absolute-addressed slot</summary>
        IsSet32 = 0x27,
        ///<summary>
        /// int32 follows - get constant with corresponding 
        /// index from object constant pool of this class
        ///</summary>
        ConstPool = 0x28,
        ///<summary>pop class, pop object, cast, push object</summary>
        Cast = 0x2B, // 
        ///<summary>jump address folows, top of o stack - class of objects to catch</summary>
        PushCatcher = 0x2D,
        ///<summary></summary>
        PopCatcher = 0x2E,
        ///<summary>
        /// thow top of stack, if stack is empty - will throw special system-wide object, 
        /// if on top of call stack - will kill thread in a bad way
        ///</summary>
        Throw = 0x2F,
        ///<summary></summary>
        SummonThread = 0x30,
        ///<summary></summary>
        SummonThis = 0x31,
        // 32-36
        ///<summary>null object</summary>
        SummonNull = 0x37,
        ///<summary></summary>
        SummonClassClass = 0x38,
        ///<summary></summary>
        SummonIntClass = 0x39,
        ///<summary></summary>
        SummonStringClass = 0x3A,
        ///<summary></summary>
        SummonInterfaceClass = 0x3B,
        ///<summary></summary>
        SummonCodeClass = 0x3C,
        ///<summary></summary>
        SummonArrayClass = 0x3D,
        ///<summary>string with class (or what?) name follows</summary>
        SummonByName = 0x3F,
        i2o = 0x40,
        o2i = 0x41,
        Isum = 0x42,
        Imul = 0x43,
        Isubul = 0x44,
        Isublu = 0x45,
        Idivul = 0x46,
        Idivlu = 0x47,
        ior = 0x48,
        iand = 0x49,
        ixor = 0x4A,
        inot = 0x4B,
        log_or = 0x4C,
        log_and = 0x4D,
        log_xor = 0x4E,
        log_not = 0x4F,
        // TODO: iload/isave
        ///<summary>load (push) this object's field on stack top</summary>
        IsLoad8 = 0x50,
        ///<summary>save (pop) stack top to this object's field</summary>
        IsSave8 = 0x51,
        ///<summary>>=</summary>
        ige = 0x52,
        ///<summary> <= </summary>
        ile = 0x53,
        ///<summary> > </summary>
        igt = 0x54,
        ///<summary> < </summary>
        ilt = 0x55,
        ///<summary> % </summary>
        iremul = 0x56,
        ///<summary></summary>
        iremlu = 0x57,
        // Compare two object pointers
        ///<summary>pointers are equal</summary>
        OsEq = 0x58,
        ///<summary>pointers are not equal</summary>
        OsNeq = 0x59,
        ///<summary>pointer is null</summary>
        OsIsnull = 0x5A,
        // BUG! Duplicate!
        //id(Ospush_null, = 0x5B)	// push null on stack
        // Prefixes - modify next op operands type
        ///<summary>next operation works on longs (uses 2x slots on int stack)</summary>
        PrefixLong = 0x5C,
        ///<summary>next operation works on floats (uses 1x slots on int stack)</summary>
        PrefixFloat = 0x5D,
        ///<summary>next operation works on doubles (uses 2x slots on int stack)</summary>
        PrefixDouble = 0x5E,
        //id(lock_this, = 0x60)		// mutex in 'this' is locked, automatic unlock on return
        //id(unlock_this, = 0x61)	// mutex in 'this' is unlocked
        ///<summary>mutex is locked on stack top.</summary>
        GeneralLock = 0x62,
        ///<summary> mutex is unlocked on stack top. </summary>
        GeneralUnlock = 0x63,
        // 64-6e
        ///<summary>
        /// no args. stack (from top): string method name, 
        /// this (or null for static), n_args, args</summary>
        DynamicInvoke = 0x6F,
        ///<summary>shift left</summary>
        Ishl = 0x70,
        ///<summary>shift right signed</summary>
        Ishr = 0x71,
        ///<summary>shift right unsigned</summary>
        Ushr = 0x72,
        // no 73 yet
        ///<summary>
        /// cast int from int stack to current (as defined by prefix) type on int stack
        ///</summary>
        FromI = 0x74,
        ///<summary>cast from long</summary>
        FromL = 0x75,
        ///<summary>cast from float</summary>
        FromF = 0x76,
        ///<summary>cast from double</summary>
        FromD = 0x77,
        // 73-7f
        // TODO kill shortcuts for we will have JIT anyway and bytecode size does not matter
        ///<summary>shortcut for syscall 0</summary>
        Sys0 = 0x80,
        Sys1 = 0x81,
        Sys2 = 0x82,
        Sys3 = 0x83,
        Sys4 = 0x84,
        Sys5 = 0x85,
        Sys6 = 0x86,
        Sys7 = 0x87,
        Sys8 = 0x88,
        Sys9 = 0x89,
        SysA = 0x8A,
        SysB = 0x8B,
        SysC = 0x8C,
        SysD = 0x8D,
        SysE = 0x8E,
        SysF = 0x8F,
        ///<summary>shortcut for call 0</summary>
        Call00 = 0xA0,
        Call01 = 0xA1,
        Call02 = 0xA2,
        Call03 = 0xA3,
        Call04 = 0xA4,
        Call05 = 0xA5,
        Call06 = 0xA6,
        Call07 = 0xA7,
        Call08 = 0xA8,
        Call09 = 0xA9,
        Call0A = 0xAA,
        Call0B = 0xAB,
        Call0C = 0xAC,
        Call0D = 0xAD,
        Call0E = 0xAE,
        Call0F = 0xAF,
        ///<summary>shortcut for call 16</summary>
        Call10 = 0xB0,
        Call11 = 0xB1,
        Call12 = 0xB2,
        Call13 = 0xB3,
        Call14 = 0xB4,
        Call15 = 0xB5,
        Call16 = 0xB6,
        Call17 = 0xB7,
        Call18 = 0xB8,
        Call19 = 0xB9,
        Call1A = 0xBA,
        Call1B = 0xBB,
        Call1C = 0xBC,
        Call1D = 0xBD,
        Call1E = 0xBE,
        Call1F = 0xBF
        // c0-cf
        // d0-df
        // e0-ef
        // f0-ff
    }
}
