namespace Sumerics.Plots.Models
{
    using System;

    public class Plot3dAxis : LiveModel
    {
        Double _min;
        Double _max;
        Boolean _log;

        public Double Minimum
        {
            get { return _min; }
            set { _min = value; RaisePropertyChanged(); }
        }

        public Double Maximum
        {
            get { return _max; }
            set { _max = value; RaisePropertyChanged(); }
        }

        public Boolean IsLogarithmic
        {
            get { return _log; }
            set { _log = value; RaisePropertyChanged(); }
        }
    }
}
