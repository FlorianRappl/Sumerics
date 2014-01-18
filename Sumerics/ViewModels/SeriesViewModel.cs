using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using YAMP;

namespace Sumerics
{
	class SeriesViewModel : BaseViewModel
	{
		#region Members

		PlotValue value;
		SeriesElementViewModel selected;
		ObservableCollection<SeriesElementViewModel> children;

		#endregion

		#region ctor

		public SeriesViewModel(XYPlotValue value)
		{
			int index = 1;
			this.value = value;
			Children = new ObservableCollection<SeriesElementViewModel>();

			for (var i = 0; i < value.Count; i++)
			{
				var child = new SeriesElementViewModel(value.GetSeries(i), index++);
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
