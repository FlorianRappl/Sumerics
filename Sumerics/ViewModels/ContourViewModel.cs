﻿namespace Sumerics
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

		ContourPlotValue value;
		String title;
		Boolean showLevels;
		ColorPalettes colorPalette;

		#endregion

		#region ctor

		public ContourViewModel(ContourPlotValue value, IContainer container)
            : base(container)
		{
			this.value = value;
			this.title = value[0].Label;
			this.colorPalette = value.ColorPalette;
			this.showLevels = value.ShowLevel;
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

		public bool ShowLevels
		{
			get
			{
				return showLevels;
			}
			set
			{
				showLevels = value;
				RaisePropertyChanged("ShowLevels");
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
					value.ShowLevel = showLevels;
					value.UpdateProperties();
					window.Close();
				});
			}
		}

		#endregion
	}
}
