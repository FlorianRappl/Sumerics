using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using YAMP.Core;

namespace YAMP.Tables.Maps
{
    sealed class UnaryOperatorMapping : IEnumerable<UnaryMappingItem>
    {
        #region Members

        static UnaryOperatorMapping _empty;

        List<UnaryMappingItem> functions;

        #endregion

        #region ctor

        public UnaryOperatorMapping()
        {
            functions = new List<UnaryMappingItem>();
        }

        #endregion

        #region Properties

        public static UnaryOperatorMapping Empty
        {
            get { return _empty ?? (_empty = new UnaryOperatorMapping()); }
        }

        #endregion

        #region Methods

        public void Add(Func<object, object> method, IType type, IType source)
        {
            functions.Add(new UnaryMappingItem { Method = method, Type = type, Source = source });
        }

        public void Remove(IType source)
        {
            for (int i = functions.Count - 1; i >= 0; i--)
            {
                if (functions[i].Source == source)
                    functions.RemoveAt(i);
            }
        }

        public IEnumerator<UnaryMappingItem> GetEnumerator()
        {
            return functions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)functions).GetEnumerator();
        }

        public Func<object, object> Resolve(ref object obj)
        {
            UnaryMappingItem possible = null;

            for (int i = 0; i < functions.Count; i++)
            {
                var item = functions[i];

                if (item.Type.IsDirect(obj))
                    return item.Method;
                else if (!item.Type.IsIndirect(obj))
                    continue;
                else
                    possible = item;
            }

            if (possible != null)
            {
                obj = possible.Type.Cast(obj);
                return possible.Method;
            }

            return null;
        }

        #endregion
    }
}
