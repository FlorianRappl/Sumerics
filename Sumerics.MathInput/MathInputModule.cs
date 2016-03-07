namespace Sumerics.MathInput
{
    public sealed class MathInputModule : IModule
    {
        public void RegisterComponents(Services components)
        {
            var service = new MathInputService();
            components.Register<IMathInputService>(service);
        }
    }
}
