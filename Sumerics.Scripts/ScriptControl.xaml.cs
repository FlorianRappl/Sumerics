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
using YAMP;

namespace Sumerics.Controls
{
	/// <summary>
	/// Interaction logic for ScriptControl.xaml
	/// </summary>
	public partial class ScriptControl : UserControl
	{
		#region Binding stuff

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
			new PropertyMetadata(Parser.PrimaryContext));

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

		const int EL_OFFSET = 5;
		const int EL_HEIGHT = 50;

		#endregion

		#region Members

		double scale = 1.0;
		double translateX = 0.0;
		double translateY = 0.0;
		List<OutConnectorNode> inputNodes = new List<OutConnectorNode>();
		List<InConnectorNode> outputNodes = new List<InConnectorNode>();

        static ScriptControl instance;

		#endregion

		#region ctor

		public ScriptControl()
		{
            instance = this;
			InitializeComponent();
			AddLeft();
			AddRight();
			AddOutput();
		}

		#endregion

		#region Properties

        public static ScriptControl Instance
        {
            get
            {
                return instance ?? new ScriptControl();
            }
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
            var left = dx / scale + Canvas.GetLeft(scriptElement);
            var top = dy / scale + Canvas.GetTop(scriptElement);
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

			Canvas.SetLeft(ellipse, 150 + 40 * inputNodes.Count);
			Canvas.SetBottom(ellipse, ellipse.Height);
			inputNodes.Add(ctrl);

			DeleteInputButton.IsEnabled = true;
			AddInputButton.IsEnabled = inputNodes.Count < 8;
		}

		void RemoveInput()
		{
			var ctrl = inputNodes[inputNodes.Count - 1];
			TopContainer.Children.Remove(ctrl.Control);
			inputNodes.RemoveAt(inputNodes.Count - 1);

			DeleteInputButton.IsEnabled = inputNodes.Count > 0;
			AddInputButton.IsEnabled = true;
		}

		void AddOutput()
		{
			var ctrl = new InConnectorNode();
			var ellipse = ctrl.Control;
			ellipse.Width = 20;
			ellipse.Height = 20;
			TopContainer.Children.Add(ellipse);

			Canvas.SetRight(ellipse, 150 + 40 * inputNodes.Count);
			Canvas.SetBottom(ellipse, ellipse.Height);
			outputNodes.Add(ctrl);
		}

		#region Add Left and Right

		void AddLeft()
		{
			var _sensor = new ScriptElement();
			_sensor.ScriptHost = new SensorScriptObject();
			var _function = new ScriptElement();
			_function.ScriptHost = new FunctionScriptObject();
			var _plot = new ScriptElement();
			_plot.ScriptHost = new PlotScriptObject();
			var _operator = new ScriptElement();
			_operator.ScriptHost = new OperatorScriptObject();

			var list = new[] { _sensor, _function, _plot, _operator };
			var pos = EL_OFFSET;

			foreach(var el in list)
			{
				Add(LeftContainer, 5, pos, el);
				pos += EL_HEIGHT + EL_OFFSET;
			}
		}

		void AddRight()
		{
			var _statement = new ScriptElement();
			_statement.ScriptHost = new StatementScriptObject();
			var _conditions = new ScriptElement();
			_conditions.ScriptHost = new ConditionScriptObject();

			var list = new[] { _statement, _conditions };
			var pos = EL_OFFSET;

			foreach (var el in list)
			{
				Add(RightContainer, 5, pos, el);
				pos += EL_HEIGHT + EL_OFFSET;
			}
		}

		#endregion

		void Add(Canvas container, int left, int top, ScriptElement element)
		{
			container.Children.Add(element);
			Canvas.SetLeft(element, left);
			Canvas.SetTop(element, top);
		}

		void AddInputButtonClick(object sender, RoutedEventArgs e)
		{
			AddInput();
		}

		void DeleteInputButtonClick(object sender, RoutedEventArgs e)
		{
			RemoveInput();
		}

		void ToggleConnectorsButtonClick(object sender, RoutedEventArgs e)
		{

		}

		void CenterButtonClick(object sender, RoutedEventArgs e)
		{
			scale = 1.0;
			translateX = 0.0;
			translateY = 0.0;
			ApplyTransformations();
		}

		void ApplyTransformations()
		{
			var width = CenterContainer.ActualWidth;
			var height = CenterContainer.ActualHeight;
			var tr = new TranslateTransform(translateX, translateY);
			var tg = new ScaleTransform(scale, scale, width / 2, height / 2);
			CenterContainer.LayoutTransform = tg;

			foreach (UIElement child in CenterContainer.Children)
				child.RenderTransform = tr;
		}

		void MagnifyInButtonClick(object sender, RoutedEventArgs e)
		{
			scale *= 1.1;
			ApplyTransformations();
		}

		void MagnifyOutButtonClick(object sender, RoutedEventArgs e)
		{
			scale /= 1.1;
			ApplyTransformations();
		}

		#endregion
    }
}
