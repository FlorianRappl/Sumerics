namespace Sumerics
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using YAMP;

	sealed class SeriesViewModel : BaseViewModel
	{
		#region Fields

		PlotValue value;
		SeriesElementViewModel selected;
		ObservableCollection<SeriesElementViewModel> children;

		#endregion

		#region ctor

		public SeriesViewModel(XYPlotValue value, IContainer container)
            : base(container)
		{
			int index = 1;
			this.value = value;
			Children = new ObservableCollection<SeriesElementViewModel>();

			for (var i = 0; i < value.Count; i++)
			{
                var series = value.GetSeries(i);
				var child = new SeriesElementViewModel(series, index++, container);
				Children.Add(child);
			}

			SelectedItem = Children.FirstOrDefault();
		}

		#endregion

		#region Properties

		public SeriesElementViewModel SelectedItem
		{
			get { return selected; }
			set 
			{
				selected = value;
				RaisePropertyChanged("SelectedItem");
			}
		}

		public ObservableCollection<SeriesElementViewModel> Children
		{
			get
			{
				return children;
			}
			set
			{
				children = value;
				RaisePropertyChanged("Children");
			}
		}

		public ICommand SaveAndClose
		{
			get
			{
				return new RelayCommand(x =>
				{
					var window = x as Window;

					foreach (var series in Children)
						series.Save();

					value.UpdateProperties();
					window.Close();
				});
			}
		}

		#endregion
	}
}
