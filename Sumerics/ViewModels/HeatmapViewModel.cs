namespace Sumerics.ViewModels
{
    using Sumerics.Resources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using YAMP;

	public sealed class HeatmapViewModel : BaseViewModel
	{
		#region Fields

		readonly HeatmapPlotValue _value;
        readonly ICommand _save;
		String _title;
		ColorPalettes _colorPalette;

		#endregion

		#region ctor

        public HeatmapViewModel(HeatmapPlotValue value)
		{
			_value = value;
			_title = value[0].Label;
			_colorPalette = value.ColorPalette;
            _save = new RelayCommand(x =>
            {
                var window = x as Window;
                _value.ColorPalette = _colorPalette;
                _value[0].Label = _title;
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
                    _title = Messages.Data;
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
