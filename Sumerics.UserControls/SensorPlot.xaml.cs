namespace Sumerics.Controls
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SensorPlot.xaml
    /// </summary>
    public partial class SensorPlot : UserControl
	{
		#region Fields

		readonly PlotModel _model;

		Boolean _maximized;
		Int32 _time;
		Int32 _length;

		static readonly OxyColor[] colors = new[] 
        { 
            OxyColors.Blue, 
            OxyColors.Red,
            OxyColors.Green 
        };

		static readonly MarkerType[] markers = new[] 
        { 
            MarkerType.Square, 
            MarkerType.Circle, 
            MarkerType.Diamond 
        };

		#endregion

		#region ctor

        public SensorPlot()
        {
            InitializeComponent();
            PlotControl.IsManipulationEnabled = false;
            PlotControl.Model = _model = CreateModel();
		}

		#endregion

		#region Properties

		public String Title
        {
            get { return _model.Title; }
			set { _model.Title = value; }
        }

		public Boolean Maximized
		{
			get { return _maximized; }
			set
			{
				_maximized = value;

                if (_maximized)
                {
                    _model.Subtitle = "(Click to reduce)";
                }
                else
                {
                    _model.Subtitle = string.Empty;
                }
			}
		}

        public String Unit
        {
			get { return _model.Axes[1].Title; }
			set { _model.Axes[1].Title = value; }
        }

		public Int32 Length
		{
			get { return _length; }
			set { _length = value; }
		}

        public String Legend
        {
            get 
			{
				var labels = new List<String>();

                foreach (var series in _model.Series)
                {
                    labels.Add(series.Title);
                }

				return String.Join(",", labels.ToArray()); 
			}
            set
            {
                var labels = value.Split(',');

				for (var i = 0; i < labels.Length; i++)
				{
                    if (_model.Series.Count > i)
                    {
                        _model.Series[i].Title = labels[i];
                    }
                    else
                    {
                        var series = new LineSeries { Color = colors[i], StrokeThickness = 1.0, Title = labels[i] };
                        series.MarkerType = markers[i];
                        _model.Series.Add(series);
                    }
				}
            }
		}

		#endregion

		#region Methods

		static PlotModel CreateModel()
        {
            var model = new PlotModel();
			model.TitleFontWeight = 1.0;
			model.TitleFontSize = 16.0;
			model.TitleColor = OxyColors.DarkGray;
			model.Axes.Add(new LinearAxis());
			model.Axes.Add(new LinearAxis());
			model.Axes[0].Position = AxisPosition.Bottom;
			model.Axes[0].Title = "Seconds";
			model.Axes[1].Position = AxisPosition.Left;
			model.LegendPosition = LegendPosition.LeftTop;
			model.PlotMargins = new OxyThickness(0);
			model.Padding = new OxyThickness(0, 10, 10, 0);
			model.LegendBackground = OxyColor.FromArgb(100, 240, 240, 240);
			model.LegendBorder = OxyColors.LightGray;
			model.PlotAreaBorderThickness = new OxyThickness(0);
			model.LegendOrientation = LegendOrientation.Horizontal;
            return model;
		}

        public void AddValues(params Double[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
				var series = (LineSeries)_model.Series[i];
				
				series.Points.Add(new DataPoint(_time, values[i]));

                while (series.Points.Count > _length)
                {
                    series.Points.RemoveAt(0);
                }
            }

            if (_model.PlotView != null)
            {
                _model.PlotView.InvalidatePlot(true);
            }

            if (!_maximized)
            {
                PlotControl.ResetAllAxes();
            }

			_time++;
		}

		#endregion
	}
}
