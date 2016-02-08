namespace Sumerics
{
    using Sumerics.Controls;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// ViewModel for the editor in general.
    /// </summary>
    sealed class EditorViewModel : BaseViewModel
    {
        #region Fields

        EditorFileViewModel selectedFile;

        #endregion

        #region ctor

        public EditorViewModel(IContainer container)
            : base(container)
        {
            Files = new ObservableCollection<EditorFileViewModel>();
            Files.Add(new EditorFileViewModel(this, container));
        }

        #endregion

        #region Properties

        public ObservableCollection<EditorFileViewModel> Files
        {
            get;
            set;
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

            var tab = new EditorFileViewModel(this, file, Container);
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
                var result = DecisionDialog.Show(
                    String.Format("You have {0} unsaved file(s). Do you want to save them?", filesChanged),
                    new[] { "Yes, I want to save them.", "No, but thanks!", "Cancel closing." });

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
                        Files[i].Save();
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
            get
            {
                return new RelayCommand(x =>
                {
                    var newfile = new EditorFileViewModel(this, Container);
                    Files.Add(newfile);
                    SelectedFile = newfile;
                });
            }
        }

        public ICommand Open
        {
            get
            {
                return new RelayCommand(x =>
                {
                    var dialog = new OpenFileWindow(Container);
                    dialog.Title = "Open file ...";
                    dialog.AddFilter("All files (*.*)", "*.*");
                    dialog.AddFilter("YAMP Script (*.ys)", "*.ys");
                    dialog.ShowDialog();

                    if (dialog.Accepted)
                    {
                        OpenFile(dialog.SelectedFile);
                    }
                });
            }
        }

        #endregion
    }
}
