﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using IronPlot;
using IronPlot.Plotting3D;

namespace Sumerics
{
    class IronPlotExamples
    {
        /* INSERT THE FOLLOWING XAML INTO MAINWINDOW.XAML


                <TabItem Header="2D Plot">
                    <Grid>
                        <Grid.RowDefinitions>
                            <RowDefinition Height="Auto"></RowDefinition>
                            <RowDefinition Height="*"></RowDefinition>
                        </Grid.RowDefinitions>
                        <ironplot:Plot2D Name="plotMultipleAxes" Grid.Row="1" Visibility="Visible"/>
                    </Grid>
                </TabItem>
                <TabItem Header="2D equal axes">
                    <Grid>
                        <Grid.RowDefinitions>
                            <RowDefinition Height="Auto"></RowDefinition>
                            <RowDefinition Height="*"></RowDefinition>
                        </Grid.RowDefinitions>
                        <ironplot:Plot2D Name="equalAxesPlot" EqualAxes="True" Grid.Row="1" Visibility="Visible"/>
                    </Grid>
                </TabItem>
                <TabItem Header="2D with dates">
                    <Grid>
                        <Grid.RowDefinitions>
                            <RowDefinition Height="Auto"></RowDefinition>
                            <RowDefinition Height="*"></RowDefinition>
                        </Grid.RowDefinitions>
                        <ironplot:Plot2D Name="datesPlot" Grid.Row="1" Visibility="Visible"/>
                    </Grid>
                </TabItem>
                <TabItem Header="2D False Colour Plot">
                    <ironplot:Plot2D Name="falseColourPlot" Visibility="Visible"/>
                </TabItem>
                <TabItem Header="3D Plot">
                    <ironplot3d:Plot3D Name="surfacePlot" Visibility="Visible"/>
                </TabItem>
                <TabItem Header="2D Plot in ScrollViewer">
                    <ScrollViewer VerticalScrollBarVisibility="Auto" HorizontalScrollBarVisibility="Auto">
                        <ironplot:Plot2D Name="scrollViewerPlot" Visibility="Visible"/>
                    </ScrollViewer>
                </TabItem>
                <TabItem Header="2D Plot alignment">
                    <ironplot:Plot2DGrid Name="aligningGrid">
                        <Grid.RowDefinitions>
                            <RowDefinition Height="*" />
                            <RowDefinition Height="*" />
                        </Grid.RowDefinitions>
                        <ironplot:Plot2D Name="plot1" Grid.Row="0"></ironplot:Plot2D>
                        <ironplot:Plot2D Name="plot2" Grid.Row="1"></ironplot:Plot2D>
                    </ironplot:Plot2DGrid>
                </TabItem>
         
         
        AND ENABLE THE CODE FROM THE CONSTRUCTOR */


        public IronPlotExamples(MainWindow window)
        {
            /*
            Plot2DMultipleAxes(window.plotMultipleAxes);
            DatesPlot(window.datesPlot);
            EqualAxesPlot(window.equalAxesPlot);
            FalseColourPlot(window.falseColourPlot);
            SurfacePlot(window.surfacePlot);
            ScrollViewerPlot(window.scrollViewerPlot);
            */
        }

        void Plot2DMultipleAxes(Plot2D plotMultipleAxes)
        {
            // Example 2D plot:
            // First curve:
            Plot2DCurve curve1 = plotMultipleAxes.AddLine(new double[] { 1.2, 1.3, 2.8, 5.6, 1.9, -5.9 });
            curve1.Stroke = Brushes.Blue; 
            curve1.StrokeThickness = 1.5; 
            curve1.MarkersType = MarkersType.Square;
            // Second curve:
            int nPoints = 2000;
            double[] x = new double[nPoints];
            double[] y = new double[nPoints];
            Random rand = new Random();
            for (int i = 0; i < nPoints; ++i)
            {
                x[i] = i * 10.0 / (double)nPoints;
                y[i] = Math.Sin(x[i]) + 0.1 * Math.Sin(x[i] * 100);
            }
            Plot2DCurve curve2 = new Plot2DCurve(new Curve(x, y)) { QuickLine = "r-" };
            plotMultipleAxes.Children.Add(curve2);
            // Third curve:
            Plot2DCurve curve3 = new Plot2DCurve(new Curve(new double[] { 1, 3, 1.5, 7 }, new double[] { 4.5, 9.0, 3.2, 4.5 })) { StrokeThickness = 3.0, Stroke = Brushes.Green };
            curve3.QuickLine = "o";
            curve3.MarkersFill = Brushes.Blue;
            curve3.Title = "Test3";
            //curve3.QuickStrokeDash = QuickStrokeDash.Dash;
            plotMultipleAxes.Children.Add(curve3);
            // Can use Direct2D acceleration, but requires DirectX10 (Windows 7)
            //plotMultipleAxes.UseDirect2D = true; 
            //plotMultipleAxes.EqualAxes = true;


            // If you want to lose the gradient background:
            //plotMultipleAxes.Legend.Background = Brushes.White;
            //plotMultipleAxes.BackgroundPlot = Brushes.White;


            // Additional labels:
            //plotMultipleAxes.BottomLabel.Text = "Bottom label";
            //plotMultipleAxes.LeftLabel.Text = "Left label";
            plotMultipleAxes.FontSize = 14;
            plotMultipleAxes.Axes.XAxes[0].AxisLabel.Text = "Innermost X Axis";
            plotMultipleAxes.Axes.YAxes[0].AxisLabel.Text = "Innermost Y Axis";
            XAxis xAxisOuter = new XAxis(); YAxis yAxisOuter = new YAxis();
            xAxisOuter.AxisLabel.Text = "Added X Axis";
            yAxisOuter.AxisLabel.Text = "Added Y Axis";
            plotMultipleAxes.Axes.XAxes.Add(xAxisOuter);
            plotMultipleAxes.Axes.YAxes.Add(yAxisOuter);
            yAxisOuter.Position = YAxisPosition.Left;
            plotMultipleAxes.Axes.XAxes[0].FontStyle = plotMultipleAxes.Axes.YAxes[0].FontStyle = FontStyles.Oblique;
            //curve3.XAxis = xAxisOuter;
            curve3.YAxis = yAxisOuter;
            // Other alyout options to try:
            //plotMultipleAxes.Axes.EqualAxes = new AxisPair(plot1.Axes.XAxes.Bottom, plot1.Axes.YAxes.Left);
            //plotMultipleAxes.Axes.SetAxesEqual();
            //plotMultipleAxes.Axes.Height = 100;
            //plotMultipleAxes.Axes.Width = 500;
            //plotMultipleAxes.Axes.MinAxisMargin = new Thickness(200, 0, 0, 0);
            plotMultipleAxes.Axes.XAxes.Top.TickLength = 5;
            plotMultipleAxes.Axes.YAxes.Left.TickLength = 5;


            xAxisOuter.Min = 6.5e-5;
            xAxisOuter.Max = 7.3e-3;
            xAxisOuter.AxisType = AxisType.Log;
            plotMultipleAxes.Children.Add(new Plot2DCurve(new Curve(new double[] { 0.01, 10 }, new double[] { 5, 6 })) { XAxis = xAxisOuter });
            plotMultipleAxes.UseDirect2D = false;
        }

