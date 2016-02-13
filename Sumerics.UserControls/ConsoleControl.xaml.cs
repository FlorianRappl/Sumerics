namespace Sumerics.Controls
{
    using micautLib;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ConsoleControl.xaml
    /// </summary>
    public partial class ConsoleControl : UserControl
    {
        #region Fields

        ICommand _command;
        String[] _currentHistory;
		Int32 _historyIndex;
		AutocompletePopup _autoComplete;
        MathInputControl _mip;

        #endregion

        #region Event

        public event EventHandler<string> MathInputReceived;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty HasNotificationProperty = DependencyProperty.Register(
            "HasNotification", 
            typeof(bool), 
            typeof(ConsoleControl), 
            new FrameworkPropertyMetadata(false, OnNotificationChanged));      

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(ConsoleControl),
            new FrameworkPropertyMetadata(null, OnCommandChange));

        public static readonly DependencyProperty InputProperty = DependencyProperty.Register(
            "Input",
            typeof(string),
            typeof(ConsoleControl),
            new FrameworkPropertyMetadata(null, OnInputChange));

        public static readonly DependencyProperty CommandHistoryProperty = DependencyProperty.Register(
            "CommandHistory",
            typeof(ObservableCollection<string>),
            typeof(ConsoleControl),
			new FrameworkPropertyMetadata(new ObservableCollection<string>()));

		public static readonly DependencyProperty AutoCompleteItemsProperty = DependencyProperty.Register(
			"AutoCompleteItems", 
			typeof(ObservableCollection<AutocompleteItem>), 
			typeof(ConsoleControl),
			new FrameworkPropertyMetadata(new ObservableCollection<AutocompleteItem>(), OnAutoCompleteItemsChange));

        public static readonly DependencyProperty OpenEditorCommandProperty =  DependencyProperty.Register(
            "OpenEditorCommand", 
            typeof(ICommand), 
            typeof(ConsoleControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty OpenEditorCommandParameterProperty = DependencyProperty.Register(
            "OpenEditorCommandParameter",
            typeof(Object),
            typeof(ConsoleControl),
            new PropertyMetadata(null));

        static void OnNotificationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as ConsoleControl);
            control.Notification.IsOpen = (bool)e.NewValue;
        }  

        static void OnCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as ConsoleControl);
            var value = e.NewValue as ICommand;
            control._command = value;
        }

        static void OnInputChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ConsoleControl;
            var value = e.NewValue as string;
			control.Console.Query = value;
        }

        static void OnAutoCompleteItemsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var basis = (ConsoleControl)d;
            var newList = (ObservableCollection<AutocompleteItem>)e.NewValue;
            var oldList = basis._autoComplete.AvailableItems;
            
            oldList.Clear();

            foreach (var entry in newList)
            {
                oldList.Add(entry);
            }
        }		

        #endregion

        #region ctor

        public ConsoleControl()
        {
            InitializeComponent();
            InputPanel.PlaceFocus = SetFocus;
            InputPanel.Insert = InsertText;
            InputPanel.Delete = Backspace;
            _currentHistory = new string[0];
			CommandHistory.Add(string.Empty);

			Console.OnQueryEntered += OnQueryEntered;
			Console.OnHistoryDown += OnHistoryDown;
			Console.OnHistoryUp += OnHistoryUp;

			var binding = new Binding("Count");
			binding.Source = CommandHistory;
			binding.Converter = new CountConverter();
			HistoryButton.SetBinding(Button.IsEnabledProperty, binding);

            EvaluateButton.Click += EvaluateClick;
			AutocompleteButton.Click += AutocompleteClick;
            HistoryButton.Click += HistoryClick;
            CommandHistoryList.ItemsSource = CommandHistory;
            InputPanelButton.Click += InputPanelClick;
            EditorButton.Click += EditorClick;
            CollapseButton.Click += CollapseButtonClick;
            ExpandButton.Click += ExpandButtonClick;
            StopButton.Click += StopButtonClick;
            QueryResultViewModel.RunningQueriesChanged += OnRunningQueriesChanged;

            InitializeComplete();
            InitializeMip();
        }

		void InitializeComplete()
		{
			_autoComplete = new AutocompletePopup(this);
		}

        #endregion

        #region Math Input Panel

        void InitializeMip()
        {
            try
            {
                _mip = new MathInputControl();
                _mip.SetCaptionText("Draw query");
                _mip.EnableExtendedButtons(true);
                _mip.Insert += InsertMathInputPanel;
                _mip.Close += CloseMathInputPanel;
                MathInputButton.Click += OpenMathInputPanel;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("MATH INPUT PANEL COULD NOT BE LOADED... See exception for details.");
                Trace.WriteLine(ex);
                MathInputButton.IsEnabled = false;
            }
		}

        void OpenMathInputPanel(object sender, EventArgs ev)
        {
            _mip.Show();
        }

        void CloseMathInputPanel()
        {
            _mip.Hide();
        }

        void InsertMathInputPanel(string query)
        {
            _mip.Clear();

            if (MathInputReceived != null)
                MathInputReceived(this, query);
        }

        #endregion

        #region Properties

        public Boolean HasNotification
        {
            get { return (Boolean)GetValue(HasNotificationProperty); }
            set { SetValue(HasNotificationProperty, value); }
        }

        public ICommand OpenEditorCommand
        {
            get { return (ICommand)GetValue(OpenEditorCommandProperty); }
            set { SetValue(OpenEditorCommandProperty, value); }
        }

        public Object OpenEditorCommandParameter
        {
            get { return (Object)GetValue(OpenEditorCommandParameterProperty); }
            set { SetValue(OpenEditorCommandParameterProperty, value); }
        }

		public ObservableCollection<AutocompleteItem> AutoCompleteItems
		{
			get { return (ObservableCollection<AutocompleteItem>)GetValue(AutoCompleteItemsProperty); }
			set { SetValue(AutoCompleteItemsProperty, value); }
		}

		public Single ConsoleFontSize 
		{
			get { return Console.FontSize; }
			set { Console.FontSize = value; }
		}

        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        public String Input
        {
            get { return GetValue(InputProperty) as String; }
            set { SetValue(InputProperty, value); }
        }

        public ObservableCollection<String> CommandHistory
        {
            get { return GetValue(CommandHistoryProperty) as ObservableCollection<String>; }
            set { SetValue(CommandHistoryProperty, value); }
        }

        #endregion

		#region Methods

		protected override Boolean HasEffectiveKeyboardFocus
		{
			get { return Console.Focused; }
		}

        void Backspace()
        {
            Console.ResetSelectionToPrompt();

            if (Console.Selection.IsEmpty)
            {
                Console.Selection.GoLeft(true);
            }

            Console.ClearSelected();
        }

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			SetFocus();
			e.Handled = true;
		}

        void OnRunningQueriesChanged(Object sender, EventArgs e)
        {
            StopButton.IsEnabled = QueryResultViewModel.HasRunningQueries;
        }

		public void SetFocus()
		{
			Host.Focus();
			Console.Focus();
		}

        void EditorClick(Object sender, RoutedEventArgs e)
        {
            var command = OpenEditorCommand;

            if (command != null)
            {
                command.Execute(OpenEditorCommandParameter);
            }
        }

        void StopButtonClick(Object sender, RoutedEventArgs e)
        {
            foreach (var query in QueryResultViewModel.RunningQueries)
            {
                query.Cancel();
            }
        }

        void ExpandButtonClick(Object sender, RoutedEventArgs e)
        {
            Console.ExpandAllFoldingBlocks();
        }

        void CollapseButtonClick(Object sender, RoutedEventArgs e)
        {
            Console.CollapseAllFoldingBlocks();
        }

        void InputPanelClick(Object sender, RoutedEventArgs e)
        {
            InputPanel.Toggle();
        }

        void AutocompleteClick(Object sender, RoutedEventArgs e)
		{
			_autoComplete.Show(true);
            Console.Focus();
		}

        void HistoryClick(Object sender, RoutedEventArgs e)
        {
            if (CommandHistoryList.Visibility == Visibility.Collapsed)
            {
                CommandHistoryList.Visibility = Visibility.Visible;
            }
            else
            {
                CommandHistoryList.Visibility = Visibility.Collapsed;
            }
        }

        void EvaluateClick(Object sender, RoutedEventArgs e)
        {
            Console.RunQuery();
            Console.Focus();
        }

		public void InsertAndRun(String query)
		{
			var current = Console.Query;
			Console.Query = query;
			Console.RunQuery();
			Console.Query = current;
		}

        public void InsertAndRun(String query, String message)
        {
            var current = Console.Query;
            Console.Query = "/* " + message + " */";
            Console.RunQuery(query);
            Console.Query = current;
        }

        public void InsertText(String text)
		{
			Console.InsertText(text);
		}

		void CommandHistoryClick(Object sender, RoutedEventArgs e)
		{
			var cmd = (TextBlock)(((Button)e.Source).Content);
			Console.Query = cmd.Text;
			Console.Focus();
		}

		public void Reset()
		{
			Console.Reset();
		}

        #endregion

        #region History

        void SavePersonalHistory()
		{
			_currentHistory[_historyIndex] = Console.Query;
		}

		void OnHistoryUp(Object sender, EventArgs e)
		{
			if (_historyIndex > 0)
			{
				SavePersonalHistory();
				_historyIndex -= 1;
				Console.Query = _currentHistory[_historyIndex];
			}
		}

		void OnHistoryDown(Object sender, EventArgs e)
		{
			if (_historyIndex + 1 < _currentHistory.Length)
			{
				SavePersonalHistory();
				_historyIndex += 1;
				Console.Query = _currentHistory[_historyIndex];
			}
		}

		void OnQueryEntered(Object sender, FastColoredTextBoxNS.QueryEventArgs e)
		{
			var query = new QueryResultViewModel(e.Query, e.Region);

            if (e.IsHistoryEntry)
            {
                if (CommandHistory.Count > 1 && CommandHistory[CommandHistory.Count - 2] == e.Query)
                {
                    CommandHistory.RemoveAt(CommandHistory.Count - 2);
                }

                CommandHistory[CommandHistory.Count - 1] = e.Query;
                _historyIndex = CommandHistory.Count;
                CommandHistory.Add(String.Empty);
                _currentHistory = CommandHistory.ToArray();
            }

            if (_command != null && _command.CanExecute(query))
            {
                _command.Execute(query);
            }
		}

		#endregion
	}
}
