namespace Sumerics.ViewModels
{
    using Sumerics.Controls;
    using Sumerics.Resources;
    using Sumerics.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
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
        readonly RelayCommand _close;
        readonly RelayCommand _saveAs;
        readonly RelayCommand _save;
        readonly RelayCommand _compile;
        readonly RelayCommand _execute;

        String _text;
        String _path;
        Boolean _selected;
        Boolean _awaiting;
        Boolean _changed;
        QueryResultViewModel _currentExecution;

        #endregion

        #region ctor

        public EditorFileViewModel(EditorViewModel parent, Kernel kernel)
        {
            _kernel = kernel;
            kernel.RunningQueriesChanged += OnRunningQueriesChanged;
            _debugContext = new ParseContext(_kernel.Parser.Context);

            _parent = parent;
            _items = new ObservableCollection<AutocompleteItem>();
            _variableItems = new List<AutocompleteItem>();

            foreach (var item in EditorViewModel.BasicItems)
            {
                Items.Add(item);
            }

            _close = new RelayCommand(_ => CloseEditor());
            _saveAs = new RelayCommand(_ => SaveCurrentAs());
            _save = new RelayCommand(_ => SaveCurrent());
            _compile = new RelayCommand(obj => CompileSource(obj as EditorControl));
            _execute = new RelayCommand(_ => ExecuteFile());
        }

        public EditorFileViewModel(EditorViewModel parent, Kernel kernel, String path)
            : this(parent, kernel)
        {
            _path = path;
            _text = ReadText(path);
        }

        #endregion

        #region Properties

        public Func<String, String> Converter
        {
            get { return _parent.Service.ConvertToYamp; }
        }

        public ICommand Close
        {
            get { return _close; }
        }

        public ICommand Save
        {
            get { return _save; }
        }

        public ICommand SaveAs
        {
            get { return _saveAs; }
        }

        public ICommand Compile
        {
            get { return _compile; }
        }

        public ICommand Execute
        {
            get { return _execute; }
        }

        public List<AutocompleteItem> VariableItems
        {
            get { return _variableItems; }
        }

        public ObservableCollection<AutocompleteItem> Items
        {
            get { return _items; }
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
            set { _changed = value; RaisePropertyChanged(); }
        }

        public Boolean IsSelected
        {
            get { return _selected; }
            set { _selected = value; RaisePropertyChanged(); }
        }

        public String Text
        {
            get { return _text ?? String.Empty; }
            set { _text = value; }
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

        void SaveCurrent()
        {
            if (IsSaveAs)
            {
                SaveCurrentAs();
            }
            else
            {
                SaveText();
            }
        }

        void SaveCurrentAs()
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

        void CloseEditor()
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
                    SaveCurrent();

                    //Application apparently crashed, i.e. could not write file
                    if (Changed)
                    {
                        CloseEditor();
                        return;
                    }
                }
            }

            _kernel.RunningQueriesChanged -= OnRunningQueriesChanged;
            _parent.Remove(this);
        }

        void CompileSource(EditorControl ed)
        {
            var p = _kernel.Parser.Parse(Text.Replace("\r\n", "\n"));

            if (ed != null)
            {
                Clean(ed);

                if (p.Parser.HasErrors)
                {
                    foreach (var error in p.Parser.Errors)
                    {
                        ed.SetError(error.Line, error.Column, error.Length, error.Message);
                    }

                    ed.Refresh();
                }
            }

            AddVariableSymbols(p.Parser.CollectedSymbols);
        }

        void ExecuteFile()
        {
            if (_currentExecution == null || !_currentExecution.Running)
            {
                _awaiting = true;
                var message = String.Format(Messages.EvaluateFile, FileName);
                _parent.Console.Execute(Text, message);
                _awaiting = false;
            }
        }

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

        void Clean(EditorControl ed)
        {
            ed.ClearErrors();

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

        static String ReadText(String path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                ShowOutputDialog(Messages.ErrorCannotOpenFile, ex.Message);
            }

            return String.Empty;
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
