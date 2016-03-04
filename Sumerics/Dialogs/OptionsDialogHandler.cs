namespace Sumerics.Dialogs
{
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using System.Windows;

    [DialogType(Dialog.Options)]
    sealed class OptionsDialogHandler : IDialogHandler
    {
        readonly IComponents _container;

        public OptionsDialogHandler(IComponents container)
        {
            _container = container;
        }

        public void Open(params Object[] parameters)
        {
            Window current = DialogExtensions.Get<OptionsWindow>();

            if (current == null)
            {
                var settings = _container.Get<ISettings>();
                var dialogs = _container.Get<IDialogManager>();
                var context = new OptionsViewModel(settings, dialogs);
                current = WindowFactory.Instance.Create(context);
            }

            current.Show();
        }

        public void Close()
        {
            this.Close<OptionsWindow>();
        }
    }
}
