﻿namespace Sumerics
{
    using MahApps.Metro.Controls;
    using YAMP;

	/// <summary>
	/// Interaction logic for PlotSeriesWindow.xaml
	/// </summary>
	public partial class PlotSeriesWindow : MetroWindow
	{
		public PlotSeriesWindow(XYPlotValue value, IContainer container)
		{
			InitializeComponent();
            DataContext = new SeriesViewModel(value, container);
		}
	}
}