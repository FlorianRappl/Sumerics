namespace Sumerics.Dialogs
{
    using Sumerics.MathInput;
    using Sumerics.Resources;
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using System.IO;
    using System.Windows;

    [DialogType(Dialog.Editor)]
    sealed class EditorDialogHandler : IDialogHandler
    {
        readonly IApplication _app;

        public EditorDialogHandler(IApplication app)
        {
            _app = app;
        }

        public void Open(params Object[] parameters)
        {
            var editor = DialogExtensions.Get<EditorWindow>() as Window;

            if (editor == null)
            {
                var kernel = _app.Get<Kernel>();
                var console = _app.Get<IConsole>();
                var service = _app.Get<IMathInputService>();
                var context = new EditorViewModel(kernel, console, service);
                editor = WindowFactory.Instance.Create(context);
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
                        var output = new OutputViewModel 
                        { 
                            Message = message, 
                            Title = Messages.UnexpectedError 
                        };
                        output.ShowWindow();
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
