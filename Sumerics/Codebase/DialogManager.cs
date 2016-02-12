namespace Sumerics
{
    using Sumerics.Views;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    sealed class DialogManager : IDialogManager
    {
        #region Fields

        readonly IContainer _container;
        readonly Dictionary<Dialog, Func<Window>> _handlers;

        #endregion

        #region ctor

        public DialogManager(IContainer container)
        {
            _container = container;
            _handlers = new Dictionary<Dialog, Func<Window>>();

            _handlers.Add(Dialog.About, Obtain<AboutWindow>);
            _handlers.Add(Dialog.Demos, Obtain<DemoBrowser>);
            _handlers.Add(Dialog.Directory, ObtainFolderBrowser);
            _handlers.Add(Dialog.Editor, Obtain<EditorWindow>);
            _handlers.Add(Dialog.Help, Obtain<HelpWindow>);
            _handlers.Add(Dialog.LoadWorkspace, ObtainLoadWorkspace);
            _handlers.Add(Dialog.Options, Obtain<OptionsWindow>);
            _handlers.Add(Dialog.SaveWorkspace, ObtainSaveWorkspace);
        }

        #endregion

        #region Methods

        public void Open(Dialog value)
        {
            var handler = default(Func<Window>);

            if (_handlers.TryGetValue(value, out handler))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    var window = handler();

                    if (window != null)
                    {
                        window.Show();
                    }
                });
            }
        }

        public void Close(Dialog value)
        {
            var handler = default(Func<Window>);

            if (_handlers.TryGetValue(value, out handler))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    var window = handler();

                    if (window != null)
                    {
                        window.Close();
                    }
                });
            }
        }

        #endregion

        #region Helpers

        Window ObtainFolderBrowser()
        {
            var dialog = Obtain<FolderBrowseWindow>();
            dialog.ShowDialog();

            if (dialog.Accepted)
            {
                Environment.CurrentDirectory = dialog.SelectedDirectory;
            }

            return null;
        }

        Window ObtainLoadWorkspace()
        {
            var dialog = Obtain<OpenFileWindow>();
            dialog.Title = "Open workspace ...";
            dialog.AddFilter("All files (*.*)", "*.*");
            dialog.AddFilter("Sumerics workspace (*.sws)", "*.sws");
            dialog.ShowDialog();

            if (dialog.Accepted)
            {
                _container.Get<IKernel>().LoadWorkspaceAsync(dialog.SelectedFile);
            }

            return null;
        }

        Window ObtainSaveWorkspace()
        {
            var dialog = Obtain<SaveFileWindow>();
            dialog.Title = "Save workspace as ...";
            dialog.AddFilter("Sumerics workspace (*.sws)", "*.sws");
            dialog.ShowDialog();

            if (dialog.Accepted)
            {
                _container.Get<IKernel>().SaveWorkspaceAsync(dialog.SelectedFile);
            }

            return null;
        }

        T Obtain<T>()
            where T : Window
        {
			foreach (var window in App.Current.Windows)
			{
                var dialog = window as T;

				if (dialog != null)
				{
					dialog.Activate();
					return dialog;
				}
			}

			return _container.Create<T>();
        }

        #endregion
    }
}
