namespace Sumerics
{
    public interface IFactory<TKey, TProduct>
    {
        TProduct Create(TKey value);
    }
}
