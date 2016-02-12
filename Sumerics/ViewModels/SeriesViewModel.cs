namespace Sumerics.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using YAMP;

	sealed class SeriesViewModel : BaseViewModel
	{
		#region Fields

        readonly ObservableCollection<SeriesElementViewModel> _children;
		readonly PlotValue _value;
        readonly ICommand _save;
		SeriesElementViewModel _selected;

		#endregion

		#region ctor

		public SeriesViewModel(XYPlotValue value)
		{
			var index = 1;
			_value = value;
			_children = new ObservableCollection<SeriesElementViewModel>();

			for (var i = 0; i < value.Count; i++)
			{
                var series = value.GetSeries(i);
				var child = new SeriesElementViewModel(series, index++);
				_children.Add(child);
			}

			SelectedItem = Children.FirstOrDefault();

            _save = new RelayCommand(x =>
            {
                var window = x as Window;

                foreach (var series in Children)
                {
                    series.Save();
                }

                _value.UpdateProperties();

                if (window != null)
                {
                    window.Close();
                }
            }); 
		}

		#endregion

		#region Properties

		public SeriesElementViewModel SelectedItem
		{
			get { return _selected; }
			set 
			{
				_selected = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<SeriesElementViewModel> Children
		{
			get { return _children; }
		}

		public ICommand SaveAndClose
		{
			get { return _save; }
		}

		#endregion
	}
}
