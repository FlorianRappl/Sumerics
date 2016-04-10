namespace Sumerics.Commands
{
    using System;

    public interface ICommandFactory
    {
        Boolean HasCommand(String command);

        Boolean HasOverload(String command, Int32 parameters);

        String TryCommand(String input);
    }
}
