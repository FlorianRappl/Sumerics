namespace Sumerics.Api
{
    using YAMP;

    [Description("Stops all currently active computations.")]
    [Kind("UI")]
    sealed class StopFunction : ArgumentFunction
    {
        readonly IApplication _application;

        public StopFunction(IApplication application)
        {
            _application = application;
        }

        [Description("Sumerics performs every computation in a new thread, giving you great flexibility and a smooth UI. Therefore the only way to actively stop a computation is to kill it. This function kills all outstanding threads, which stops the related computations.")]
        public void Function()
        {
            _application.Kernel.StopAll();
        }
    }
}
