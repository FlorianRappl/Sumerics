using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using YAMP;
using System.Windows.Media.Imaging;

namespace Sumerics.Controls
{
	abstract class SumericsOxyPlot : SumericsPlot
    {
        #region Members

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
            control.Model = model;
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
            var rc = new OxyPlot.Wpf.ShapesRenderContext(canvas);
            model.Update();
            model.Render(rc);
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

			model.PlotAreaBorderThickness = 0;
			model.PlotMargins = new OxyThickness(0);
			model.Padding = new OxyThickness(0);
			model.IsLegendVisible = false;
			model.Axes[0].IsAxisVisible = false;
			model.Axes[1].IsAxisVisible = false;
            model.AutoAdjustPlotMargins = false;

            model.Axes[0].IsZoomEnabled = false;
            model.Axes[1].IsZoomEnabled = false;
            control.IsManipulationEnabled = false;

			control.PlotAreaBorderThickness = 0;
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

		protected void UpdateLineSeries(LineSeries series, IPointSeries points)
		{
			if (!points.Lines)
				series.StrokeThickness = 0.0;
			else
				series.StrokeThickness = points.LineWidth;

			series.Title = points.Label;
			series.Color = points.Color.OxyColorFromString();
			series.MarkerType = (MarkerType)((int)points.Symbol);
			series.MarkerFill = series.Color;
			series.MarkerSize = 3.0;
			series.MarkerStroke = series.Color;
			series.MarkerStrokeThickness = 1.0;
		}

		protected void Refresh()
		{
			if (Model.PlotControl != null)
				Model.RefreshPlot(false);
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
