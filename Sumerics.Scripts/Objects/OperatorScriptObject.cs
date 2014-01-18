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
		string variable;

		public OperatorScriptObject()
		{
			Title = "Operators";
			InputConnectors = 2;
		}

		public string Operator
		{
			get { return variable; }
			set
			{
				variable = value;
				RaisePropertyChanged("Operator");
			}
		}

		static readonly string[] operators = new string[]
		{
			"+", "-", "*", "/", "\\", "%",
			"^", ":", ".*", "./", ".\\", ".^",
			">", "<", ">=", "<=", "~=", "=="
		};

		protected override AbstractScriptObject CreateCopy()
		{
			var op =  new OperatorScriptObject();
			op.Title = "Select an operator";
			op.variable = variable;
			return op;
		}

		protected override UIElement CreateContent()
		{
			var cb = new ComboBox();

			cb.Height = 20;
			cb.DataContext = this;
			cb.Margin = new Thickness(5);
			cb.IsEditable = false;
			cb.ItemsSource = operators;

			if (cb.Items.Count > 0)
				cb.SelectedIndex = 0;

			var binding = new Binding("Operator");
			binding.Mode = BindingMode.TwoWay;
			binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			cb.SetBinding(ComboBox.SelectedItemProperty, binding);

			return cb;
		}
	}
}
