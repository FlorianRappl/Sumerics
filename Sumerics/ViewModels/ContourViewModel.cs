namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using YAMP;

	sealed class ContourViewModel : BaseViewModel
	{
		#region Fields

        readonly ContourPlotValue _value;
        readonly ICommand _save;
		String _title;
		Boolean _showLevels;
		ColorPalettes _colorPalette;

		#endregion

		#region ctor

		public ContourViewModel(ContourPlotValue value)
		{
			_value = value;
			_title = value[0].Label;
			_colorPalette = value.ColorPalette;
			_showLevels = value.ShowLevel;
            _save = new RelayCommand(x =>
            {
                var window = x as Window;
                _value.ColorPalette = _colorPalette;
                _value[0].Label = _title;
                _value.ShowLevel = _showLevels;
                _value.UpdateProperties();

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
                if (String.IsNullOrEmpty(value))
                {
                    _title = "Data";
                }
                else
                {
                    _title = value;
                }

				RaisePropertyChanged();
			}
		}

		public ColorPalettes ColorPalette
		{
			get { return _colorPalette; }
			set
			{
				_colorPalette = value;
				RaisePropertyChanged();
			}
		}

		public Boolean ShowLevels
		{
			get { return _showLevels; }
			set
			{
				_showLevels = value;
				RaisePropertyChanged();
			}
		}

		public IEnumerable<ColorPalettes> ColorPalettes
		{
			get { return Enum.GetValues(typeof(ColorPalettes)).Cast<ColorPalettes>(); }
		}

		public ICommand SaveAndClose
		{
			get { return _save; }
		}

		#endregion
	}
}
