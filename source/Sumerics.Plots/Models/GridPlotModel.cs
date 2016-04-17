namespace Sumerics.Plots.Models
{
    using System;

    public class GridPlotModel : BasePlotModel
    {
        Int32 _rows;
        Int32 _columns;
        String _title;
        SubplotModels _models;

        public Int32 Rows
        {
            get { return _rows; }
            set { _rows = value; RaisePropertyChanged(); }
        }

        public Int32 Columns
        {
            get { return _columns; }
            set { _columns = value; RaisePropertyChanged(); }
        }

        public SubplotModels Models
        {
            get { return _models; }
            set { _models = value; RaisePropertyChanged(); }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }
    }
}
