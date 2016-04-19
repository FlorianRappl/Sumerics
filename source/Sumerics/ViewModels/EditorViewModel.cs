namespace Sumerics.ViewModels
{
    using Sumerics.Controls;
    using Sumerics.MathInput;
    using Sumerics.Resources;
    using Sumerics.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// ViewModel for the editor in general.
    /// </summary>
    public sealed class EditorViewModel : BaseViewModel
    {
        #region Fields

        readonly Kernel _kernel;
        readonly IConsole _console;
        readonly IMathInputService _service;
        readonly ICommand _create;
        readonly ICommand _open;
        readonly ObservableCollection<EditorFileViewModel> _files;
        EditorFileViewModel selectedFile;

        #endregion

        #region ctor

        public EditorViewModel(Kernel kernel, IConsole console, IMathInputService service)
        {
            _kernel = kernel;
            _console = console;
            _service = service;
            _files = new ObservableCollection<EditorFileViewModel>();
            _files.Add(new EditorFileViewModel(this, kernel));
            _create = new RelayCommand(x =>
            {
                var newfile = new EditorFileViewModel(this, _kernel);
                Files.Add(newfile);
                SelectedFile = newfile;
            });
            _open = new RelayCommand(x =>
            {
                var context = new OpenFileViewModel();
                context.AddFilter(Messages.AllFiles + " (*.*)", "*.*");
                context.AddFilter(Messages.YampScript + " (*.ys)", "*.ys");
                var dialog = new OpenFileWindow();
                dialog.DataContext = context;
                dialog.Title = Messages.OpenFile;
                dialog.ShowDialog();

                if (context.Accepted)
                {
                    OpenFile(context.SelectedFile.FullName);
                }
            });
        }

        #endregion

        #region Properties

        public IConsole Console
        {
            get { return _console; }
        }

        public IMathInputService Service
        {
            get { return _service; }
        }

        public ObservableCollection<EditorFileViewModel> Files
        {
            get { return _files; }
        }

        public static readonly List<AutocompleteItem> BasicItems = new List<AutocompleteItem>();

        public EditorFileViewModel SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a specific file in the editor.
        /// </summary>
        /// <param name="file">The path to the file to open.</param>
        public void OpenFile(String file)
        {
            if (Files.Count == 1 && Files[0].Text.Equals(String.Empty) && !Files[0].Changed)
            {
                Remove(Files[0]);
            }

            for (var i = 0; i != Files.Count; i++)
            {
                if (Files[i].FilePath != null && Files[i].FilePath.Equals(file, StringComparison.CurrentCultureIgnoreCase))
                {
                    return;
                }
            }

            var tab = new EditorFileViewModel(this, _kernel, file);
            Files.Add(tab);
            SelectedFile = tab;
        }

        /// <summary>
        /// Removes a certain tab form the editor.
        /// </summary>
        /// <param name="child">The child to remove.</param>
        public void Remove(EditorFileViewModel child)
        {
            Files.Remove(child);
        }

        /// <summary>
        /// Closes all tabs.
        /// </summary>
        /// <returns>True: Cancel Close. False: Do not cancel.</returns>
        public Boolean CloseAll()
        {
            var filesChanged = 0;

            for (var i = 0; i < Files.Count; i++)
            {
                if (Files[i].Changed)
                {
                    filesChanged++;
                }
            }

            if (filesChanged > 0)
            {
                var message = String.Format(Messages.EditorSaveQuestionMultiple, filesChanged);
                var result = DecisionDialog.Show(message, new[] 
                { 
                    Messages.EditorSaveAnswerYesMultiple,
                    Messages.EditorSaveAnswerNo,
                    Messages.EditorSaveAnswerCancel 
                });

                if (result == 1)
                {
                    return false;
                }
                else if (result == 2)
                {
                    return true;
                }

                for (var i = 0; i < Files.Count; i++)
                {
                    if (Files[i].Changed)
                    {
                        Files[i].Save.Execute(null);
                    }
                }

                return CloseAll();
            }

            return false;
        }

        #endregion

        #region Commands

        public ICommand Create
        {
            get { return _create; }
        }

        public ICommand Open
        {
            get { return _open; }
        }

        #endregion
    }
}
