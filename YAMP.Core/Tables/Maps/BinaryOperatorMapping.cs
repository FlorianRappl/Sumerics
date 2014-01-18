using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP.Tables.Maps
{
    sealed class BinaryOperatorMapping : IEnumerable<BinaryMappingItem>
    {
        #region Members

        static BinaryOperatorMapping _empty;

        List<BinaryMappingItem> functions;

        #endregion

        #region ctor

        public BinaryOperatorMapping()
        {
            functions = new List<BinaryMappingItem>();
        }

        #endregion

        #region Properties

        public static BinaryOperatorMapping Empty
        {
            get { return _empty ?? (_empty = new BinaryOperatorMapping()); }
        }

        #endregion

        #region Methods

        public void Add(Func<object, object, object> method, IType left, IType right, IType source)
        {
            functions.Add(new BinaryMappingItem { Method = method, LeftType = left, RightType = right, Source = source });
        }

        public void Remove(IType source)
        {
            for (int i = functions.Count - 1; i >= 0; i--)
            {
                if (functions[i].Source == source)
                    functions.RemoveAt(i);
            }
        }

        public IEnumerator<BinaryMappingItem> GetEnumerator()
        {
            return functions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)functions).GetEnumerator();
        }

        public Func<object, object, object> Resolve(ref object left, ref object right)
        {
            BinaryMappingItem possible = null;
            int maxDirectMatches = -1;

            for (int i = 0; i < functions.Count; i++)
            {
                var directMatches = 0;
                var item = functions[i];

                if (item.LeftType.IsDirect(left))
                    directMatches++;
                else if (!item.LeftType.IsIndirect(left))
                    continue;

                if (item.RightType.IsDirect(right))
                    directMatches++;
                else if (!item.RightType.IsIndirect(right))
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
                left = possible.LeftType.Cast(left);
                right = possible.RightType.Cast(right);

                return possible.Method;
            }

            return null;
        }

        #endregion
    }
}
