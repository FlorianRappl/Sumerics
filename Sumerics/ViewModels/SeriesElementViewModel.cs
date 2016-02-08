namespace Sumerics
{
    using Sumerics.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using YAMP;

	sealed class SeriesElementViewModel : BaseViewModel
	{
		#region Fields

		Int32 index;
		String title;
		SolidColorBrush color;
		PointSymbol symbol;
		Boolean lines;
		IPointSeries series;

		#endregion

		#region ctor

		public SeriesElementViewModel(IPointSeries series, Int32 index, IContainer container)
            : base(container)
		{
			this.index = index;
			this.series = series;
			Title = series.Label;
			Color = series.Color.BrushFromString();
			Symbol = series.Symbol;
			Lines = series.Lines;
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
					title = "Data #" + index;
				else
					title = value;

				RaisePropertyChanged("Title");
			}
		}

		public Brush Color
		{
			get
			{
				return color;
			}
			set
			{
				color = (SolidColorBrush)value;
				RaisePropertyChanged("Color");
			}
		}

		public PointSymbol Symbol
		{
			get
			{
				return symbol;
			}
			set
			{
				symbol = value;
				RaisePropertyChanged("Symbol");
			}
		}

		public bool Lines
		{
			get
			{
				return lines;
			}
			set
			{
				lines = value;
				RaisePropertyChanged("Lines");
			}
		}

		public IEnumerable<PointSymbol> Symbols
		{
			get
			{
				return Enum.GetValues(typeof(PointSymbol)).Cast<PointSymbol>();
			}
		}

		#endregion

		#region Methods

		public void Save()
		{
			series.Label = title;
			series.Lines = lines;
			series.Symbol = symbol;
			series.Color = color.ToHtml();
		}

		#endregion
	}
}
