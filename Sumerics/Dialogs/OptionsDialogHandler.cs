namespace Sumerics.Dialogs
{
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;

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
            var current = DialogExtensions.Get<OptionsWindow>();

            if (current == null)
            {
                var settings = _container.Get<ISettings>();
                var dialogs = _container.Get<IDialogManager>();
                var vm = new OptionsViewModel(settings, dialogs);
                current = new OptionsWindow(vm);
            }

            current.Show();
        }

        public void Close()
        {
            this.Close<OptionsWindow>();
        }
    }
}
