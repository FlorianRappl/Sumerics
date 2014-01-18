using OxyPlot;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PreviewPlot.xaml
    /// </summary>
    public partial class PreviewPlot : UserControl
    {
        #region ctor

        public PreviewPlot()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency Property

        public Value SelectedPreview
        {
            get { return (Value)GetValue(SelectedPreviewProperty); }
            set { SetValue(SelectedPreviewProperty, value); }
        }

        public static readonly DependencyProperty SelectedPreviewProperty =
            DependencyProperty.Register("SelectedPreview", typeof(Value), typeof(PreviewPlot), new PropertyMetadata(null, OnSelectedPreviewChanged));

        #endregion

        #region Generate Preview

        static void OnSelectedPreviewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (PreviewPlot)d;

            if (e.NewValue == e.OldValue)
                return;

            if (e.NewValue == null)
                ctrl.Content = null;
            else
                ctrl.Content = GeneratePreview((Value)e.NewValue);
        }

        static object GeneratePreview(Value value)
		{
			if (value is PlotValue)
			{
                var plotControl = PlotFactory.Create((PlotValue)value);
                plotControl.AsPreview();
				return plotControl.Content;
			}
			
			var model = new PlotModel();

			if (value is StringValue)
			{
				var sv = value as StringValue;
				var str = sv.Value;
				var series = new LineSeries();
				series.Color = OxyColors.SteelBlue;

				for (var i = 0; i < str.Length; i++)
					series.Points.Add(new DataPoint(i + 1, str[i]));

				model.Series.Add(series);
			}
			else if (value is MatrixValue)
			{
				var mv = value as MatrixValue;
				var transpose = mv.DimensionX > mv.DimensionY;
				var dimension = transpose ? mv.DimensionX : mv.DimensionY;
				var count = transpose ? mv.DimensionY : mv.DimensionX;

				for (var i = 0; i < count; i++)
				{
					var k = i + 1;
					var series = new LineSeries();
					series.Color = model.DefaultColors[i % model.DefaultColors.Count];

					for (var j = 1; j <= dimension; j++)
					{
						var val = transpose ? mv[k, j] : mv[j, k];
						series.Points.Add(new DataPoint(j, val.Value));
					}

					model.Series.Add(series);
				}
			}
			else if (value is ScalarValue)
			{
                var sv = value as ScalarValue;
                var sqrt = sv.Sqrt();
				var series = new ScatterSeries();
				series.Points.Add(new DataPoint(sv.Re, sv.Im));
				series.Points.Add(new DataPoint(sv.IntValue, sv.ImaginaryIntValue));
				series.Points.Add(new DataPoint(sv.Abs(), 0));
				series.Points.Add(new DataPoint(sqrt.Re, sqrt.Im));
				model.Series.Add(series);
			}

			model.Axes.Add(new LinearAxis(AxisPosition.Left, string.Empty));
			model.Axes.Add(new LinearAxis(AxisPosition.Bottom, string.Empty));
			model.PlotAreaBorderThickness = 0;
			model.PlotMargins = new OxyThickness(0);
			model.Padding = new OxyThickness(0);
			model.IsLegendVisible = false;
			model.Axes[0].IsAxisVisible = false;
			model.Axes[1].IsAxisVisible = false;
			model.AutoAdjustPlotMargins = false;
			var ctrl = new OxyPlot.Wpf.Plot();
			ctrl.Model = model;
			return ctrl;
        }

        #endregion
    }
}
