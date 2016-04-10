namespace Sumerics.Dialogs
{
    using Sumerics.Commands;
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using System.Windows;
    using YAMP.Help;

    [DialogType(Dialog.Help)]
    sealed class HelpDialogHandler : IDialogHandler
    {
        readonly IApplication _app;

        public HelpDialogHandler(IApplication app)
        {
            _app = app;
        }

        public void Open(params Object[] parameters)
        {
            Window window = DialogExtensions.Get<HelpWindow>();

            if (window == null)
            {
                var kernel = _app.Get<IKernel>() as Kernel;
                var commands = _app.Get<ICommandFactory>();
                var context = new DocumentationViewModel(kernel.Help, commands);
                window = WindowFactory.Instance.Create(context);
            }
            else
            {
                window.Activate();
            }

            if (parameters.Length == 1 && parameters[0] is HelpSection)
            {
                var model = window.DataContext as DocumentationViewModel;

                if (model != null)
                {
                    model.Topic = (HelpSection)parameters[0];
                }
            }

            window.Show();
        }

        public void Close()
        {
            this.Close<HelpWindow>();
        }
    }
}
