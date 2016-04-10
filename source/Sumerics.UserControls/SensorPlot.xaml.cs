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

		static readonly OxyColor[] Colors = new[] 
        { 
            OxyColors.Blue, 
            OxyColors.Red,
            OxyColors.Green 
        };

		static readonly MarkerType[] Markers = new[] 
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
                    _model.Subtitle = String.Empty;
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
                        _model.Series.Add(new LineSeries 
                        { 
                            Color = Colors[i], 
                            StrokeThickness = 1.0, 
                            Title = labels[i],
                            MarkerType = Markers[i]
                        });
                    }
				}
            }
		}

		#endregion

		#region Methods

		static PlotModel CreateModel()
        {
            var model = new PlotModel
            {
			    TitleFontWeight = 1.0,
			    TitleFontSize = 16.0,
			    TitleColor = OxyColors.DarkGray,
			    LegendPosition = LegendPosition.LeftTop,
			    PlotMargins = new OxyThickness(0),
			    Padding = new OxyThickness(0, 10, 10, 0),
			    LegendBackground = OxyColor.FromArgb(100, 240, 240, 240),
			    LegendBorder = OxyColors.LightGray,
			    PlotAreaBorderThickness = new OxyThickness(0),
			    LegendOrientation = LegendOrientation.Horizontal,
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Seconds" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
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
