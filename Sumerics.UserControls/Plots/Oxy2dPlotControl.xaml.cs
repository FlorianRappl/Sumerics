﻿namespace Sumerics.Controls.Plots
{
    using Sumerics.Plots;
    using System;
    using System.IO;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for Oxy2dPlotControl.xaml
    /// </summary>
    public partial class Oxy2dPlotControl : OxyPlotControl, IPlotControl
    {
        public Oxy2dPlotControl(I2dPlotController controller)
        {
            InitializeComponent();
            controller.Control = this;
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

        public void ToggleGrid()
        {
            throw new NotImplementedException();
        }

        public void PreviewMode()
        {
            throw new NotImplementedException();
        }

        public void CenterPlot()
        {
            throw new NotImplementedException();
        }

        public void ExportAsPng(Stream stream, Int32 width, Int32 height)
        {
            throw new NotImplementedException();
        }
        /*
        // HEATMAP:

        public override void RefreshData()
        {
            Model.Series.Clear();
            SetSeries(Model);
            Refresh();
        }

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                //var series = (HeatmapSeries)Model.Series[i];
                //UpdateSeries(series, data);
            }

            Refresh();
        }

        public override void RefreshProperties()
        {
            SetGeneralProperties(Model);
            UpdateProperties(Model);
            Refresh();
        }

        // ERRROR:

        public override void RefreshData()
        {
            Model.Series.Clear();
            SetSeries(Model);
            Refresh();
        }

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                var series = (LineSeries)Model.Series[i];
                UpdateLineSeries(series, data);
            }

            Refresh();
        }

        public override void RefreshProperties()
        {
            SetGeneralProperties(Model);
            UpdateProperties(Model);
            Refresh();
        }

        // CONTOUR:

        public override void PreviewMode()
        {
            base.AsPreview();

            foreach (ContourSeries cs in Model.Series)
            {
                cs.FontSize = 0;
                cs.LabelBackground = OxyColors.Transparent;
            }
        }

        public override void RefreshData()
        {
            Model.Series.Clear();
            SetSeries(Model);
            Refresh();
        }

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                var series = (ContourSeries)Model.Series[i];
                UpdateSeries(series, data);
            }

            Refresh();
        }

        public override void RefreshProperties()
        {
            SetGeneralProperties(Model);
            UpdateProperties(Model);
            Refresh();
        }

        // COMPLEX:

        public override void RefreshData()
        {
            Model.Series.Clear();
            SetSeries(Model);
            Refresh();
        }

        public override void RefreshSeries()
        {
            Refresh();
        }

        public override void RefreshProperties()
        {
            SetGeneralProperties(Model);
            UpdateProperties(Model);
            Refresh();
        }

        // POLAR:

        public override void RefreshData()
        {
            Model.Series.Clear();
            SetSeries(Model);
            Refresh();
        }

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                var series = (LineSeries)Model.Series[i];
                UpdateLineSeries(series, data);
            }

            Refresh();
        }

        public override void RefreshProperties()
        {
            SetGeneralProperties(Model);
            UpdateProperties(Model);
            Refresh();
        }

        // CLASSIC (2D):

        public override void RefreshData()
        {
            Model.Series.Clear();
            SetSeries(Model);
            Refresh();
        }

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                var series = (LineSeries)Model.Series[i];
                UpdateLineSeries(series, data);
            }

            Refresh();
        }

        public override void RefreshProperties()
        {
            SetGeneralProperties(Model);
            UpdateProperties(Model);
            Refresh();
        }
         */
    }
}