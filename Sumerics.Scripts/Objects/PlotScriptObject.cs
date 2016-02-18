namespace Sumerics.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

	public class PlotScriptObject : AbstractScriptObject
    {
        readonly IEnumerable<String> _functions;
		String _variable;

		public PlotScriptObject(IEnumerable<String> functions)
		{
            _functions = functions;
			Title = "Plots";
			InputConnectors = 2;
		}

		protected override AbstractScriptObject CreateCopy()
		{
            return new PlotScriptObject(_functions)
            {
			    Title = "Choose a plot method",
			    _variable = _variable
            };
		}

		public String PlotFunction
		{
			get { return _variable; }
			set
			{
				_variable = value;
				RaisePropertyChanged("PlotFunction");
			}
		}

		protected override UIElement CreateContent()
		{
            var cb = new ComboBox
            {
			    Height = 20,
			    DataContext = this,
			    Margin = new Thickness(5),
			    IsEditable = false,
			    ItemsSource = _functions
            };

            if (cb.Items.Count > 0)
            {
                cb.SelectedIndex = 0;
            }

			var binding = new Binding("PlotFunction")
            {
			    Mode = BindingMode.TwoWay,
			    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

			cb.SetBinding(ComboBox.SelectedItemProperty, binding);
			return cb;
		}
	}
}
