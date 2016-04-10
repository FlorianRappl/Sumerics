namespace Sumerics.Controls
{
    using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

	public class SensorScriptObject : AbstractScriptObject
    {
        readonly IEnumerable<String> _functions;
		String _variable;

		public SensorScriptObject(IEnumerable<String> functions)
		{
            _functions = functions;
			Title = "Sensors";
		}

		protected override AbstractScriptObject CreateCopy()
		{
            return new SensorScriptObject(_functions)
            {
			    Title = "Pick a sensor function",
			    _variable = _variable
            };
		}

		public String SensorFunction
		{
			get { return _variable; }
			set
			{
				_variable = value;
				RaisePropertyChanged("SensorFunction");
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

            var binding = new Binding("SensorFunction")
            {
			    Mode = BindingMode.TwoWay,
			    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

			cb.SetBinding(ComboBox.SelectedItemProperty, binding);
			return cb;
		}
	}
}
