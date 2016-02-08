namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using YAMP;

	sealed class HeatmapViewModel : BaseViewModel
	{
		#region Fields

		HeatmapPlotValue value;
		String title;
		ColorPalettes colorPalette;

		#endregion

		#region ctor

        public HeatmapViewModel(HeatmapPlotValue value, IContainer container)
            : base(container)
		{
			this.value = value;
			this.title = value[0].Label;
			this.colorPalette = value.ColorPalette;
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
				if (string.IsNullOrEmpty(value))
					title = "Data";
				else
					title = value;

				RaisePropertyChanged("Title");
			}
		}

		public ColorPalettes ColorPalette
		{
			get
			{
				return colorPalette;
			}
			set
			{
				colorPalette = value;
				RaisePropertyChanged("ColorPalette");
			}
		}

		public IEnumerable<ColorPalettes> ColorPalettes
		{
			get
			{
				return Enum.GetValues(typeof(ColorPalettes)).Cast<ColorPalettes>();
			}
		}

		public ICommand SaveAndClose
		{
			get
			{
				return new RelayCommand(x =>
				{
					var window = x as Window;
					value.ColorPalette = colorPalette;
					value[0].Label = title;
					value.UpdateProperties();
					window.Close();
				});
			}
		}

		#endregion
	}
}
