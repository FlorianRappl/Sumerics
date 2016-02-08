namespace Sumerics
{
    public interface IApplication
    {
        void Shutdown();

        IConsole Console { get; }
    }
}
