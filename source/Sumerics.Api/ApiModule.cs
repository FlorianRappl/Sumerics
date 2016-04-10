namespace Sumerics.Api
{
    public class ApiModule : IModule
    {
        public void RegisterComponents(Services components)
        {
            var application = components.Get<IApplication>();
            var provider = new ApiProvider(application);
            components.Register<IApiProvider>(provider);
        }
    }
}
