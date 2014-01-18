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
using OxyPlot;

namespace Sumerics.Controls
{
    /// <summary>
    /// Interaction logic for SensorPlot.xaml
    /// </summary>
    public partial class SensorPlot : UserControl
	{
		#region Members

		PlotModel model;
		bool maximized;
		int t;
		int length;

		static OxyColor[] colors = new OxyColor[] { OxyColors.Blue, OxyColors.Red, OxyColors.Green };
		static MarkerType[] markers = new MarkerType[] { MarkerType.Square, MarkerType.Circle, MarkerType.Diamond };

		#endregion

		#region ctor

        public SensorPlot()
        {
            InitializeComponent();
            PlotControl.IsManipulationEnabled = false;
			PlotControl.Model = SetupModel();
		}

		#endregion

		#region Properties

		public string Title
        {
            get { return model.Title; }
			set { model.Title = value; }
        }

		public bool Maximized
		{
			get { return maximized; }
			set
			{
				maximized = value;

				if (maximized)
					model.Subtitle = "(Click to reduce)";
				else
					model.Subtitle = string.Empty;
			}
		}

        public string Unit
        {
			get { return model.Axes[1].Title; }
			set { model.Axes[1].Title = value; }
        }

		public int Length
		{
			get { return length; }
			set { length = value; }
		}

        public string Legend
        {
            get 
			{
				var labels = new List<string>();

				foreach(var series in model.Series)
					labels.Add(series.Title);

				return string.Join(",", labels.ToArray()); 
			}
            set
            {
                var labels = value.Split(',');

				for (var i = 0; i < labels.Length; i++)
				{
					if (model.Series.Count > i)
						model.Series[i].Title = labels[i];
					else
					{
						var series = new LineSeries(colors[i], 1.0, labels[i]);
						series.MarkerType = markers[i];
						model.Series.Add(series);
					}
				}
            }
		}

		#endregion

		#region Methods

		PlotModel SetupModel()
		{
			model = new PlotModel();
			model.TitleFontWeight = 1.0;
			model.TitleFontSize = 16;
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
			model.PlotAreaBorderThickness = 0;
			model.LegendOrientation = LegendOrientation.Horizontal;
			return model;
		}

        public void AddValues(params double[] values)
        {
            for(var i = 0; i < values.Length; i++)
            {
				var series = (LineSeries)model.Series[i];
				
				series.Points.Add(new DataPoint
				{
					X = t,
					Y = values[i]
				});

				while (series.Points.Count > length)
					series.Points.RemoveAt(0);
            }

			if(model.PlotControl != null)
				model.RefreshPlot(true);

            if(!maximized)
                PlotControl.ResetAllAxes();

			t++;
		}

		#endregion
	}
}
