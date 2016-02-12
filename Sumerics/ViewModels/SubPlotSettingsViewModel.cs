namespace Sumerics
{
    using System;
    using System.Windows.Input;
    using YAMP;

    sealed class SubPlotSettingsViewModel : BaseViewModel
    {
        #region Fields

        readonly SubPlotValue _value;
        readonly RelayCommand _save;
        String _title;
        Int32 _columns;
        Int32 _rows;

        #endregion

        #region ctor

        public SubPlotSettingsViewModel(SubPlotValue plot)
        {
            _value = plot;
            _title = plot.Title;
            _columns = plot.Columns;
            _rows = plot.Rows;
            _save = new RelayCommand(x =>
            {
                var window = x as SubPlotSettingsWindow;
                _value.Title = _title;
                _value.Rows = _rows;
                _value.Columns = _columns;
                _value.UpdateLayout();

                if (window != null)
                {
                    window.Close();
                }
            });
        }

        #endregion

        #region Properties

        public String Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        public Int32 Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                RaisePropertyChanged();
            }
        }

        public Int32 Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SaveAndClose
        {
            get { return _save; }
        }

        #endregion
    }
}
