using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YAMP;

namespace Sumerics
{
	class HeatmapViewModel : BaseViewModel
	{
		#region Members

		HeatmapPlotValue value;
		string title;
		ColorPalettes colorPalette;

		#endregion

		#region ctor

        public HeatmapViewModel(HeatmapPlotValue value)
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
