namespace Sumerics.Controls.Plots
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for GridPlotControl.xaml
    /// </summary>
    public partial class GridPlotControl : BasePlotControl
    {
        public GridPlotControl()
        {
            InitializeComponent();
        }

        public void ToggleGrid()
        {
        }

        public void PreviewMode()
        {
        }

        public void CenterPlot()
        {
        }

        public void RefreshData()
        {
            throw new NotImplementedException();
        }

        public void RefreshProperties()
        {
            throw new NotImplementedException();
        }

        public void RefreshSeries()
        {
            throw new NotImplementedException();
        }

        public void ExportAsPng(Stream s, Int32 width, Int32 height)
        {
            var bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();
            var vb = new VisualBrush(Plots);

            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size
                {
                    Height = height,
                    Width = width
                }));
            }

            bmp.Render(dv);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(s);
        }

        /*
        public override void RenderToCanvas(Canvas canvas)
        {
            var printGrid = SetupGrid();
            printGrid.Width = canvas.ActualWidth;
            printGrid.Height = canvas.ActualHeight;
            var printPlotGrid = SetupPlotGrid(printGrid);
            var title = SetupTitle(printGrid);
            title.Text = _title.Text;

            for (var i = 0; i < _plot.Columns; i++)
            {
                printPlotGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (var i = 0; i < _plot.Rows; i++)
            {
                printPlotGrid.RowDefinitions.Add(new RowDefinition());
            }

            canvas.Children.Add(printGrid);
            canvas.UpdateLayout();

            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot[i];
                var c = new Canvas();
                printPlotGrid.Children.Add(c);
                c.Width = data.ColumnSpan * printPlotGrid.ActualWidth / _plot.Columns;
                c.Height = data.RowSpan * printPlotGrid.ActualHeight / _plot.Rows;

                c.Measure(new Size(c.Width, c.Height));
                c.Arrange(new Rect(0, 0, c.Width, c.Height));

                Grid.SetRow(c, data.Row - 1);
                Grid.SetColumn(c, data.Column - 1);
                Grid.SetRowSpan(c, data.RowSpan);
                Grid.SetColumnSpan(c, data.ColumnSpan);

                _subplots[i].RenderToCanvas(c);
                c.UpdateLayout();
            }
        }

        public void RefreshData()
        {
        }

        public void RefreshSeries()
        {
            _plotGrid.Children.Clear();
            _subplots.Clear();

            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot[i];
                var subplot = PlotFactory.Create(data.Plot);
                var ctrl = subplot.Content;
                _plotGrid.Children.Add(ctrl);

                Grid.SetRow(ctrl, data.Row - 1);
                Grid.SetColumn(ctrl, data.Column - 1);
                Grid.SetRowSpan(ctrl, data.RowSpan);
                Grid.SetColumnSpan(ctrl, data.ColumnSpan);

                _subplots.Add(subplot);
            }

            _oldCount = _plot.Count;
        }

        public void RefreshProperties()
        {
            _plotGrid.RowDefinitions.Clear();
            _plotGrid.ColumnDefinitions.Clear();

            for (var i = 0; i < _plot.Columns; i++)
            {
                _plotGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (var i = 0; i < _plot.Rows; i++)
            {
                _plotGrid.RowDefinitions.Add(new RowDefinition());
            }

            _title.Text = _plot.Title;

            if (_oldCount != _plot.Count)
            {
                RefreshSeries();
            }
        }
        */
    }
}
