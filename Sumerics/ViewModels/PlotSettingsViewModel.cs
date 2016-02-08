namespace Sumerics
{
    using Sumerics.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media;
    using YAMP;

	sealed class PlotSettingsViewModel : BaseViewModel
	{
		#region Fields

		String title;
		Boolean isLegendVisible;
		SolidColorBrush legendBackground;
		SolidColorBrush legendBorder;
		XYPlotValue value;
        YAMP.LegendPosition position;

		#endregion

		#region ctor

		public PlotSettingsViewModel(XYPlotValue value, IContainer container)
            : base(container)
		{
			this.value = value;
			Title = value.Title;
			IsLegendVisible = value.ShowLegend;
			LegendBackground = value.LegendBackground.BrushFromString();
			LegendBorder = value.LegendLineColor.BrushFromString();
            Position = value.LegendPosition;
		}

		#endregion

        #region Properties

        public IEnumerable<YAMP.LegendPosition> Positions
        {
            get
            {
                return Enum.GetValues(typeof(YAMP.LegendPosition)).Cast<YAMP.LegendPosition>();
            }
        }

        public YAMP.LegendPosition Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                RaisePropertyChanged();
            }
        }

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

		public bool IsLegendVisible
		{
			get 
			{ 
				return isLegendVisible;
			}
			set
			{
				isLegendVisible = value;
				RaisePropertyChanged();
			}
		}

		public Brush LegendBackground
		{
			get
			{
				return legendBackground;
			}
			set
			{
				legendBackground = (SolidColorBrush)value;
				RaisePropertyChanged();
			}
		}

		public Brush LegendBorder
		{
			get
			{
				return legendBorder;
			}
			set
			{
				legendBorder = (SolidColorBrush)value;
				RaisePropertyChanged();
			}
		}

		public ICommand SaveAndClose
		{
			get
			{
				return new RelayCommand(x =>
				{
					var window = x as PlotSettingsWindow;
					value.Title = title;
                    value.LegendPosition = position;
					value.ShowLegend = isLegendVisible;
					value.LegendLineColor = legendBorder.ToHtml();
					value.LegendBackground = legendBackground.ToHtml();
					value.UpdateLayout();
					window.Close();
				});
			}
		}

		#endregion
	}
}
