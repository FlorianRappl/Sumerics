﻿namespace Sumerics.Plots.Models
{
    using System;

    public class WpfPlotModel : BasePlotModel
    {
        String _title;
        Boolean _axisShown;
        Object _model;

        public Object Model
        {
            get { return _model; }
            set { _model = value; RaisePropertyChanged(); }
        }

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
    }
}