        void DatesPlot(Plot2D datesPlot)
        {
            DateTime start = new DateTime(2006, 4, 26);
            DateTime[] dates = Enumerable.Range(0, 100).Select(t => start.AddMonths(t)).ToArray();
            Random random = new Random();
            double[] values = Enumerable.Range(0, 100).Select(t => random.NextDouble()).ToArray();
            var curve = datesPlot.AddLine(dates, values);
            curve.XAxis.AxisType = AxisType.Date;
            datesPlot.UseDirect2D = false;
        }

        void EqualAxesPlot(Plot2D equalAxesPlot)
        {
            var x = Enumerable.Range(0, 1000).Select(t => (double)t * 10 / 1000);
            var y = x.Select(t => 5 * Math.Exp(-t * t / 5));
            equalAxesPlot.AddLine(x, y).Title = "Test";
            equalAxesPlot.Axes.YAxes.Left.FormatOverride = FormatOverrides.Currency;
            //equalAxesPlot.Axes.YAxes.Left.FormatOverride = value => value.ToString("N");
        }

        void FalseColourPlot(Plot2D falseColourPlot)
        {
            // Example false colour plot: 
            double[,] falseColour = new double[128, 128];
            for (int i = 0; i < 128; ++i)
                for (int j = 0; j < 128; ++j) falseColour[i, j] = i + j;
            falseColourPlot.AddFalseColourImage(falseColour);
            //falseColourPlot.Axes.XAxes.GridLines.Visibility = Visibility.Collapsed;
            //falseColourPlot.Axes.YAxes.GridLines.Visibility = Visibility.Collapsed;
        }

        void SurfacePlot(Plot3D surfacePlot)
        {
            // Example surface plot:
            int nx = 96;
            int ny = 96;
            var x2 = MathHelper.MeshGridX(Enumerable.Range(1, nx).Select(t => (double)t - nx / 2), ny);
            var y2 = MathHelper.MeshGridY(Enumerable.Range(1, ny).Select(t => (double)t - ny / 2), nx);
            var z2 = x2.Zip(y2, (u, v) => Math.Exp((-u * u - v * v) / 400)); // .NET4 method
            //var z2 = x2.Select(u => u * u);
            SurfaceModel3D surface = new SurfaceModel3D(x2, y2, z2, nx, ny);
            surface.Transparency = 5;
            surface.MeshLines = MeshLines.None;
            surface.SurfaceShading = SurfaceShading.Smooth;
            surfacePlot.Viewport3D.Models.Add(surface);
            //surfacePlot.LeftLabel.Text = "Left Label"; plot3.BottomLabel.Text = "Bottom Label";
        }

        void ScrollViewerPlot(Plot2D scrollViewerPlot)
        {
            Plot2DCurve curve = scrollViewerPlot.AddLine(new double[] { 1.2, 1.3, 2.8, 5.6, 1.9, -5.9 });
            scrollViewerPlot.Axes.Height = scrollViewerPlot.Axes.Width = 500;
            //scrollViewerPlot.Height = scrollViewerPlot.Width = 500;
            // Some events.
            //scrollViewerPlot.Axes.YAxes.Right.MouseEnter += new MouseEventHandler(Bottom_MouseEnter);
        }
    }
}
