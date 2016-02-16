namespace Sumerics.Plots
{
    using System;

    public interface IPlotControl
    {
        void RefreshData();

        void RefreshProperties();

        void RefreshSeries();

        void ToggleGrid();

        void AsPreview();

        void CenterPlot();

        void ExportPlot(String fileName, Int32 width, Int32 height);
    }
}
