using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP.Tables.Maps
{
    class LogicOperatorMapping : IEnumerable<LogicMappingItem>
    {
        #region Members

        static LogicOperatorMapping _empty;

        List<LogicMappingItem> functions;

        #endregion

        #region ctor

        public LogicOperatorMapping()
        {
            functions = new List<LogicMappingItem>();
        }

        #endregion

        #region Properties

        public static LogicOperatorMapping Empty
        {
            get { return _empty ?? (_empty = new LogicOperatorMapping()); }
        }

        #endregion

        #region Methods

        public void Add(Func<object, object, object> method, IType type, IType source)
        {
            functions.Add(new LogicMappingItem { Method = method, Type = type, Source = source });
        }

        public void Remove(IType source)
        {
            for (int i = functions.Count - 1; i >= 0; i--)
            {
                if (functions[i].Source == source)
                    functions.RemoveAt(i);
            }
        }

        public IEnumerator<LogicMappingItem> GetEnumerator()
        {
            return functions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)functions).GetEnumerator();
        }

        public Func<object, object, object> Resolve(ref object left, ref object right)
        {
            LogicMappingItem possible = null;
            int maxDirectMatches = -1;

            for (int i = 0; i < functions.Count; i++)
            {
                var directMatches = 0;
                var item = functions[i];

                if (item.Type.IsDirect(left))
                    directMatches++;
                else if (!item.Type.IsIndirect(left))
                    continue;

                if (item.Type.IsDirect(right))
                    directMatches++;
                else if (!item.Type.IsIndirect(right))
                    continue;

                if (directMatches == 2)
                    return item.Method;

                if (directMatches > maxDirectMatches)
                {
                    maxDirectMatches = directMatches;
                    possible = item;
                }
            }

            if (possible != null)
            {
                left = possible.Type.Cast(left);
                right = possible.Type.Cast(right);

                return possible.Method;
            }

            return null;
        }

        #endregion
    }
}
