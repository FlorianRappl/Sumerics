namespace Sumerics
{
    using System;
    using System.Collections.Generic;

    public abstract class TypeFactory<TKey, TProduct> : IFactory<TKey, TProduct>
    {
        readonly Dictionary<Type, Func<TKey, TProduct>> _mapping;
        readonly Boolean _requireExactMatch;

        public TypeFactory(Boolean requireExactMatch = true)
        {
            _mapping = new Dictionary<Type, Func<TKey, TProduct>>();
            _requireExactMatch = requireExactMatch;
        }

        public void Register<T>(Func<T, TProduct> creator)
            where T : TKey
        {
            _mapping.Add(typeof(T), key => creator((T)key));
        }

        public TProduct Create(TKey value)
        {
            var type = value.GetType();
            var closest = default(Func<TKey, TProduct>);

            foreach (var pair in _mapping)
            {
                if (type == pair.Key)
                {
                    closest = pair.Value;
                    return closest(value);
                }
                else if (type.IsSubclassOf(pair.Key))
                {
                    closest = pair.Value;
                }
            }

            if (!_requireExactMatch && closest != null)
            {
                return closest(value);
            }

            return CreateDefault();
        }

        protected abstract TProduct CreateDefault();
    }
}
