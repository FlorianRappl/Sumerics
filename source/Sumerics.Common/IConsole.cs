namespace Sumerics
{
    using System;

    public interface IConsole
    {
        void Clear();

        void Execute(String query);

        void Execute(String query, String message);
    }
}
