namespace Sumerics.Controls.Plots
{
    /// <summary>
    /// Interaction logic for OxyPlotControl.xaml
    /// </summary>
    public partial class OxyPlotControl : BasePlotControl
    {
        public OxyPlotControl()
        {
            InitializeComponent();
        }

        /*
        public override void RenderToCanvas(Canvas canvas)
        {
            //var rc = new OxyPlot.Wpf.CanvasRenderContext(canvas);
            //model.Render(rc);
        }

        public override void ExportPlot(string fileName, int width, int height)
        {
            control.SaveBitmap(fileName, width, height, OxyColors.White);
        }
         */
    }
}
