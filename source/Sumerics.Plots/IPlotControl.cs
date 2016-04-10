namespace Sumerics.Plots
{
    using System;
    using System.IO;

    public interface IPlotControl
    {
        void RefreshData();

        void RefreshProperties();

        void RefreshSeries();

        void ToggleGrid();

        void PreviewMode();

        void CenterPlot();

        void ExportAsPng(Stream stream, Int32 width, Int32 height);
    }
}
