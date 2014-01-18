using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using OxyPlot;
using YAMP;

namespace Sumerics.Controls
{
    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : UserControl
    {
        #region Members

        SumericsPlot plot;
        IPlotViewModel data;
        bool canDock;

        #endregion

        #region ctor

        public PlotControl()
        {
            InitializeComponent();
			PlotArea.Content = Placeholder;
		}

		#endregion

		#region Binding stuff

		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
				"Data",
				typeof(IPlotViewModel),
				typeof(PlotControl),
				new FrameworkPropertyMetadata(null, OnDataChanged));

		static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as PlotControl;
			var value = e.NewValue as IPlotViewModel;
			var activate = false;
			var grid = true;
            var series = true;

			if (value == null)
				control.PlotArea.Content = control.Placeholder;
			else
			{
                var t = PlotFactory.Create(value.Plot);

                if (t != null)
                {
                    activate = true;
                    control.plot = t;
                    control.PlotArea.Content = t.Content;
                    series = t.IsSeriesEnabled;
                    grid = t.IsGridEnabled;
                    control.data = value;
                }
			}

			control.SettingsButton.IsEnabled = activate;
			control.SaveButton.IsEnabled = activate;
			control.SeriesButton.IsEnabled = activate && series;
			control.PrintButton.IsEnabled = activate;
			control.GridButton.IsEnabled = activate && grid;
            control.CenterButton.IsEnabled = activate;
            control.DuplicateButton.IsEnabled = activate;
		}

		#endregion

		#region Properties

		public IPlotViewModel Data
        {
            get { return GetValue(DataProperty) as IPlotViewModel; }
            set { SetValue(DataProperty, value); }
        }

		TextBlock Placeholder
		{
			get
			{
				var tb = new TextBlock();
				tb.Text = "Nothing to visualize yet. Type plot(1:100, rand(100,3)) to generate a simple plot.";
				tb.FontSize = 16;
				tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
				tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
				tb.Foreground = new SolidColorBrush(Colors.DarkGray);
				return tb;
			}
		}

        public bool CanDock
        {
            get { return canDock; }
            set
            {
                canDock = value;

                if (canDock)
                {
                    DockImg.Source = new BitmapImage(new Uri(@"Images\dock.png", UriKind.Relative));
                    ConsoleButton.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    DockImg.Source = new BitmapImage(new Uri(@"Images\undock.png", UriKind.Relative));
                    ConsoleButton.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

		#endregion

        #region Printing Methods

        void PrintButtonClick(object sender, RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                var canvas = new Canvas
                {
                    Width = printDialog.PrintableAreaWidth,
                    Height = printDialog.PrintableAreaHeight
                };

                canvas.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                canvas.Arrange(new Rect(0, 0, printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                plot.RenderToCanvas(canvas);
                canvas.UpdateLayout();
                printDialog.PrintVisual(canvas, plot.Plot.Title ?? "Sumerics Plot");
            }
        }

        #endregion

        #region Methods

        void GridButtonClick(object sender, RoutedEventArgs e)
		{
            plot.ToggleGrid();
		}

		void CenterButtonClick(object sender, RoutedEventArgs e)
		{
			plot.CenterPlot();
        }

        void SettingsButtonClick(object sender, RoutedEventArgs e)
        {
            data.OpenPlotSettings();
        }

        void SeriesButtonClick(object sender, RoutedEventArgs e)
        {
            data.OpenPlotSeries();
        }

        void ConsoleButtonClick(object sender, RoutedEventArgs e)
        {
            data.OpenConsole();
        }

        void DuplicateButtonClick(object sender, RoutedEventArgs e)
        {
            if (canDock)
                data.DockPlot();
            else
                data.UndockPlot();
        }

        void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            data.SavePlot(plot);
        }

        public void Undock()
        {
            if (data != null)
                data.UndockPlot();
        }

        public void Dock()
        {
            if (data != null)
                data.DockPlot();
        }

		#endregion
	}
}
