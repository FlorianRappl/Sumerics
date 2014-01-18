using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Core;
using YAMP.Types;

namespace YAMP.Tables.Maps
{
    sealed class MethodMapping : IEnumerable<MappingItem>, IFunctionResolver
    {
        #region Members

        List<MappingItem> functions;

        #endregion

        #region ctor

        public MethodMapping(IEnumerable<IFunctionItem> items)
        {
            functions = new List<MappingItem>();

            foreach (var item in items)
            {
                IType[] types = new IType[item.Parameters.Length];

                for (int i = 0; i < types.Length; i++)
                    types[i] = Dynamic.FindType(item.Parameters[i]);

                Add(item.Function, types);
            }
        }

        #endregion

        #region Methods

        public void Add(Func<Dynamic[], Dynamic> method, IType[] types)
        {
            functions.Add(new MappingItem { Method = method, Types = types });
        }

        public IEnumerator<MappingItem> GetEnumerator()
        {
            return functions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return functions.GetEnumerator();
        }

        public Func<Dynamic[], Dynamic> Resolve(Dynamic[] parameters)
        {
            MappingItem possible = null;
            int maxDirectMatches = -1;

            for (int i = 0; i < functions.Count; i++)
            {
                if (functions[i].Types.Length != parameters.Length)
                    continue;

                var directMatches = 0;
                var indirectMatches = 0;
                var item = functions[i];

                for (int j = 0; j < parameters.Length; j++)
                {
                    if (item.Types[j] is ObjectType)
                        indirectMatches++;

                    var metric = item.Types[j].RelationTo(parameters[j].Type);

                    if (metric == TypeMetric.Exact)
                        directMatches++;
                    else if (metric == TypeMetric.None)
                        break;
                    else
                        indirectMatches++;
                }

                if (directMatches == parameters.Length)
                    return item.Method;

                if (directMatches + indirectMatches < parameters.Length)
                    continue;

                if (directMatches > maxDirectMatches)
                {
                    maxDirectMatches = directMatches;
                    possible = item;
                }
            }

            if (possible != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (possible.Types[i] != null)
                        parameters[i] = new Dynamic(possible.Types[i].Convert(parameters[i]));
                }

                return possible.Method;
            }

            return null;
        }

        #endregion
    }
}
