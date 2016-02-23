namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Plots;

    /// <summary>
    /// Interaction logic for PlotWindow.xaml
    /// </summary>
    public partial class PlotWindow : MetroWindow
    {
        private PlotWindow(IPlotController controller)
        {
            InitializeComponent();
            Plot.Controller = controller;
        }

        public IPlotController Controller
        {
            get { return Plot.Controller; } 
        }

        internal static void Show(IPlotController controller)
        {
            var window = new PlotWindow(controller);
            window.Show();
        }
    }
}
