namespace Sumerics.Controls
{
    using FastColoredTextBoxNS;
    using Sumerics.Resources;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for EditorControl.xaml
    /// </summary>
    public partial class EditorControl : UserControl
    {
        #region Fields

        readonly AutocompletePopup _autoComplete;
        readonly StyleIndex _styleIndex;
        readonly ObservableCollection<ErrorRange> _errorRanges;
        readonly MathInputPanelWrapper _mipw;

        #endregion

        #region ctor

        public EditorControl()
        {
            InitializeComponent();

            var index = Editor.AddStyle(new WavyLineStyle(255, System.Drawing.Color.Red));

            _errorRanges = new ObservableCollection<ErrorRange>();
            _mipw = new MathInputPanelWrapper(Messages.DrawExpression);
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
            Editor.TextChanged += ContentChanged;
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

        #region Dependency Properties

        public ObservableCollection<AutocompleteItem> AutoCompleteItems
        {
            get { return (ObservableCollection<AutocompleteItem>)GetValue(AutoCompleteItemsProperty); }
            set { SetValue(AutoCompleteItemsProperty, value); }
        }

        public static readonly DependencyProperty AutoCompleteItemsProperty = DependencyProperty.Register(
            "AutoCompleteItems",
            typeof(ObservableCollection<AutocompleteItem>),
            typeof(EditorControl),
            new FrameworkPropertyMetadata(new ObservableCollection<AutocompleteItem>(), OnAutoCompleteItemsChange));

        public Func<String, String> MathConverter
        {
            get { return (Func<String, String>)GetValue(MathConverterProperty); }
            set { SetValue(MathConverterProperty, value); }
        }

        public static readonly DependencyProperty MathConverterProperty =
            DependencyProperty.Register("MathConverter", typeof(Func<String, String>), typeof(EditorControl), new PropertyMetadata(null));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(EditorControl), new PropertyMetadata(null));


