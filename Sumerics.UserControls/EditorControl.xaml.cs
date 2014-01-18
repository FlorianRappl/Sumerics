using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using micautLib;
using System.Diagnostics;
using FastColoredTextBoxNS;
using System.Timers;

namespace Sumerics.Controls
{
    /// <summary>
    /// Interaction logic for EditorControl.xaml
    /// </summary>
    public partial class EditorControl : UserControl
    {
        #region Members

        AutocompletePopup autoComplete;
        IScriptFileModel model;
        StyleIndex styleIndex;
        ObservableCollection<ErrorRange> errorRanges;
        MathInputPanelWrapper mipw;

        #endregion

        #region Events

        public event EventHandler OnCreateNewFile;
        public event EventHandler OnOpenAnotherFile;

        #endregion

        #region ctor

        public EditorControl(IScriptFileModel model)
        {
            this.model = model;
            errorRanges = new ObservableCollection<ErrorRange>();

            InitializeComponent();
            SetupEditor();
            SetupAutocomplete();
            SetupMathInput();
            SetupSoftKeyboard();

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
            Errors.ItemsSource = errorRanges;
        }

        void SetupMathInput()
        {
            mipw = new MathInputPanelWrapper("Draw statement");

            if (!mipw.IsAvailable)
                MathInputButton.IsEnabled = false;
            else
            {
                MathInputButton.Click += (s, ev) => mipw.Open();
                mipw.OnInsertPressed += MathInputInserted;
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

            var index = Editor.AddStyle(new FastColoredTextBoxNS.WavyLineStyle(255, System.Drawing.Color.Red));
            styleIndex = Editor.GetStyleIndexMask(new FastColoredTextBoxNS.Style[] { Editor.Styles[index] });
        }

        void SetupAutocomplete()
        {
            autoComplete = new AutocompletePopup(this);
            AutocompleteButton.Click += ShowAutoComplete;
        }

        void SetupSoftKeyboard()
        {
            InputPanel.Delete = Backspace;
            InputPanel.Insert = InsertText;
            InputPanel.PlaceFocus = SetFocus;
        }

        #endregion

        #region Properties

        public IScriptFileModel Model
        {
            get { return model; }
        }

        public string Text
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

        void InputPanelButtonClicked(object sender, RoutedEventArgs e)
        {
            InputPanel.Toggle();
        }

        void MathInputInserted(object sender, string query)
        {
            Editor.InsertText(model.TransformMathML(query));
        }

        void EditorKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
                Execute();
            else if (e.KeyCode == System.Windows.Forms.Keys.S && e.Modifiers == System.Windows.Forms.Keys.Control)
                model.Save();
            else if (e.KeyCode == System.Windows.Forms.Keys.W && e.Modifiers == System.Windows.Forms.Keys.Control)
                model.Close();
            else if (e.KeyCode == System.Windows.Forms.Keys.N && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                if (OnCreateNewFile != null)
                    OnCreateNewFile(this, EventArgs.Empty);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.O && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                if (OnOpenAnotherFile != null)
                    OnOpenAnotherFile(this, EventArgs.Empty);
            }
        }

        void ShowErrorsButton_Click(object sender, RoutedEventArgs e)
        {
            if (ErrorGrid.Visibility == Visibility.Visible)
                ErrorGrid.Visibility = Visibility.Collapsed;
            else
                ErrorGrid.Visibility = Visibility.Visible;
        }

        void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            model.Close();
        }

        void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            Editor.IsChanged = false;
            model.Save();
        }

        void SaveAsButtonClick(object sender, RoutedEventArgs e)
        {
            Editor.IsChanged = false;
            model.SaveAs();
        }

        void ContentChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            model.Changed = Editor.IsChanged;
            model.Compile();
        }

        void ExecuteButtonClicked(object sender, RoutedEventArgs e)
        {
            Execute();
        }

        void PasteButtonClicked(object sender, RoutedEventArgs e)
        {
            Editor.Paste();
        }

        void CopyButtonClicked(object sender, RoutedEventArgs e)
        {
            Editor.Copy();
        }

        void RedoButtonClicked(object sender, RoutedEventArgs e)
        {
            Editor.Redo();
        }

        void UndoButtonClicked(object sender, RoutedEventArgs e)
        {
            Editor.Undo();
        }

        void UndoRedoStateChanged(object sender, EventArgs e)
        {
            UndoButton.IsEnabled = Editor.UndoEnabled;
            RedoButton.IsEnabled = Editor.RedoEnabled;
        }

        void ShowAutoComplete(object sender, RoutedEventArgs e)
        {
            autoComplete.Show(true);
            Editor.Focus();
        }

        void SelectionChanged(object sender, EventArgs e)
        {
            Line.Text = c2s(Editor.Selection.Start.iLine);
            Column.Text = c2s(Editor.Selection.Start.iChar);
        }

        string c2s(int p)
        {
            return (p + 1).ToString();
        }

        #endregion

        #region Methods

        void InsertText(string obj)
        {
            Editor.InsertText(obj);
        }

        void Backspace()
        {
            Editor.ResetSelectionToPrompt();

            if (Editor.Selection.IsEmpty)
                Editor.Selection.GoLeft(true);

            Editor.ClearSelected();
        }

        public void SetFocus()
        {
            Host.Focus();
            Editor.Focus();
        }

        void Execute()
        {
            model.Compile();

            if (errorRanges.Count == 0)
                model.Execute();
            else
                ErrorGrid.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region Handle Errors

        void ShowToolTip(object sender, ToolTipNeededEventArgs e)
        {
            for(var i = 0; i < errorRanges.Count; i++)
            {
                if (errorRanges[i].Range.Contains(e.Place))
                {
                    e.ToolTipTitle = "Compilation error";
                    e.ToolTipText = errorRanges[i].Message;
                    e.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
                    break;
                }
            }
        }

        public void SetError(int line, int col, int length, string message)
        {
            var from = Editor.PlaceToPosition(new Place(col - 1, line - 1));
            var range = Editor.GetRange(from, from + length);

            if (length > 0 && range.Text.Length == 0)
                range = Editor.GetRange(Math.Max(from - length, 0), from);

            if (range.Text.Length > 0)
            {
                range.SetStyle(styleIndex);

                errorRanges.Add(new ErrorRange
                {
                    Message = message,
                    Range = range,
                    Column = col,
                    Line = line
                });

                ErrorCount.Text = errorRanges.Count.ToString();
            }
        }

        public void ClearErrors()
        {
            var r = new Range(Editor);
            r.SelectAll();
            r.ClearStyle(styleIndex);
            errorRanges.Clear();
            ErrorCount.Text = "0";
        }

        public void Refresh()
        {
            Editor.Refresh();
        }

        class ErrorRange
        {
            public Range Range { get; set; }

            public string Message { get; set; }

            public int Column { get; set; }

            public int Line { get; set; }
        }

        #endregion
    }
}
