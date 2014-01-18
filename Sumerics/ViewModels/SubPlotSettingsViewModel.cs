using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YAMP;

namespace Sumerics
{
    class SubPlotSettingsViewModel : BaseViewModel
    {
        #region Members

        SubPlotValue value;
        string title;
        int columns;
        int rows;

        #endregion

        #region ctor

        public SubPlotSettingsViewModel(SubPlotValue plot)
        {
            value = plot;
            title = plot.Title;
            columns = plot.Columns;
            rows = plot.Rows;
        }

        #endregion

        #region Properties

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }

        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
                RaisePropertyChanged();
            }
        }

        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SaveAndClose
        {
            get
            {
                return new RelayCommand(x =>
                {
                    var window = x as SubPlotSettingsWindow;
                    value.Title = title;
                    value.Rows = rows;
                    value.Columns = columns;
                    value.UpdateLayout();
                    window.Close();
                });
            }
        }

        #endregion
    }
}
