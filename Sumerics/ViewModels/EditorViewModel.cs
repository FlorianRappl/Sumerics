using Sumerics.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sumerics
{
    /// <summary>
    /// ViewModel for the editor in general.
    /// </summary>
    class EditorViewModel : BaseViewModel
    {
        #region Members

        EditorFileViewModel selectedFile;

        #endregion

        #region ctor

        static EditorViewModel()
        {
            BasicItems = new List<AutocompleteItem>();
        }

        public EditorViewModel()
        {
            Files = new ObservableCollection<EditorFileViewModel>();
            Files.Add(new EditorFileViewModel(this));
        }

        #endregion

        #region Properties

        public ObservableCollection<EditorFileViewModel> Files
        {
            get;
            set;
        }

        public static List<AutocompleteItem> BasicItems
        {
            get;
            private set;
        }

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
        public void OpenFile(string file)
        {
            if (Files.Count == 1 && Files[0].Text.Equals(string.Empty) && !Files[0].Changed)
                Remove(Files[0]);

            for(var i = 0; i != Files.Count; i++)
                if (Files[i].FilePath != null && Files[i].FilePath.Equals(file, StringComparison.CurrentCultureIgnoreCase))
                    return;

            var tab = new EditorFileViewModel(this, file);
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
        public bool CloseAll()
        {
            int filesChanged = 0;

            for (var i = 0; i < Files.Count; i++)
            {
                if (Files[i].Changed)
                    filesChanged++;
            }

            if (filesChanged > 0)
            {
                var result = DecisionDialog.Show(
                    string.Format("You have {0} unsaved file(s). Do you want to save them?", filesChanged),
                    new[] { "Yes, I want to save them.", "No, but thanks!", "Cancel closing." });

                if (result == 1)
                    return false;
                else if (result == 2)
                    return true;

                for (var i = 0; i < Files.Count; i++)
                {
                    if (Files[i].Changed)
                        Files[i].Save();
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
                    var newfile = new EditorFileViewModel(this);
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
                    var dialog = new OpenFileWindow();
                    dialog.Title = "Open file ...";
                    dialog.AddFilter("All files (*.*)", "*.*");
                    dialog.AddFilter("YAMP Script (*.ys)", "*.ys");
                    dialog.ShowDialog();

                    if (dialog.Accepted)
                        OpenFile(dialog.SelectedFile);
                });
            }
        }

        #endregion
    }
}
