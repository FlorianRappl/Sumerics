namespace Sumerics.Dialogs
{
    using Sumerics.MathInput;
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using System.IO;

    [DialogType(Dialog.Editor)]
    sealed class EditorDialogHandler : IDialogHandler
    {
        readonly IComponents _container;

        public EditorDialogHandler(IComponents container)
        {
            _container = container;
        }

        public void Open(params Object[] parameters)
        {
            var editor = DialogExtensions.Get<EditorWindow>();

            if (editor == null)
            {
                var kernel = _container.Get<Kernel>();
                var console = _container.Get<IConsole>();
                var service = _container.Get<IMathInputService>();
                var vm = new EditorViewModel(kernel, console, service);
                editor = new EditorWindow(vm);
            }

            if (parameters.Length == 1 && parameters[0] is String)
            {
                var file = (String)parameters[0];

                if (!File.Exists(file))
                {
                    var directory = Path.GetDirectoryName(file);

                    try
                    {
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        File.Create(file).Close();
                    }
                    catch
                    {
                        var message = String.Format("The file {0} does not exist and could not be created. Usually this is due to unsufficients privileges. Try running Sumerics in admin mode or create the file on your own to get rid of this exception.", file);
                        OutputDialog.Show("Unexpected error", message);
                        return;
                    }
                }

                editor.OpenFile(file);
            }
            
            editor.Show();
        }

        public void Close()
        {
            this.Close<EditorWindow>();
        }
    }
}
