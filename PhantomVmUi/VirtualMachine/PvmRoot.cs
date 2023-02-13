using System;
using System.Collections.Generic;
using Cc.Anba.PhantomOs.VirtualMachine.PvmObjects;

namespace Cc.Anba.PhantomOs.VirtualMachine
{
    public class PvmRoot
    {
        #region Private variables

        private PvmClass _nullClass;
        private PvmClass _classClass;
        private PvmClass _interfaceClass;
        private PvmClass _codeClass;
        private PvmClass _intClass;
        private PvmClass _stringClass;
        private PvmClass _arrayClass;

        private PvmNull _nullObject;

        #endregion

        #region Fields

        /// <summary>
        /// List of all created objects for debug and visualisation purposes.
        /// </summary>
        public List<PvmObject> ObjList { get; set; } = new List<PvmObject>();

        #endregion

        #region Ctor

        /// <summary>
        /// creates internal classes as in pvm_create_root_objects
        /// for futher references
        /// </summary>
        public PvmRoot()
        {
            // creates internal classes as in pvm_create_root_objects

            // Partially build interface object 
            // with null interface class reference
            var sysInterfaceObject = new PvmInterface(null, 64); // N_SYS_METHODS
            ObjList.Add(sysInterfaceObject);
            // interface of interface object is itself
            sysInterfaceObject.Iface = sysInterfaceObject;        

            // Crate null object with recently created interface.
            // Not a common way for creating objects.
            // In common way object interface should be taken from its
            // class default interface.
            var nullObject = new PvmNull();
            ObjList.Add(nullObject);
            // set null object interface manually.
            nullObject.Iface = sysInterfaceObject;
            _nullObject = nullObject;

            // initialize internal classes without specifiing its names
            // because PvmString class is not exists yet.
            var nullClass = new PvmClass(sysInterfaceObject);
            ObjList.Add(nullClass);
            nullClass.Parent = nullClass;
            nullClass.IfaceDefault = sysInterfaceObject;
            _nullClass = nullClass;

            nullObject.Class = nullClass;

            var classClass = new PvmClass(sysInterfaceObject);
            ObjList.Add(classClass);
            classClass.Parent = nullClass;
            classClass.IfaceDefault = sysInterfaceObject;
            classClass.Class = classClass;
            _classClass= classClass;

            nullClass.Class = classClass;

            var interfaceClass = new PvmClass(sysInterfaceObject);
            ObjList.Add(interfaceClass);
            interfaceClass.Parent = nullClass;
            interfaceClass.IfaceDefault = sysInterfaceObject;
            interfaceClass.Class = classClass;
            _interfaceClass = interfaceClass;

            sysInterfaceObject.Class = interfaceClass;

            var stringClass = new PvmClass(sysInterfaceObject);
            ObjList.Add(stringClass);
            stringClass.Parent = nullClass;
            stringClass.IfaceDefault = sysInterfaceObject;
            stringClass.Class = classClass;
            _stringClass = stringClass;

            // It must be ok to create strings in usual way now
            nullClass.Name = NewString(".internal.void");
            classClass.Name = NewString(".internal.class");
            interfaceClass.Name = NewString(".internal.interface");
            stringClass.Name = NewString(".internal.string");

            // Standart class create. Creates PvmObject of specific class.
            // paren class and default interface should be specified.
            var codeClass = NewClass(NewString(".internal.code"),
                nullClass, sysInterfaceObject);
            _codeClass = codeClass;

            // Create other classes by common way.
            var intClass = NewClass(NewString(".internal.int"),
                nullClass, sysInterfaceObject);
            _intClass = intClass;

            var arrayClass = NewClass(NewString(".internal.container.array"),
                nullClass, sysInterfaceObject);
            _arrayClass = arrayClass;
        }

        #endregion

        #region Private methods

        internal PvmNull NewNull()
        {
            // already created once forever
            return _nullObject;
        }

        internal PvmClass NewClass(PvmString name,
            PvmClass parent, PvmInterface ifaceDefault)
        {
            if (_classClass == null)
                throw new NullReferenceException();
            var pvmClass = new PvmClass(_classClass,
                name, parent, ifaceDefault);
            ObjList.Add(pvmClass);
            return pvmClass;
        }

        internal PvmClass NewClass(PvmString name,
            PvmClass parent, PvmInterface ifaceDefault,
            PvmArray<PvmString> methodNames,
            PvmArray<PvmString> fieldNames)
        {
            if (_classClass == null)
                throw new NullReferenceException();
            var pvmClass = new PvmClass(_classClass,
                name, parent, ifaceDefault,
                methodNames, fieldNames);
            ObjList.Add(pvmClass);
            return pvmClass;
        }

        /// <summary>
        /// New PvmObject of interface class.
        /// </summary>
        /// <param name="nMethods"></param>
        /// <returns></returns>
        internal PvmInterface NewInterface(int nMethods)
        {
            if (_interfaceClass == null)
                throw new NullReferenceException();
            var pvmInterface = new PvmInterface(_interfaceClass,
                nMethods);
            ObjList.Add(pvmInterface);
            return pvmInterface;
        }

        internal PvmInterface NewInterface(PvmClass baseClass)
        {
            if (_interfaceClass == null)
                throw new NullReferenceException();
            var pvmInterface = new PvmInterface(_interfaceClass,
                baseClass);
            ObjList.Add(pvmInterface);
            return pvmInterface;
        }

        internal PvmObject NewObject(PvmClass objectClass)
        {
            if (objectClass == null)
                throw new NullReferenceException();
            PvmArray<PvmObject> fields = null;
            if (objectClass.FieldNames != null)
                fields = NewArrayObject(objectClass.FieldNames.GetLength());
            var pvmObject = new PvmObject(objectClass, fields);
            ObjList.Add(pvmObject);
            return pvmObject;
        }

        internal PvmCode NewCode(OpCode[] code)
        {
            if (_codeClass == null)
                throw new NullReferenceException();
            var pvmCode = new PvmCode(_codeClass, code);
            ObjList.Add(pvmCode);
            return pvmCode;
        }

        internal PvmInt NewInt(int i)
        {
            if (_intClass == null)
                throw new NullReferenceException();
            var pvmInt = new PvmInt(_intClass, i);
            ObjList.Add(pvmInt);
            return pvmInt;
        }

        internal PvmString NewString(string str)
        {
            if (_stringClass == null)
                throw new NullReferenceException();
            var pvmString = new PvmString(_stringClass, str);
            ObjList.Add(pvmString);
            return pvmString;
        }

        internal PvmArray<PvmObject> NewArrayObject(int nSlots)
        {
            if (_arrayClass == null)
                throw new NullReferenceException();
            var pvmArray = new PvmArray<PvmObject>(_arrayClass, nSlots);
            //, new PvmBootSc(this, SciBoot));
            ObjList.Add(pvmArray);
            return pvmArray;
        }

        internal PvmArray<PvmString> NewArrayString(int nSlots)
        {
            if (_arrayClass == null)
                throw new NullReferenceException();
            var pvmArray = new PvmArray<PvmString>(_arrayClass, nSlots);
            ObjList.Add(pvmArray);
            return pvmArray;
        }

        internal PvmClass GetClassByName(string className)
        {
            switch (className)
            {
                case ".internal.object":
                    return _nullClass;
            }
            return _nullClass;
        }

        #endregion
    }
}
