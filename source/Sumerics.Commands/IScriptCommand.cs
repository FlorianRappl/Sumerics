namespace Sumerics.Commands
{
    using System;

    public interface IScriptCommand
    {
        Boolean CanExecute(Int32 parameters);

        String Execute(String[] values);
    }
}
