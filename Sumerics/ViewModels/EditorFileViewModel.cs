namespace Sumerics.ViewModels
{
    using Sumerics.Controls;
    using Sumerics.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using YAMP;

    /// <summary>
    /// ViewModel for one tab in the editor, i.e. one file.
    /// </summary>
    public sealed class EditorFileViewModel : BaseViewModel, IScriptFileModel
    {
        #region Fields

        readonly Parser _parser;
        readonly ParseContext _debugContext;
        readonly EditorViewModel _parent;
        readonly List<AutocompleteItem> _variableItems;
        readonly ObservableCollection<AutocompleteItem> _items;

        String _path;
        Boolean _awaiting;
        Boolean _changed;
        EditorControl _ed;
        String _originalText;
        QueryResultViewModel _currentExecution;

        #endregion

        #region ctor

        public EditorFileViewModel(EditorViewModel parent, Parser parser)
        {
            _parser = parser;
            QueryResultViewModel.RunningQueriesChanged += OnRunningQueriesChanged;
            _debugContext = new ParseContext(_parser.Context);

            _parent = parent;
            _items = new ObservableCollection<AutocompleteItem>();
            _variableItems = new List<AutocompleteItem>();

            foreach (var item in EditorViewModel.BasicItems)
            {
                Items.Add(item);
            }

            InitEditor();
        }

        public EditorFileViewModel(EditorViewModel parent, Parser parser, String path)
            : this(parent, parser)
        {
            _path = path;
            ReadText();
        }

        void InitEditor()
        {
            var ed = new EditorControl(this);
            ed.OnCreateNewFile += (s, e) => _parent.Create.Execute(null);
            ed.OnOpenAnotherFile += (s, e) => _parent.Open.Execute(null);
            Control = ed;
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
            var vm = new SaveFileViewModel(Environment.CurrentDirectory);
            var dialog = new SaveFileWindow(vm);
            dialog.AddFilter("YAMP Script (*.ys)", "*.ys");
            dialog.AddFilter("Textfile (*.txt)", "*.txt");
            dialog.ShowDialog();

            if (dialog.Accepted)
            {
                SaveText(dialog.SelectedFile);
            }
        }

        public void Close()
        {
            if (Changed)
            {
                var result = DecisionDialog.Show("The content has been changed. Save the changes?",
                    new[] { "Yes, save the changes.", "No, but thanks!", "Cancel closing." });

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

            QueryResultViewModel.RunningQueriesChanged -= OnRunningQueriesChanged;
            _parent.Remove(this);
        }

        public void Compile()
        {
            Clean();
            var p = _parser.Parse(Text.Replace("\r\n", "\n"));

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
                _parent.Console.Execute(Text, "Evaluating " + FileName);
                _awaiting = false;
            }
        }

        #endregion

        #region Events

        void OnRunningQueriesChanged(object sender, EventArgs e)
        {
            if (_awaiting)
                _currentExecution = sender as QueryResultViewModel;
            else if (_currentExecution == sender)
                _currentExecution = null;
        }

        public void Clean()
        {
            _ed.ClearErrors();

            for (var i = 0; i < VariableItems.Count; i++)
                Items.Remove(VariableItems[i]);

            VariableItems.Clear();
        }

        void AddVariableSymbols(string[] symbols)
        {
            for (var i = 0; i < symbols.Length; i++)
            {
                var item = new AutocompleteItem(symbols[i], "Local variable " + symbols[i] + ".", Icons.VariableIcon);
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
                    return "untitled";
                }

                return Path.GetFileName(_path); 
            }
        }

        #endregion

        #region Methods

        public String TransformMathML(String query)
        {
            return _parent.Service.ConvertToYamp(query);
        }

        void ReadText()
        {
            try
            {
                Text = File.ReadAllText(_path);
                Changed = false;
            }
            catch (Exception ex)
            {
                OutputDialog.Show("Cannot open file", ex.Message);
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
                OutputDialog.Show("Cannot save file", ex.Message);
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
                OutputDialog.Show("Cannot save file", ex.Message);
            }
        }

        #endregion
    }
}
