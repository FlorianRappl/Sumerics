namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.MathInput;
    using Sumerics.ViewModels;
    using System;
    using System.IO;

    /// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsWindow : MetroWindow
	{
		public OptionsWindow(IApplication app)
		{
            InitializeComponent();
            var callback = CreateEditorCallback(app);
            DataContext = new OptionsViewModel(app.Settings, callback);
		}

        static Action<String> CreateEditorCallback(IApplication app)
        {
            return file =>
            {
                var kernel = app.Components.Get<Kernel>();
                var console = app.Components.Get<IConsole>();
                var service = app.Components.Get<IMathInput>();
                var vm = new EditorViewModel(kernel, console, service);
                LoadEditor(vm, file);
            };
        }

        static void LoadEditor(EditorViewModel vm, String file)
        {
            if (!File.Exists(file))
            {
                var dir = Path.GetDirectoryName(file);

                try
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.Create(file).Close();
                }
                catch
                {
                    OutputDialog.Show("Unexpected error",
                        "The file " + file + " does not exist and could not be created. Usually this is due to unsufficients privileges. Try running Sumerics in admin mode or create the file on your own to get rid of this exception.");
                    return;
                }
            }

            var editor = new EditorWindow(vm);
            editor.OpenFile(file);
        }
	}
}
