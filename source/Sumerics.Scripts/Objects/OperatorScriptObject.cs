using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sumerics.Controls
{
	public class OperatorScriptObject : AbstractScriptObject
	{
		String _variable;

		public OperatorScriptObject()
		{
			Title = "Operators";
			InputConnectors = 2;
		}

		public String Operator
		{
			get { return _variable; }
			set
			{
				_variable = value;
				RaisePropertyChanged();
			}
		}

		static readonly String[] Operators = new[]
		{
			"+", "-", "*", "/", "\\", "%",
			"^", ":", ".*", "./", ".\\", ".^",
			">", "<", ">=", "<=", "~=", "=="
		};

		protected override AbstractScriptObject CreateCopy()
		{
            return new OperatorScriptObject
            {
                Title = "Select an operator",
                _variable = _variable
            };
		}

		protected override UIElement CreateContent()
		{
            var cb = new ComboBox
            {
			    Height = 20,
			    DataContext = this,
			    Margin = new Thickness(5),
			    IsEditable = false,
			    ItemsSource = Operators
            };

            if (cb.Items.Count > 0)
            {
                cb.SelectedIndex = 0;
            }

			var binding = new Binding("Operator")
            {
			    Mode = BindingMode.TwoWay,
			    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

			cb.SetBinding(ComboBox.SelectedItemProperty, binding);
			return cb;
		}
	}
}
