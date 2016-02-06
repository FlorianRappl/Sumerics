namespace Sumerics.Controls
{
    using OxyPlot;
    using OxyPlot.Series;
    using System.Windows;
    using System.Windows.Controls;
    using YAMP;

	abstract class SumericsOxyPlot : SumericsPlot
    {
        #region Fields

        PlotModel model;
		OxyPlot.Wpf.Plot control;
        XYPlotValue plot;

        #endregion

        #region ctor

        public SumericsOxyPlot(XYPlotValue plot) : base(plot)
		{
            this.plot = plot;
            model = new PlotModel();
            control = new OxyPlot.Wpf.Plot();
            control.DataContext = model;
            SetGeneralProperties(model);
		}

        #endregion

        #region Properties

        public PlotModel Model
		{
			get { return model; }
		}

		public override FrameworkElement Content
		{
			get { return control; }
		}

        public XYPlotValue XYPlot 
        { 
            get { return plot; }
        }

        #endregion

        #region Methods

        public override void RenderToCanvas(Canvas canvas)
        {
            //var rc = new OxyPlot.Wpf.CanvasRenderContext(canvas);
            //model.Render(rc);
        }

		public override void CenterPlot()
		{
			control.ResetAllAxes();
		}

		public override void ExportPlot(string fileName, int width, int height)
		{
            control.SaveBitmap(fileName, width, height, OxyColors.White);
		}

        public override void ToggleGrid()
        {
            if (plot.Gridlines)
            {
                plot.Gridlines = false;
                plot.MinorGridlines = false;
            }
            else
            {
                plot.Gridlines = true;
                plot.MinorGridlines = true;
            }

            plot.UpdateLayout();
        }

		public override void AsPreview()
		{
			IsPreview = true;

			model.PlotAreaBorderThickness = new OxyThickness(0);
			model.PlotMargins = new OxyThickness(0);
			model.Padding = new OxyThickness(0);
			model.IsLegendVisible = false;
			model.Axes[0].IsAxisVisible = false;
			model.Axes[1].IsAxisVisible = false;

            model.Axes[0].IsZoomEnabled = false;
            model.Axes[1].IsZoomEnabled = false;
            control.IsManipulationEnabled = false;

			control.PlotAreaBorderThickness = new Thickness(0);
			control.PlotMargins = new Thickness(0);
			control.Padding = new Thickness(0);
			control.Margin = new Thickness(0);
		}

        #endregion

        #region Some very general modifiers

        protected void SetGeneralProperties(PlotModel model)
		{
			model.Title = plot.Title;
			model.PlotMargins = new OxyThickness(0);
			model.Padding = new OxyThickness(0, 10, 10, 0);
			model.IsLegendVisible = plot.ShowLegend;
			model.LegendBackground = plot.LegendBackground.OxyColorFromString();
			model.LegendBorderThickness = plot.LegendLineWidth;
			model.LegendBorder = plot.LegendLineColor.OxyColorFromString();
			model.LegendPosition = (OxyPlot.LegendPosition)plot.LegendPosition;
		}

        protected void UpdateLineSeries(XYAxisSeries series, IPointSeries points)
        {
            series.Title = points.Label;
            //series.StrokeThickness = points.Lines ? points.LineWidth : 0.0;
            //series.Color = points.Color.OxyColorFromString();
            //series.MarkerType = (MarkerType)((int)points.Symbol);
            //series.MarkerFill = series.Color;
            //series.MarkerSize = 3.0;
            //series.MarkerStroke = series.Color;
            //series.MarkerStrokeThickness = 1.0;
		}

		protected void Refresh()
		{
			if (Model.PlotView != null)
				Model.InvalidatePlot(false);
		}

        #endregion

        #region Helpers

        public static OxyColor[] GenerateColors(ColorPalettes palette, int length)
        {
            var c = new OxyColor[length];
            var op = typeof(OxyPalettes).GetMethod(palette.ToString()).Invoke(null, new object[] { length }) as OxyPalette;

            if (op == null)
                op = OxyPalettes.Jet(length);

            for (var i = 0; i < length; i++)
                c[i] = op.Colors[i];

            return c;
        }

        #endregion
    }
}
