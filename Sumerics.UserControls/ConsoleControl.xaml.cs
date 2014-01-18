using FastColoredTextBoxNS;
using micautLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YAMP;

namespace Sumerics.Controls
{
    /// <summary>
    /// Interaction logic for ConsoleControl.xaml
    /// </summary>
    public partial class ConsoleControl : UserControl
    {
        #region Members

        ICommand command;
        string[] currentHistory;
		int historyIndex;
		AutocompletePopup autoComplete;
        MathInputControl mip;

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

        public static readonly DependencyProperty OpenEditorProperty =  DependencyProperty.Register(
            "OpenEditor", 
            typeof(ICommand), 
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
            control.command = value;
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
            basis.autoComplete.AvailableItems = (ObservableCollection<AutocompleteItem>)e.NewValue;
        }		

        #endregion

        #region ctor

        public ConsoleControl()
        {
            InitializeComponent();
            InputPanel.PlaceFocus = SetFocus;
            InputPanel.Insert = InsertText;
            InputPanel.Delete = Backspace;
            currentHistory = new string[0];
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
			autoComplete = new AutocompletePopup(this);
		}

        #endregion

        #region Math Input Panel

        void InitializeMip()
        {
            try
            {
                mip = new MathInputControl();
                mip.SetCaptionText("Draw query");
                mip.EnableExtendedButtons(true);
                mip.Insert += InsertMathInputPanel;
                mip.Close += CloseMathInputPanel;
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
            mip.Show();
        }

        void CloseMathInputPanel()
        {
            mip.Hide();
        }

        void InsertMathInputPanel(string query)
        {
            mip.Clear();

            if (MathInputReceived != null)
                MathInputReceived(this, query);
        }

        #endregion

        #region Properties

        public bool HasNotification
        {
            get { return (bool)GetValue(HasNotificationProperty); }
            set { SetValue(HasNotificationProperty, value); }
        }

        public ICommand OpenEditor
        {
            get { return (ICommand)GetValue(OpenEditorProperty); }
            set { SetValue(OpenEditorProperty, value); }
        }

		public ObservableCollection<AutocompleteItem> AutoCompleteItems
		{
			get { return (ObservableCollection<AutocompleteItem>)GetValue(AutoCompleteItemsProperty); }
			set { SetValue(AutoCompleteItemsProperty, value); }
		}

		public float ConsoleFontSize 
		{
			get { return Console.FontSize; }
			set { Console.FontSize = value; }
		}

        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        public string Input
        {
            get { return GetValue(InputProperty) as string; }
            set { SetValue(InputProperty, value); }
        }

        public ObservableCollection<string> CommandHistory
        {
            get { return GetValue(CommandHistoryProperty) as ObservableCollection<string>; }
            set { SetValue(CommandHistoryProperty, value); }
        }

        #endregion

		#region Methods

		protected override bool HasEffectiveKeyboardFocus
		{
			get
			{
				return Console.Focused;
			}
		}

        void Backspace()
        {
            Console.ResetSelectionToPrompt();

            if (Console.Selection.IsEmpty)
                Console.Selection.GoLeft(true);

            Console.ClearSelected();
        }

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			SetFocus();
			e.Handled = true;
		}

        void OnRunningQueriesChanged(object sender, EventArgs e)
        {
            StopButton.IsEnabled = QueryResultViewModel.HasRunningQueries;
        }

		public void SetFocus()
		{
			Host.Focus();
			Console.Focus();
		}

        void EditorClick(object sender, RoutedEventArgs e)
        {
            OpenEditor.Execute(null);
        }

        void StopButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var query in QueryResultViewModel.RunningQueries)
                query.Cancel();
        }

        void ExpandButtonClick(object sender, RoutedEventArgs e)
        {
            Console.ExpandAllFoldingBlocks();
        }

        void CollapseButtonClick(object sender, RoutedEventArgs e)
        {
            Console.CollapseAllFoldingBlocks();
        }

        void InputPanelClick(object sender, RoutedEventArgs e)
        {
            InputPanel.Toggle();
        }

		void AutocompleteClick(object sender, RoutedEventArgs e)
		{
			autoComplete.Show(true);
            Console.Focus();
		}

        void HistoryClick(object sender, RoutedEventArgs e)
        {
            if (CommandHistoryList.Visibility == Visibility.Collapsed)
                CommandHistoryList.Visibility = Visibility.Visible;
            else
                CommandHistoryList.Visibility = Visibility.Collapsed;
        }

        void EvaluateClick(object sender, RoutedEventArgs e)
        {
            Console.RunQuery();
            Console.Focus();
        }

		public void InsertAndRun(string query)
		{
			var current = Console.Query;
			Console.Query = query;
			Console.RunQuery();
			Console.Query = current;
		}

        public void InsertAndRun(string query, string message)
        {
            var current = Console.Query;
            Console.Query = "/* " + message + " */";
            Console.RunQuery(query);
            Console.Query = current;
        }

		public void InsertText(string text)
		{
			Console.InsertText(text);
		}

		void CommandHistoryClick(object sender, RoutedEventArgs e)
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
			currentHistory[historyIndex] = Console.Query;
		}

		void OnHistoryUp(object sender, EventArgs e)
		{
			if (historyIndex > 0)
			{
				SavePersonalHistory();
				historyIndex -= 1;
				Console.Query = currentHistory[historyIndex];
			}
		}

		void OnHistoryDown(object sender, EventArgs e)
		{
			if (historyIndex + 1 < currentHistory.Length)
			{
				SavePersonalHistory();
				historyIndex += 1;
				Console.Query = currentHistory[historyIndex];
			}
		}

		void OnQueryEntered(object sender, FastColoredTextBoxNS.QueryEventArgs e)
		{
			var query = new QueryResultViewModel(e.Query, e.Region);

            if (e.IsHistoryEntry)
            {
                if (CommandHistory.Count > 1 && CommandHistory[CommandHistory.Count - 2] == e.Query)
                    CommandHistory.RemoveAt(CommandHistory.Count - 2);

                CommandHistory[CommandHistory.Count - 1] = e.Query;
                historyIndex = CommandHistory.Count;
                CommandHistory.Add(string.Empty);
                currentHistory = CommandHistory.ToArray();
            }

			if (command != null && command.CanExecute(query))
				command.Execute(query);
		}

		#endregion
	}
}
