namespace Sumerics.Dialogs
{
    using Sumerics.MathInput;
    using Sumerics.Resources;
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
                var context = new EditorViewModel(kernel, console, service);
                editor = new EditorWindow { DataContext = context };
            }

            if (parameters.Length == 1 && parameters[0] is String)
            {
                var context = editor.DataContext as EditorViewModel;
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
                        var message = String.Format(Messages.ErrorCannotCreateFile, file);
                        OutputDialog.Show(Messages.UnexpectedError, message);
                        return;
                    }
                }

                if (context != null)
                {
                    context.OpenFile(file);
                }
            }
            
            editor.Show();
        }

        public void Close()
        {
            this.Close<EditorWindow>();
        }
    }
}
