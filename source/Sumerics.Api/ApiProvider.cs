namespace Sumerics.Api
{
    using YAMP;

    sealed class ApiProvider : IApiProvider
    {
        readonly IApplication _application;

        public ApiProvider(IApplication application)
        {
            _application = application;
        }

        public void RegisterApi(ParseContext context)
        {
            context.AddFunction("switchtab", new SwitchTabFunction(_application));
            context.AddFunction("undock", new UndockFunction(_application));
            context.AddFunction("window", new WindowFunction(_application));
            context.AddFunction("dock", new DockFunction(_application));
            context.AddFunction("stop", new StopFunction(_application));
        }
    }
}
