using System;
using YAMP.Tables;

namespace YAMP.Core
{
    class Context : IObject
    {
        readonly VariableContainer _vars;
        readonly FunctionContainer _fcts;
        readonly ConstantContainer _csts;

        public Context()
        {
            _vars = new VariableContainer();
            _fcts = new FunctionContainer();
            _csts = new ConstantContainer();
        }

        public ConstantContainer Constants
        {
            get { return _csts; }
        }

        public FunctionContainer Functions
        {
            get { return _fcts; }
        }

        public VariableContainer Variables
        {
            get { return _vars; }
        }

        public String[] Methods
        {
            get { throw new NotImplementedException(); }
        }

        public String[] Properties
        {
            get { throw new NotImplementedException(); }
        }

        public CustomFunction GetMethod(String name)
        {
            throw new NotImplementedException();
        }

        public bool HasMethod(String name)
        {
            throw new NotImplementedException();
        }

        public bool HasProperty(String name)
        {
            throw new NotImplementedException();
        }

        public bool TryReadProperty(String name, out Dynamic value)
        {
            throw new NotImplementedException();
        }

        public bool TryWriteProperty(String name, Dynamic value)
        {
            throw new NotImplementedException();
        }
    }
}
