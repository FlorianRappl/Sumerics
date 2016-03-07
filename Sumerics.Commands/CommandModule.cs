namespace Sumerics.Commands
{
    public sealed class CommandModule : IModule
    {
        public void RegisterComponents(Services components)
        {
            var commands = new CommandFactory(components);
            commands.RegisterCommands();
            components.Register<ICommandFactory>(commands);
        }
    }
}
