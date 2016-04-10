namespace Sumerics.Api
{
    using YAMP;
    using YAMP.Converter;

    [Description("Modifies the UI in such a sense that it opens the specified Window. Note: Not all Dialogs / Windows can be opened from the console.")]
    [Kind("UI")]
    sealed class WindowFunction : ArgumentFunction
    {
        readonly IApplication _application;

        public WindowFunction(IApplication application)
        {
            _application = application;
        }

        [Description("Opens the specified window.")]
        [Example("window(\"savews\")", "Opens the dialog for saving the current workspace.")]
        [Example("window(\"editor\")", "Opens the file / script editor.")]
        public void Function(StringValue window)
        {
            var converter = new StringToEnumConverter(typeof(Dialog));
            var dialog = (Dialog)converter.Convert(window);
            var dialogManager = _application.Get<IDialogManager>();
            dialogManager.Open(dialog);
        }
    }
}
