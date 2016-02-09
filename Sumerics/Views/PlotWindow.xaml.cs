using MahApps.Metro.Controls;
using Sumerics.Controls;
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
using System.Windows.Shapes;
using YAMP;

namespace Sumerics
{
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
            var window = new PlotWindow();
            window.PlotModel = plot;
            window.Show();
        }
    }
}
