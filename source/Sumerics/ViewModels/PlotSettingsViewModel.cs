namespace Sumerics.ViewModels
{
    using Sumerics.Controls;
    using Sumerics.Views;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media;
    using YAMP;

	public sealed class PlotSettingsViewModel : BaseViewModel
	{
		#region Fields

        readonly XYPlotValue _value;
        readonly ICommand _save;
		String _title;
		Boolean _isLegendVisible;
		SolidColorBrush _legendBackground;
		SolidColorBrush _legendBorder;
        LegendPosition _position;

		#endregion

		#region ctor

		public PlotSettingsViewModel(XYPlotValue value)
		{
			_value = value;
			Title = value.Title;
			IsLegendVisible = value.ShowLegend;
			LegendBackground = value.LegendBackground.BrushFromString();
			LegendBorder = value.LegendLineColor.BrushFromString();
            Position = value.LegendPosition;
            _save = new RelayCommand(x =>
            {
                var window = x as PlotSettingsWindow;
                _value.Title = _title;
                _value.LegendPosition = _position;
                _value.ShowLegend = _isLegendVisible;
                _value.LegendLineColor = _legendBorder.ToHtml();
                _value.LegendBackground = _legendBackground.ToHtml();
                _value.UpdateLayout();

                if (window != null)
                {
                    window.Close();
                }
            });
		}

		#endregion

        #region Properties

        public IEnumerable<LegendPosition> Positions
        {
            get { return Enum.GetValues(typeof(LegendPosition)).Cast<LegendPosition>(); }
        }

        public LegendPosition Position
        {
            get { return _position; }
            set
            {
                _position = value;
                RaisePropertyChanged();
            }
        }

		public String Title
		{
			get { return _title; }
			set
			{
				_title = value;
				RaisePropertyChanged();
			}
		}

		public Boolean IsLegendVisible
		{
			get { return _isLegendVisible; }
			set
			{
				_isLegendVisible = value;
				RaisePropertyChanged();
			}
		}

		public Brush LegendBackground
		{
			get { return _legendBackground; }
			set
			{
				_legendBackground = (SolidColorBrush)value;
				RaisePropertyChanged();
			}
		}

		public Brush LegendBorder
		{
			get { return _legendBorder; }
			set
			{
				_legendBorder = (SolidColorBrush)value;
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
