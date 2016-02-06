namespace Sumerics.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using YAMP;

	/// <summary>
	/// Interaction logic for ScriptControl.xaml
	/// </summary>
	public partial class ScriptControl : UserControl
	{
		#region Dependency Properties

		public static readonly DependencyProperty SettingsCommandProperty = DependencyProperty.Register(
			"SettingsCommand",
			typeof(ICommand),
			typeof(ScriptControl),
			new FrameworkPropertyMetadata(null, OnSettingsCommandChange));

		public static readonly DependencyProperty RunCommandProperty = DependencyProperty.Register(
			"RunCommand",
			typeof(ICommand),
			typeof(ScriptControl),
			new FrameworkPropertyMetadata(null, OnRunCommandChange));

		public static readonly DependencyProperty ContextProperty = DependencyProperty.Register(
			"Context", 
			typeof(ParseContext),
			typeof(ScriptControl), 
			new PropertyMetadata(null));

		public static readonly DependencyProperty SensorFunctionsProperty = DependencyProperty.Register(
			"SensorFunctions",
			typeof(ObservableCollection<string>),
			typeof(ScriptControl),
			new PropertyMetadata(new ObservableCollection<string>()));

		public static readonly DependencyProperty NumericFunctionsProperty = DependencyProperty.Register(
			"NumericFunctions",
			typeof(ObservableCollection<string>),
			typeof(ScriptControl),
			new PropertyMetadata(new ObservableCollection<string>()));

		public static readonly DependencyProperty PlotFunctionsProperty = DependencyProperty.Register(
			"PlotFunctions",
			typeof(ObservableCollection<string>),
			typeof(ScriptControl),
			new PropertyMetadata(new ObservableCollection<string>()));

		public static readonly DependencyProperty SystemFunctionsProperty = DependencyProperty.Register(
			"SystemFunctions",
			typeof(ObservableCollection<string>),
			typeof(ScriptControl),
			new PropertyMetadata(new ObservableCollection<string>()));

		static void OnSettingsCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var pc = (ScriptControl)d;
			pc.SettingsButton.Command = (ICommand)e.NewValue;
		}

		static void OnRunCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var pc = (ScriptControl)d;
			pc.RunButton.Command = (ICommand)e.NewValue;
		}

		#endregion

		#region Constants

		const Int32 ElementOffset = 5;
		const Int32 ElementHeight = 50;

		#endregion

        #region Fields

        readonly List<OutConnectorNode> _inputNodes = new List<OutConnectorNode>();
        readonly List<InConnectorNode> _outputNodes = new List<InConnectorNode>();

		Double _scale = 1.0;
		Double _translateX = 0.0;
		Double _translateY = 0.0;

        static ScriptControl _instance;

		#endregion

		#region ctor

		public ScriptControl()
		{
			InitializeComponent();
			AddLeft();
			AddRight();
			AddOutput();
		}

		#endregion

		#region Properties

        public static ScriptControl Instance
        {
            get { return _instance ?? (_instance = new ScriptControl()); }
        }

		public ObservableCollection<string> SensorFunctions
		{
			get { return (ObservableCollection<string>)GetValue(SensorFunctionsProperty); }
			set { SetValue(SensorFunctionsProperty, value); }
		}

		public ObservableCollection<string> NumericFunctions
		{
			get { return (ObservableCollection<string>)GetValue(NumericFunctionsProperty); }
			set { SetValue(NumericFunctionsProperty, value); }
		}

		public ObservableCollection<string> PlotFunctions
		{
			get { return (ObservableCollection<string>)GetValue(PlotFunctionsProperty); }
			set { SetValue(PlotFunctionsProperty, value); }
		}

		public ObservableCollection<string> SystemFunctions
		{
			get { return (ObservableCollection<string>)GetValue(SystemFunctionsProperty); }
			set { SetValue(SystemFunctionsProperty, value); }
		}

		public ParseContext Context
		{
			get { return (ParseContext)GetValue(ContextProperty); }
			set { SetValue(ContextProperty, value); }
		}

		public ICommand RunCommand
		{
			get { return RunButton.Command; }
			set { RunButton.Command = value; }
		}

		public ICommand SettingsCommand
		{
			get { return SettingsButton.Command; }
			set { SettingsButton.Command = value; }
		}

		#endregion

		#region Methods

        public void RemoveChild(ScriptElement element)
        {
            element.ScriptHost.Dispose();
            CenterContainer.Children.Remove(element);
        }

        public void Shift(ScriptElement scriptElement, double dx, double dy)
        {
            var left = dx / _scale + Canvas.GetLeft(scriptElement);
            var top = dy / _scale + Canvas.GetTop(scriptElement);
            Canvas.SetLeft(scriptElement, left);
            Canvas.SetTop(scriptElement, top);
        }

		void AddInput()
		{
			var ctrl = new OutConnectorNode();
			var ellipse = ctrl.Control;
			ellipse.Width = 20;
			ellipse.Height = 20;
			TopContainer.Children.Add(ellipse);

			Canvas.SetLeft(ellipse, 150 + 40 * _inputNodes.Count);
			Canvas.SetBottom(ellipse, ellipse.Height);
			_inputNodes.Add(ctrl);

			DeleteInputButton.IsEnabled = true;
			AddInputButton.IsEnabled = _inputNodes.Count < 8;
		}

		void RemoveInput()
		{
			var ctrl = _inputNodes[_inputNodes.Count - 1];
			TopContainer.Children.Remove(ctrl.Control);
			_inputNodes.RemoveAt(_inputNodes.Count - 1);

			DeleteInputButton.IsEnabled = _inputNodes.Count > 0;
			AddInputButton.IsEnabled = true;
		}

		void AddOutput()
		{
			var ctrl = new InConnectorNode();
			var ellipse = ctrl.Control;
			ellipse.Width = 20;
			ellipse.Height = 20;
			TopContainer.Children.Add(ellipse);

			Canvas.SetRight(ellipse, 150 + 40 * _inputNodes.Count);
			Canvas.SetBottom(ellipse, ellipse.Height);
			_outputNodes.Add(ctrl);
		}

		#region Add Left and Right

		void AddLeft()
		{
            var snsr = new ScriptElement
            {
                ScriptHost = new SensorScriptObject()
            };
            var func = new ScriptElement
            {
                ScriptHost = new FunctionScriptObject()
            };
            var plot = new ScriptElement
            {
                ScriptHost = new PlotScriptObject()
            };
            var oprt = new ScriptElement
            {
                ScriptHost = new OperatorScriptObject()
            };

            var list = new[] { snsr, func, plot, oprt };
			var pos = ElementOffset;

			foreach(var el in list)
			{
				Add(LeftContainer, 5, pos, el);
				pos += ElementHeight + ElementOffset;
			}
		}

		void AddRight()
		{
            var stmt = new ScriptElement
            {
                ScriptHost = new StatementScriptObject()
            };
            var cnds = new ScriptElement
            {
                ScriptHost = new ConditionScriptObject()
            };

			var list = new[] { stmt, cnds };
			var pos = ElementOffset;

			foreach (var el in list)
			{
				Add(RightContainer, 5, pos, el);
				pos += ElementHeight + ElementOffset;
			}
		}

		#endregion

        void Add(Canvas container, Int32 left, Int32 top, ScriptElement element)
		{
			container.Children.Add(element);
			Canvas.SetLeft(element, left);
			Canvas.SetTop(element, top);
		}

		void AddInputButtonClick(Object sender, RoutedEventArgs e)
		{
			AddInput();
		}

        void DeleteInputButtonClick(Object sender, RoutedEventArgs e)
		{
			RemoveInput();
		}

        void ToggleConnectorsButtonClick(Object sender, RoutedEventArgs e)
		{

		}

        void CenterButtonClick(Object sender, RoutedEventArgs e)
		{
			_scale = 1.0;
			_translateX = 0.0;
			_translateY = 0.0;
			ApplyTransformations();
		}

		void ApplyTransformations()
		{
			var width = CenterContainer.ActualWidth;
			var height = CenterContainer.ActualHeight;
			var tr = new TranslateTransform(_translateX, _translateY);
			var tg = new ScaleTransform(_scale, _scale, width / 2, height / 2);
			CenterContainer.LayoutTransform = tg;

            foreach (UIElement child in CenterContainer.Children)
            {
                child.RenderTransform = tr;
            }
		}

        void MagnifyInButtonClick(Object sender, RoutedEventArgs e)
		{
			_scale *= 1.1;
			ApplyTransformations();
		}

        void MagnifyOutButtonClick(Object sender, RoutedEventArgs e)
		{
			_scale /= 1.1;
			ApplyTransformations();
		}

		#endregion
    }
}
