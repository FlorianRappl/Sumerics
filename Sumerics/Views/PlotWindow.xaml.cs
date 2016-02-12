namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Controls;
    using Sumerics.ViewModels;

    /// <summary>
    /// Interaction logic for PlotWindow.xaml
    /// </summary>
    public partial class PlotWindow : MetroWindow
    {
        private PlotWindow()
        {
            InitializeComponent();
        }

        public IPlotViewModel PlotModel
        {
            get { return Plot.Data; } 
            set { Plot.Data = value; }
        }

        internal static void Show(PlotViewModel plot)
        {
            var window = new PlotWindow { PlotModel = plot };
            window.Show();
        }
    }
}
