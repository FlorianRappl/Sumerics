namespace Sumerics.Controls
{
    using System;
    using System.Collections.ObjectModel;
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

        readonly AutocompletePopup _autoComplete;
        readonly MathInputPanelWrapper _mipw;
        ICommand _command;
        String[] _currentHistory;
		Int32 _historyIndex;

        #endregion

        #region Events

        public event EventHandler<String> MathInputReceived
        {
            add { _mipw.OnInsertPressed += value; }
            remove { _mipw.OnInsertPressed -= value; }
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty HasNotificationProperty = DependencyProperty.Register(
            "HasNotification", 
            typeof(Boolean), 
            typeof(ConsoleControl),
            new FrameworkPropertyMetadata(false, OnNotificationChanged));

        public static readonly DependencyProperty CanStopProperty = DependencyProperty.Register(
            "CanStop",
            typeof(Boolean),
            typeof(ConsoleControl),
            new FrameworkPropertyMetadata(false, OnStopChanged));     

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(ConsoleControl),
            new FrameworkPropertyMetadata(null, OnCommandChange));

        public static readonly DependencyProperty InputProperty = DependencyProperty.Register(
            "Input",
            typeof(String),
            typeof(ConsoleControl),
            new FrameworkPropertyMetadata(null, OnInputChange));

        public static readonly DependencyProperty CommandHistoryProperty = DependencyProperty.Register(
            "CommandHistory",
            typeof(ObservableCollection<String>),
            typeof(ConsoleControl),
			new FrameworkPropertyMetadata(new ObservableCollection<String>()));

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

        public static readonly DependencyProperty CancelQueriesCommandProperty = DependencyProperty.Register(
            "CancelQueriesCommand",
            typeof(ICommand),
            typeof(ConsoleControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CancelQueriesCommandParameterProperty = DependencyProperty.Register(
            "CancelQueriesCommandParameter",
            typeof(Object),
            typeof(ConsoleControl),
            new PropertyMetadata(null));

        static void OnNotificationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as ConsoleControl);
            control.Notification.IsOpen = (Boolean)e.NewValue;
        }

        static void OnStopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as ConsoleControl);
            control.StopButton.IsEnabled = (Boolean)e.NewValue;
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
            var value = e.NewValue as String;
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
            _currentHistory = new String[0];
			CommandHistory.Add(String.Empty);

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

            _autoComplete = new AutocompletePopup(this);
            _mipw = new MathInputPanelWrapper("Draw query");

            if (!_mipw.IsAvailable)
            {
                MathInputButton.IsEnabled = false;
            }
            else
            {
                MathInputButton.Click += (s, ev) => _mipw.Open();
            }
        }

        #endregion

        #region Properties

        public Boolean HasNotification
        {
            get { return (Boolean)GetValue(HasNotificationProperty); }
            set { SetValue(HasNotificationProperty, value); }
        }

        public Boolean CanStop
        {
            get { return (Boolean)GetValue(CanStopProperty); }
            set { SetValue(CanStopProperty, value); }
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

        public ICommand CancelQueriesCommand
        {
            get { return (ICommand)GetValue(CancelQueriesCommandProperty); }
            set { SetValue(CancelQueriesCommandProperty, value); }
        }

        public Object CancelQueriesCommandParameter
        {
            get { return (Object)GetValue(CancelQueriesCommandParameterProperty); }
            set { SetValue(CancelQueriesCommandParameterProperty, value); }
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
            var command = CancelQueriesCommand;

            if (command != null)
            {
                command.Execute(CancelQueriesCommandParameter);
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

            var cqr = new ConsoleQueryReference(e.Query, e.Region, Console);

            if (_command != null && _command.CanExecute(cqr))
            {
                _command.Execute(cqr);
            }
		}

		#endregion
	}
}
