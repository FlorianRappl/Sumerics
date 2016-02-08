namespace Sumerics
{
    using Sumerics.Controls;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using YAMP;

    /// <summary>
    /// ViewModel for one tab in the editor, i.e. one file.
    /// </summary>
    sealed class EditorFileViewModel : BaseViewModel, IScriptFileModel
    {
        #region Fields

        String path;
        Boolean awaiting;
        Boolean changed;
        EditorViewModel parent;
        EditorControl ed;
        String originalText;
        QueryResultViewModel currentExecution;

        ParseContext debugContext;

        #endregion

        #region ctor

        public EditorFileViewModel(EditorViewModel parent, IContainer container)
            : base(container)
        {
            QueryResultViewModel.RunningQueriesChanged += OnRunningQueriesChanged;
            debugContext = new ParseContext(Core.Context);

            this.parent = parent;
            Items = new ObservableCollection<AutocompleteItem>();
            VariableItems = new List<AutocompleteItem>();

            foreach (var item in EditorViewModel.BasicItems)
            {
                Items.Add(item);
            }

            InitEditor();
        }

        public EditorFileViewModel(EditorViewModel parent, String path, IContainer container)
            : this(parent, container)
        {
            this.path = path;
            ReadText();
        }

        void InitEditor()
        {
            var ed = new EditorControl(this);
            ed.OnCreateNewFile += (s, e) => parent.Create.Execute(null);
            ed.OnOpenAnotherFile += (s, e) => parent.Open.Execute(null);
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
            var dialog = new SaveFileWindow(Container);
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
            parent.Remove(this);
        }

        public void Compile()
        {
            Clean();
            var p = Core.Parser.Parse(Text.Replace("\r\n", "\n"));

            if (p.Parser.HasErrors)
            {
                foreach (var error in p.Parser.Errors)
                {
                    ed.SetError(error.Line, error.Column, error.Length, error.Message);
                }

                ed.Refresh();
            }

            AddVariableSymbols(p.Parser.CollectedSymbols);
        }

        public void Execute()
        {
            if (currentExecution == null || currentExecution.Running)
            {
                awaiting = true;
                App.Window.RunQuery(Text, "Evaluating " + FileName);
                awaiting = false;
            }
        }

        #endregion

        #region Events

        void OnRunningQueriesChanged(object sender, EventArgs e)
        {
            if (awaiting)
                currentExecution = sender as QueryResultViewModel;
            else if (currentExecution == sender)
                currentExecution = null;
        }

        public void Clean()
        {
            ed.ClearErrors();

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
            get;
            set;
        }

        public ObservableCollection<AutocompleteItem> Items
        {
            get;
            private set;
        }

        public EditorControl Control
        {
            get
            {
                return ed;
            }
            set
            {
                ed = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSaveAs
        {
            get { return string.IsNullOrEmpty(path); }
        }

        public string FilePath
        {
            get { return path; }
        }

        public bool Changed
        {
            get { return changed; }
            set
            {
                if (value)
                {
                    if (originalText == ed.Text)
                        value = false;
                }

                changed = value;
                RaisePropertyChanged();
            }
        }

        public string Text
        {
            get { return ed.Text; }
            set { ed.Text = value; originalText = value; }
        }

        public string FileName
        {
            get 
            {
                if (IsSaveAs)
                    return "untitled";

                return Path.GetFileName(path); 
            }
        }

        #endregion

        #region Methods

        public string TransformMathML(string query)
        {
            return MathMLParser.Parse(query);
        }

        void ReadText()
        {
            try
            {
                Text = File.ReadAllText(path);
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
                File.WriteAllText(path, Text);
                Changed = false;
            }
            catch (Exception ex)
            {
                OutputDialog.Show("Cannot save file", ex.Message);
            }
        }

        void SaveText(string path)
        {
            try
            {
                File.WriteAllText(path, Text);
                Changed = false;
                this.path = path;
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
