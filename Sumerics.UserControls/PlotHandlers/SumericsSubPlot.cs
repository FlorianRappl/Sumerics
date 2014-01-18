using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YAMP;

namespace Sumerics.Controls
{
    class SumericsSubPlot : SumericsPlot
    {
        #region Members

        SubPlotValue plot;
        Grid plotGrid;
        Grid grid;
        TextBlock title;

        int oldCount;
        List<SumericsPlot> subplots;

        #endregion

        #region ctor

        public SumericsSubPlot(SubPlotValue plot) 
            : base(plot)
        {
            subplots = new List<SumericsPlot>();

            this.plot = plot;
            this.grid = SetupGrid();
            this.plotGrid = SetupPlotGrid(this.grid);
            this.title = SetupTitle(this.grid);

            RefreshProperties();
            RefreshSeries();
        }

        #endregion

        #region Initialization

        static Grid SetupGrid()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(1.0, GridUnitType.Auto)
            });
            grid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(1.0, GridUnitType.Star)
            });
            return grid;
        }

        static Grid SetupPlotGrid(Grid parent)
        {
            var grid = new Grid();
            parent.Children.Add(grid);
            Grid.SetRow(grid, 1);
            return grid;
        }

        static TextBlock SetupTitle(Grid parent)
        {
            var title = new TextBlock();
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.FontSize = 18.0;
            title.FontWeight = FontWeights.Bold;
            parent.Children.Add(title);
            Grid.SetRow(title, 0);
            return title;
        }

        #endregion

        #region Properties

        public override bool IsGridEnabled
        {
            get { return false; }
        }

        public override bool IsSeriesEnabled
        {
            get { return false; }
        }

        public override FrameworkElement Content
        {
            get { return grid; }
        }

        #endregion

        #region Methods

        public override void RenderToCanvas(Canvas canvas)
        {
            var printGrid = SetupGrid();
            printGrid.Width = canvas.ActualWidth;
            printGrid.Height = canvas.ActualHeight;
            var printPlotGrid = SetupPlotGrid(printGrid);
            var title = SetupTitle(printGrid);
            title.Text = this.title.Text;

            for (var i = 0; i < plot.Columns; i++)
                printPlotGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (var i = 0; i < plot.Rows; i++)
                printPlotGrid.RowDefinitions.Add(new RowDefinition());

            canvas.Children.Add(printGrid);
            canvas.UpdateLayout();

            for (var i = 0; i < plot.Count; i++)
            {
                var data = plot[i];
                var c = new Canvas();
                printPlotGrid.Children.Add(c);
                c.Width = data.ColumnSpan * printPlotGrid.ActualWidth / plot.Columns;
                c.Height = data.RowSpan * printPlotGrid.ActualHeight / plot.Rows;

                c.Measure(new Size(c.Width, c.Height));
                c.Arrange(new Rect(0, 0, c.Width, c.Height));

                Grid.SetRow(c, data.Row - 1);
                Grid.SetColumn(c, data.Column - 1);
                Grid.SetRowSpan(c, data.RowSpan);
                Grid.SetColumnSpan(c, data.ColumnSpan);

                subplots[i].RenderToCanvas(c);
                c.UpdateLayout();
            }
        }

        public override void CenterPlot()
        {
            //Nothing
        }

        public override void ToggleGrid()
        {
            //Nothing
        }

        public override void ExportPlot(string fileName, int width, int height)
        {
            using (var s = File.Create(fileName))
            {
                var bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
                var dv = new DrawingVisual();
                var vb = new VisualBrush(grid);

                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawRectangle(vb, null, new Rect(new Point(), new Size{
                        Height = height, Width = width
                    }));
                }

                bmp.Render(dv);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(s);
            }
        }

        public override void AsPreview()
        {
            IsPreview = true;

            foreach (var subplot in subplots)
                subplot.AsPreview();
        }

        public override void RefreshData()
        {
        }

        public override void RefreshSeries()
        {
            plotGrid.Children.Clear();
            subplots.Clear();

            for (var i = 0; i < plot.Count; i++)
            {
                var data = plot[i];
                var subplot = PlotFactory.Create(data.Plot);
                var ctrl = subplot.Content;
                plotGrid.Children.Add(ctrl);

                Grid.SetRow(ctrl, data.Row - 1);
                Grid.SetColumn(ctrl, data.Column - 1);
                Grid.SetRowSpan(ctrl, data.RowSpan);
                Grid.SetColumnSpan(ctrl, data.ColumnSpan);

                subplots.Add(subplot);
            }

            oldCount = plot.Count;
        }

        public override void RefreshProperties()
        {
            plotGrid.RowDefinitions.Clear();
            plotGrid.ColumnDefinitions.Clear();

            for (var i = 0; i < plot.Columns; i++)
                plotGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (var i = 0; i < plot.Rows; i++)
                plotGrid.RowDefinitions.Add(new RowDefinition());

            title.Text = plot.Title;

            if (oldCount != plot.Count)
                RefreshSeries();
        }

        #endregion
    }
}
