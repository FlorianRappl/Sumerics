namespace Sumerics.ViewModels
{
    using Sumerics.Controls;
    using Sumerics.Resources;
    using Sumerics.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using YAMP;

    /// <summary>
    /// ViewModel for one tab in the editor, i.e. one file.
    /// </summary>
    public sealed class EditorFileViewModel : BaseViewModel
    {
        #region Fields

        readonly ParseContext _debugContext;
        readonly EditorViewModel _parent;
        readonly List<AutocompleteItem> _variableItems;
        readonly ObservableCollection<AutocompleteItem> _items;
        readonly Kernel _kernel;

        String _path;
        Boolean _awaiting;
        Boolean _changed;
        EditorControl _ed;
        String _originalText;
        QueryResultViewModel _currentExecution;

        #endregion

        #region ctor

        public EditorFileViewModel(EditorViewModel parent, Kernel kernel)
        {
            _kernel = kernel;
            kernel.RunningQueriesChanged += OnRunningQueriesChanged;
            _debugContext = new ParseContext(kernel.Parser.Context);

            _parent = parent;
            _items = new ObservableCollection<AutocompleteItem>();
            _variableItems = new List<AutocompleteItem>();

            foreach (var item in EditorViewModel.BasicItems)
            {
                Items.Add(item);
            }

            Control = new EditorControl 
            { 
                DataContext = this,
                OpenFileCommand = _parent.Open,
                NewFileCommand = _parent.Create,
                AutoCompleteItems = Items,
                SaveAsCommand = new RelayCommand(_ => SaveAs()),
                SaveCommand = new RelayCommand(_ => Save()),
                CloseCommand = new RelayCommand(_ => Close()),
                CompileCommand = new RelayCommand(_ => Compile()),
                ExecuteCommand = new RelayCommand(_ => Execute()),
                MathConverter = _parent.Service.ConvertToYamp,
            };
        }

        public EditorFileViewModel(EditorViewModel parent, Kernel kernel, String path)
            : this(parent, kernel)
        {
            _path = path;
            ReadText();
        }

        #endregion

        #region Commands

        public void Save()
        {
            if (IsSaveAs)
            {
                SaveAs();
            }
            else
            {
                SaveText();
            }
        }

        public void SaveAs()
        {
            var context = new SaveFileViewModel(Environment.CurrentDirectory);
            context.AddFilter(Messages.YampScript + " (*.ys)", "*.ys");
            context.AddFilter(Messages.Textfile + " (*.txt)", "*.txt");
            var dialog = new SaveFileWindow
            {
                DataContext = context
            };
            dialog.ShowDialog();

            if (context.Accepted)
            {
                SaveText(context.SelectedFile.FullName);
            }
        }

        public void Close()
        {
            if (Changed)
            {
                var result = DecisionDialog.Show(Messages.EditorSaveQuestion, new[] 
                {
                    Messages.EditorSaveAnswerYes, 
                    Messages.EditorSaveAnswerNo, 
                    Messages.EditorSaveAnswerCancel 
                });

                if (result == 2)
                {
                    return;
                }
                else if (result == 0)
                {
                    Save();

                    //Application apparently crashed, i.e. could not write file
                    if (Changed)
                    {
                        Close();
                        return;
                    }
                }

                Changed = false;
            }

            _kernel.RunningQueriesChanged -= OnRunningQueriesChanged;
            _parent.Remove(this);
        }

        public void Compile()
        {
            Clean();
            var p = _kernel.Parser.Parse(Text.Replace("\r\n", "\n"));

            if (p.Parser.HasErrors)
            {
                foreach (var error in p.Parser.Errors)
                {
                    _ed.SetError(error.Line, error.Column, error.Length, error.Message);
                }

                _ed.Refresh();
            }

            AddVariableSymbols(p.Parser.CollectedSymbols);
        }

        public void Execute()
        {
            if (_currentExecution == null || _currentExecution.Running)
            {
                _awaiting = true;
                var message = String.Format(Messages.EvaluateFile, FileName);
                _parent.Console.Execute(Text, message);
                _awaiting = false;
            }
        }

        #endregion

        #region Events

        void OnRunningQueriesChanged(Object sender, EventArgs e)
        {
            if (_awaiting)
            {
                _currentExecution = sender as QueryResultViewModel;
            }
            else if (_currentExecution == sender)
            {
                _currentExecution = null;
            }
        }

        public void Clean()
        {
            _ed.ClearErrors();

            for (var i = 0; i < VariableItems.Count; i++)
            {
                Items.Remove(VariableItems[i]);
            }

            VariableItems.Clear();
        }

        void AddVariableSymbols(String[] symbols)
        {
            for (var i = 0; i < symbols.Length; i++)
            {
                var message = String.Format(Messages.LocalVariable, symbols[i]);
                var item = new AutocompleteItem(symbols[i], message, IconFactory.VariableIcon);
                Items.Add(item);
                VariableItems.Add(item);
            }
        }

        #endregion

        #region Properties

        public List<AutocompleteItem> VariableItems
        {
            get { return _variableItems; }
        }

        public ObservableCollection<AutocompleteItem> Items
        {
            get { return _items; }
        }

        public EditorControl Control
        {
            get { return _ed; }
            set
            {
                _ed = value;
                RaisePropertyChanged();
            }
        }

        public Boolean IsSaveAs
        {
            get { return String.IsNullOrEmpty(_path); }
        }

        public String FilePath
        {
            get { return _path; }
        }

        public Boolean Changed
        {
            get { return _changed; }
            set
            {
                if (value && _originalText == _ed.Text)
                {
                    value = false;
                }

                _changed = value;
                RaisePropertyChanged();
            }
        }

        public String Text
        {
            get { return _ed.Text; }
            set { _ed.Text = value; _originalText = value; }
        }

        public String FileName
        {
            get 
            {
                if (IsSaveAs)
                {
                    return Messages.EditorUntitledFile;
                }

                return Path.GetFileName(_path); 
            }
        }

        #endregion

        #region Methods

        void ReadText()
        {
            try
            {
                Text = File.ReadAllText(_path);
                Changed = false;
            }
            catch (Exception ex)
            {
                ShowOutputDialog(Messages.ErrorCannotOpenFile, ex.Message);
            }
        }

        void SaveText()
        {
            try
            {
                File.WriteAllText(_path, Text);
                Changed = false;
            }
            catch (Exception ex)
            {
                ShowOutputDialog(Messages.ErrorCannotSaveFile, ex.Message);
            }
        }

        void SaveText(String path)
        {
            try
            {
                File.WriteAllText(path, Text);
                Changed = false;
                _path = path;
                RaisePropertyChanged("FilePath");
                RaisePropertyChanged("FileName");
            }
            catch (Exception ex)
            {
                ShowOutputDialog(Messages.ErrorCannotSaveFile, ex.Message);
            }
        }

        static void ShowOutputDialog(String title, String message)
        {
            var output = new OutputViewModel 
            { 
                Message = message, 
                Title = title 
            };
            output.ShowWindow();
        }

        #endregion
    }
}
