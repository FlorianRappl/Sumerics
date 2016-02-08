namespace Sumerics
{
    using YAMP;
    using YAMP.Converter;

    [Description("Modifies the UI in such a sense that it opens the specified Window. Note: Not all Dialogs / Windows can be opened from the console.")]
    [Kind("UI")]
    sealed class WindowFunction : ArgumentFunction
    {
        static IContainer _container;

        public static IFunction Create(IContainer container)
        {
            _container = container;
            return new WindowFunction();
        }

        [Description("Opens the specified window.")]
        [Example("window(\"savews\")", "Opens the dialog for saving the current workspace.")]
        [Example("window(\"editor\")", "Opens the file / script editor.")]
        public void Function(StringValue window)
        {
            var conv = new StringToEnumConverter(typeof(Windows));
            var value = (Windows)conv.Convert(window);

            switch (value)
            {
                case Windows.Editor:
                    App.Window.OpenEditorWindow(_container);
                    break;
                case Windows.LoadWS:
                    App.Window.OpenLoadWindow(_container);
                    break;
                case Windows.SaveWS:
                    App.Window.OpenSaveWindow(_container);
                    break;
                case Windows.Help:
                    App.Window.OpenHelpWindow(_container);
                    break;
                case Windows.Demos:
                    App.Window.OpenDocumentationWindow(_container);
                    break;
                case Windows.About:
                    App.Window.OpenAboutWindow(_container);
                    break;
                case Windows.Options:
                    App.Window.OpenOptionsWindow(_container);
                    break;
                case Windows.Directory:
                    App.Window.OpenDirectoryWindow(_container);
                    break;
            }
        }

        enum Windows
        {
            Editor,
            LoadWS,
            SaveWS,
            Directory,
            Help,
            Demos,
            About,
            Options
        }
    }
}