        public Object CloseCommandParameter
        {
            get { return (Object)GetValue(CloseCommandParameterProperty); }
            set { SetValue(CloseCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CloseCommandParameterProperty =
            DependencyProperty.Register("CloseCommandParameter", typeof(Object), typeof(EditorControl), new PropertyMetadata(null));

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(EditorControl), new PropertyMetadata(null));


        public Object SaveCommandParameter
        {
            get { return (Object)GetValue(SaveCommandParameterProperty); }
            set { SetValue(SaveCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandParameterProperty =
            DependencyProperty.Register("SaveCommandParameter", typeof(Object), typeof(EditorControl), new PropertyMetadata(null));

        public ICommand SaveAsCommand
        {
            get { return (ICommand)GetValue(SaveAsCommandProperty); }
            set { SetValue(SaveAsCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveAsCommandProperty =
            DependencyProperty.Register("SaveAsCommand", typeof(ICommand), typeof(EditorControl), new PropertyMetadata(null));


        public Object SaveAsCommandParameter
        {
            get { return (Object)GetValue(SaveAsCommandParameterProperty); }
            set { SetValue(SaveAsCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty SaveAsCommandParameterProperty =
            DependencyProperty.Register("SaveAsCommandParameter", typeof(Object), typeof(EditorControl), new PropertyMetadata(null));

        public Boolean Changed
        {
            get { return (Boolean)GetValue(ChangedProperty); }
            set { SetValue(ChangedProperty, value); }
        }

        public static readonly DependencyProperty ChangedProperty =
            DependencyProperty.Register("Changed", typeof(Boolean), typeof(EditorControl), new PropertyMetadata(false));

        public ICommand CompileCommand
        {
            get { return (ICommand)GetValue(CompileCommandProperty); }
            set { SetValue(CompileCommandProperty, value); }
        }

        public static readonly DependencyProperty CompileCommandProperty =
            DependencyProperty.Register("CompileCommand", typeof(ICommand), typeof(EditorControl), new PropertyMetadata(null));


        public Object CompileCommandParameter
        {
            get { return (Object)GetValue(CompileCommandParameterProperty); }
            set { SetValue(CompileCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CompileCommandParameterProperty =
            DependencyProperty.Register("CompileCommandParameter", typeof(Object), typeof(EditorControl), new PropertyMetadata(null));

        public ICommand ExecuteCommand
        {
            get { return (ICommand)GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }

        public static readonly DependencyProperty ExecuteCommandProperty =
            DependencyProperty.Register("ExecuteCommand", typeof(ICommand), typeof(EditorControl), new PropertyMetadata(null));

        public Object ExecuteCommandParameter
        {
            get { return (Object)GetValue(ExecuteCommandParameterProperty); }
            set { SetValue(ExecuteCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty ExecuteCommandParameterProperty =
            DependencyProperty.Register("ExecuteCommandParameter", typeof(Object), typeof(EditorControl), new PropertyMetadata(null));

        public ICommand NewFileCommand
        {
            get { return (ICommand)GetValue(NewFileCommandProperty); }
            set { SetValue(NewFileCommandProperty, value); }
        }

        public static readonly DependencyProperty NewFileCommandProperty =
            DependencyProperty.Register("NewFileCommand", typeof(ICommand), typeof(EditorControl), new PropertyMetadata(null));

        public Object NewFileCommandParameter
        {
            get { return (Object)GetValue(NewFileCommandParameterProperty); }
            set { SetValue(NewFileCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty NewFileCommandParameterProperty =
            DependencyProperty.Register("NewFileCommandParameter", typeof(Object), typeof(EditorControl), new PropertyMetadata(null));

        public ICommand OpenFileCommand
        {
            get { return (ICommand)GetValue(OpenFileCommandProperty); }
            set { SetValue(OpenFileCommandProperty, value); }
        }

        public static readonly DependencyProperty OpenFileCommandProperty =
            DependencyProperty.Register("OpenFileCommand", typeof(ICommand), typeof(EditorControl), new PropertyMetadata(null));

        public Object OpenFileCommandParameter
        {
            get { return (Object)GetValue(OpenFileCommandParameterProperty); }
            set { SetValue(OpenFileCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty OpenFileCommandParameterProperty =
            DependencyProperty.Register("OpenFileCommandParameter", typeof(Object), typeof(EditorControl), new PropertyMetadata(null));

        public Boolean IsActive
        {
            get { return (Boolean)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(Boolean), typeof(EditorControl), new PropertyMetadata(OnActivated));

        static async void OnActivated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as EditorControl;
            var value = (Boolean)e.NewValue;

            if (ctrl != null && value)
            {
                await Task.Delay(100);
                ctrl.SetFocus();
            }
        }

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(String), typeof(EditorControl), new PropertyMetadata(""));
        
        #endregion

        #region Events

        static void OnAutoCompleteItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var basis = (EditorControl)d;
            var newList = (ObservableCollection<AutocompleteItem>)e.NewValue;
            var oldList = basis._autoComplete.AvailableItems;

            oldList.Clear();

            foreach (var entry in newList)
            {
                oldList.Add(entry);
            }
        }

        void InputPanelButtonClicked(Object sender, RoutedEventArgs e)
        {
            InputPanel.Toggle();
        }

        void MathInputInserted(Object sender, String query)
        {
            var converter = MathConverter;

            if (converter != null)
            {
                var text = converter(query);
                Editor.InsertText(text);
            }
        }

        void EditorKeyDown(Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                Execute();
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.S && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                SaveButtonClicked(sender, null);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.W && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                CloseButtonClicked(sender, null);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.N && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                var command = NewFileCommand;
                var parameter = NewFileCommandParameter ?? this;

                if (command != null && command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.O && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                var command = OpenFileCommand;
                var parameter = OpenFileCommandParameter ?? this;

                if (command != null && command.CanExecute(parameter))
                {
                    command.Execute(parameter);
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
            var command = CloseCommand;
            var parameter = CloseCommandParameter ?? this;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        void SaveButtonClicked(Object sender, RoutedEventArgs e)
        {
            var command = SaveCommand;
            var parameter = SaveCommandParameter ?? this;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        void SaveAsButtonClick(Object sender, RoutedEventArgs e)
        {
            var command = SaveAsCommand;
            var parameter = SaveAsCommandParameter ?? this;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        void ContentChanged(Object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            Changed = Editor.IsChanged;
            Text = Editor.Text;
        }

        void ContentChangedDelayed(Object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            var command = CompileCommand;
            var parameter = CompileCommandParameter ?? this;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
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

        void SetFocus()
        {
            Host.Focus();
            Editor.Focus();
        }

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

        void Execute()
        {
            var command = ExecuteCommand;
            var parameter = ExecuteCommandParameter ?? this;

            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }

            if (_errorRanges.Count != 0)
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
                    e.ToolTipTitle = Messages.CompilationError;
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

        sealed class ErrorRange
        {
            public Range Range { get; set; }

            public String Message { get; set; }

            public Int32 Column { get; set; }

            public Int32 Line { get; set; }
        }

        #endregion
    }
}
