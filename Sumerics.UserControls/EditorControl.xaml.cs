namespace Sumerics.Controls
{
    using FastColoredTextBoxNS;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for EditorControl.xaml
    /// </summary>
    public partial class EditorControl : UserControl
    {
        #region Fields

        readonly AutocompletePopup _autoComplete;
        readonly IScriptFileModel _model;
        readonly StyleIndex _styleIndex;
        readonly ObservableCollection<ErrorRange> _errorRanges;
        readonly MathInputPanelWrapper _mipw;

        #endregion

        #region Events

        public event EventHandler OnCreateNewFile;
        public event EventHandler OnOpenAnotherFile;

        #endregion

        #region ctor

        public EditorControl(IScriptFileModel model)
        {
            InitializeComponent();

            var index = Editor.AddStyle(new WavyLineStyle(255, System.Drawing.Color.Red));

            _model = model;
            _errorRanges = new ObservableCollection<ErrorRange>();
            _mipw = new MathInputPanelWrapper("Draw statement");
            _autoComplete = new AutocompletePopup(this);
            _styleIndex = Editor.GetStyleIndexMask(new FastColoredTextBoxNS.Style[] { Editor.Styles[index] });

            SetupEditor();
            SetupMathInput();
            SetupSoftKeyboard();
            SetupHandlers();
        }

        void SetupMathInput()
        {
            if (!_mipw.IsAvailable)
            {
                MathInputButton.IsEnabled = false;
            }
            else
            {
                MathInputButton.Click += (s, ev) => _mipw.Open();
                _mipw.OnInsertPressed += MathInputInserted;
            }
        }

        void SetupEditor()
        {
            Editor.HighlightingRangeType = HighlightingRangeType.VisibleRange;
            Editor.DelayedTextChangedInterval = 1000;
            Editor.ShowLineNumbers = true;
            Editor.ConsoleMode = false;
            Editor.SyntaxHighlighter.AutoFoldBlocks = true;
            Editor.AutoIndent = true;
            Editor.Reset();

            Editor.UndoRedoStateChanged += UndoRedoStateChanged;
            Editor.SelectionChanged += SelectionChanged;
            Editor.TextChangedDelayed += ContentChangedDelayed;

            Editor.ToolTipNeeded += ShowToolTip;
            Editor.KeyDown += EditorKeyDown;

            AutocompleteButton.Click += ShowAutoComplete;
        }

        void SetupSoftKeyboard()
        {
            InputPanel.Delete = Backspace;
            InputPanel.Insert = InsertText;
            InputPanel.PlaceFocus = SetFocus;
        }

        void SetupHandlers()
        {
            CopyButton.Click += CopyButtonClicked;
            PasteButton.Click += PasteButtonClicked;
            InputPanelButton.Click += InputPanelButtonClicked;

            SaveButton.Click += SaveButtonClicked;
            SaveAsButton.Click += SaveAsButtonClick;
            CloseButton.Click += CloseButtonClicked;

            UndoButton.Click += UndoButtonClicked;
            RedoButton.Click += RedoButtonClicked;

            ExecuteButton.Click += ExecuteButtonClicked;
            ShowErrorsButton.Click += ShowErrorsButton_Click;
            Errors.ItemsSource = _errorRanges;
        }

        #endregion

        #region Properties

        public IScriptFileModel Model
        {
            get { return _model; }
        }

        public String Text
        {
            get { return Editor.Text; }
            set
            {
                Editor.Text = value;
                Editor.IsChanged = false;
            }
        }

        #endregion

        #region Events

        void InputPanelButtonClicked(Object sender, RoutedEventArgs e)
        {
            InputPanel.Toggle();
        }

        void MathInputInserted(Object sender, String query)
        {
            Editor.InsertText(_model.TransformMathML(query));
        }

        void EditorKeyDown(Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                Execute();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.S && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                _model.Save();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.W && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                _model.Close();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.N && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                if (OnCreateNewFile != null)
                {
                    OnCreateNewFile(this, EventArgs.Empty);
                }
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.O && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                if (OnOpenAnotherFile != null)
                {
                    OnOpenAnotherFile(this, EventArgs.Empty);
                }
            }
        }

        void ShowErrorsButton_Click(Object sender, RoutedEventArgs e)
        {
            if (ErrorGrid.Visibility == Visibility.Visible)
            {
                ErrorGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ErrorGrid.Visibility = Visibility.Visible;
            }
        }

        void CloseButtonClicked(Object sender, RoutedEventArgs e)
        {
            _model.Close();
        }

        void SaveButtonClicked(Object sender, RoutedEventArgs e)
        {
            Editor.IsChanged = false;
            _model.Save();
        }

        void SaveAsButtonClick(Object sender, RoutedEventArgs e)
        {
            Editor.IsChanged = false;
            _model.SaveAs();
        }

        void ContentChangedDelayed(Object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            _model.Changed = Editor.IsChanged;
            _model.Compile();
        }

        void ExecuteButtonClicked(Object sender, RoutedEventArgs e)
        {
            Execute();
        }

        void PasteButtonClicked(Object sender, RoutedEventArgs e)
        {
            Editor.Paste();
        }

        void CopyButtonClicked(Object sender, RoutedEventArgs e)
        {
            Editor.Copy();
        }

        void RedoButtonClicked(Object sender, RoutedEventArgs e)
        {
            Editor.Redo();
        }

        void UndoButtonClicked(Object sender, RoutedEventArgs e)
        {
            Editor.Undo();
        }

        void UndoRedoStateChanged(Object sender, EventArgs e)
        {
            UndoButton.IsEnabled = Editor.UndoEnabled;
            RedoButton.IsEnabled = Editor.RedoEnabled;
        }

        void ShowAutoComplete(Object sender, RoutedEventArgs e)
        {
            _autoComplete.Show(true);
            Editor.Focus();
        }

        void SelectionChanged(Object sender, EventArgs e)
        {
            Line.Text = c2s(Editor.Selection.Start.iLine);
            Column.Text = c2s(Editor.Selection.Start.iChar);
        }

        String c2s(Int32 p)
        {
            return (p + 1).ToString();
        }

        #endregion

        #region Methods

        void InsertText(String obj)
        {
            Editor.InsertText(obj);
        }

        void Backspace()
        {
            Editor.ResetSelectionToPrompt();

            if (Editor.Selection.IsEmpty)
            {
                Editor.Selection.GoLeft(true);
            }

            Editor.ClearSelected();
        }

        public void SetFocus()
        {
            Host.Focus();
            Editor.Focus();
        }

        void Execute()
        {
            _model.Compile();

            if (_errorRanges.Count == 0)
            {
                _model.Execute();
            }
            else
            {
                ErrorGrid.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #endregion

        #region Handle Errors

        void ShowToolTip(Object sender, ToolTipNeededEventArgs e)
        {
            for (var i = 0; i < _errorRanges.Count; i++)
            {
                if (_errorRanges[i].Range.Contains(e.Place))
                {
                    e.ToolTipTitle = "Compilation error";
                    e.ToolTipText = _errorRanges[i].Message;
                    e.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
                    break;
                }
            }
        }

        public void SetError(Int32 line, Int32 col, Int32 length, String message)
        {
            var from = Editor.PlaceToPosition(new Place(col - 1, line - 1));
            var range = Editor.GetRange(from, from + length);

            if (length > 0 && range.Text.Length == 0)
            {
                range = Editor.GetRange(Math.Max(from - length, 0), from);
            }

            if (range.Text.Length > 0)
            {
                range.SetStyle(_styleIndex);

                _errorRanges.Add(new ErrorRange
                {
                    Message = message,
                    Range = range,
                    Column = col,
                    Line = line
                });

                ErrorCount.Text = _errorRanges.Count.ToString();
            }
        }

        public void ClearErrors()
        {
            var r = new Range(Editor);
            r.SelectAll();
            r.ClearStyle(_styleIndex);
            _errorRanges.Clear();
            ErrorCount.Text = "0";
        }

        public void Refresh()
        {
            Editor.Refresh();
        }

        class ErrorRange
        {
            public Range Range { get; set; }

            public String Message { get; set; }

            public Int32 Column { get; set; }

            public Int32 Line { get; set; }
        }

        #endregion
    }
}
