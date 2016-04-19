namespace Sumerics.Controls.Plots
{
    using System;
    using System.IO;
    using System.Windows.Controls;

    public interface IPlotSerializer
    {
        void Print(PrintDialog printDialog, String title);

        void Save(FileStream fs, Int32 width, Int32 height);
    }
}
