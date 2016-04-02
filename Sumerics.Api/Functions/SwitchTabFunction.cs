namespace Sumerics.Api
{
    using YAMP;

    [Description("Modifies the UI in such a sense that it switches to a specific tab (1-n) of the Sumerics main window.")]
    [Kind("UI")]
    sealed class SwitchTabFunction : ArgumentFunction
    {
        readonly IApplication _application;

        public SwitchTabFunction(IApplication application)
        {
            _application = application;
        }

        [Description("Switches to the specified index.")]
        [Example("switchtab(3)", "Opens the visualization tab.")]
        public void Function(ScalarValue index)
        {
            var tabs = _application.Get<ITabs>();
            tabs.Change(index.IntValue - 1);
        }
    }
}
