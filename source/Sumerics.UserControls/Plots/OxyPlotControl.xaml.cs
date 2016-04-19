namespace Sumerics.Controls.Plots
{
    using OxyPlot;
    using OxyPlot.Wpf;
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for OxyPlotControl.xaml
    /// </summary>
    public partial class OxyPlotControl : BasePlotControl
    {
        public OxyPlotControl()
        {
            InitializeComponent();
        }

        protected override void RenderToCanvas(Canvas canvas)
        {
            var width = (Int32)canvas.Width;
            var height = (Int32)canvas.Height;
            var bmp = PngExporter.ExportToBitmap(Plotter.Model, width, height, OxyColors.Transparent);
            canvas.Children.Add(new Image { Source = bmp });
        }
    }
}
