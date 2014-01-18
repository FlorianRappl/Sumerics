using System;
using System.Reflection;
using YAMP.Core;

namespace YAMP.Tables.Maps
{
    sealed class MappingItem
    {
        public IType Source 
        {
            get; 
            set; 
        }

        public Func<Dynamic[], Dynamic> Method
        { 
            get; 
            set; 
        }

        public IType[] Types
        {
            get;
            set;
        }
    }
}
