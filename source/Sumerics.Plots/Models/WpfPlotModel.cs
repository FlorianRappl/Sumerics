namespace Sumerics.Plots.Models
{
    using OxyPlot;
    using System;

    public class WpfPlotModel : BasePlotModel
    {
        String _title;
        Boolean _axisShown;
        Plot3dAxis _xAxis;
        Plot3dAxis _yAxis;
        Plot3dAxis _zAxis;
        Double _thickness;
        OxyColor _color;

        public String Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        public Boolean IsAxisShown
        {
            get { return _axisShown; }
            set { _axisShown = value; RaisePropertyChanged(); }
        }

        public Plot3dAxis XAxis
        {
            get { return _xAxis; }
            set { _xAxis = value; RaisePropertyChanged(); }
        }

        public Plot3dAxis YAxis
        {
            get { return _yAxis; }
            set { _yAxis = value; RaisePropertyChanged(); }
        }

        public Plot3dAxis ZAxis
        {
            get { return _zAxis; }
            set { _zAxis = value; RaisePropertyChanged(); }
        }

        public Double Thickness
        {
            get { return _thickness; }
            set { _thickness = value; RaisePropertyChanged(); }
        }

        public OxyColor Color
        {
            get { return _color; }
            set { _color = value; RaisePropertyChanged(); }
        }
    }
}
