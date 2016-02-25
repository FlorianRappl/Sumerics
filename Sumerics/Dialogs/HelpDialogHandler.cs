namespace Sumerics.Dialogs
{
    using Sumerics.Commands;
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using YAMP.Help;

    [DialogType(Dialog.Help)]
    sealed class HelpDialogHandler : IDialogHandler
    {
        readonly IComponents _container;

        public HelpDialogHandler(IComponents container)
        {
            _container = container;
        }

        public void Open(params Object[] parameters)
        {
            var window = DialogExtensions.Get<HelpWindow>();

            if (window == null)
            {
                var kernel = _container.Get<IKernel>() as Kernel;
                var commands = _container.Get<ICommandFactory>();
                window = new HelpWindow(commands)
                {
                    DataContext = new DocumentationViewModel(kernel.Help)
                };
            }

            if (parameters.Length == 1 && parameters[0] is HelpSection)
            {
                window.Topic = (HelpSection)parameters[0];
            }

            window.Show();
        }

        public void Close()
        {
            this.Close<HelpWindow>();
        }
    }
}
