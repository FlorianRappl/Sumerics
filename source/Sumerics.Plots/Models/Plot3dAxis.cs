namespace Sumerics.Plots.Models
{
    using System;

    public class Plot3dAxis : LiveModel
    {
        Double _min;
        Double _max;
        Double _start;
        Double _end;
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

        public Double Start
        {
            get { return _start; }
            set { _start = value; RaisePropertyChanged(); }
        }

        public Double End
        {
            get { return _end; }
            set { _end = value; RaisePropertyChanged(); }
        }

        public Boolean IsLogarithmic
        {
            get { return _log; }
            set { _log = value; RaisePropertyChanged(); }
        }

        public void Reset()
        {
            Start = _min;
            End = _max;
        }
    }
}
