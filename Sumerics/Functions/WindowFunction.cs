using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;
using YAMP.Converter;

namespace Sumerics
{
    [Description("Modifies the UI in such a sense that it opens the specified Window. Note: Not all Dialogs / Windows can be opened from the console.")]
    [Kind("UI")]
    sealed class WindowFunction : ArgumentFunction
    {
        public static IFunction Create()
        {
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
                    App.Window.OpenEditorWindow();
                    break;
                case Windows.LoadWS:
                    App.Window.OpenLoadWindow();
                    break;
                case Windows.SaveWS:
                    App.Window.OpenSaveWindow();
                    break;
                case Windows.Help:
                    App.Window.OpenHelpWindow();
                    break;
                case Windows.Demos:
                    App.Window.OpenDocumentationWindow();
                    break;
                case Windows.About:
                    App.Window.OpenAboutWindow();
                    break;
                case Windows.Options:
                    App.Window.OpenOptionsWindow();
                    break;
                case Windows.Directory:
                    App.Window.OpenDirectoryWindow();
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
