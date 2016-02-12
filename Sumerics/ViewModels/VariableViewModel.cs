namespace Sumerics.ViewModels
{
    using System;
    using System.Windows.Media.Imaging;
    using YAMP;

    public sealed class VariableViewModel : BaseViewModel
	{
		#region Fields

		String _type;
        String _shortInfo;
		Value _value;
        BitmapImage _icon;

		#endregion

		#region ctor

		public VariableViewModel(String name, Value value)
        {
            Name = name;
            Value = value;
		}

		#endregion

		#region Properties

		public String Name
        {
            get;
            private set;
        }

        public String Type
        {
            get { return _type; }
            set
            {
                _type = value;
                RaisePropertyChanged();
            }
        }

        public BitmapImage Icon
        {
            get { return _icon; }
            set 
            { 
                _icon = value; 
                RaisePropertyChanged();
            }
        }

        public String ShortInfo
        {
            get { return _shortInfo; }
            set
            {
                _shortInfo = value;
                RaisePropertyChanged();
            }
        }

        public Value Value
        {
            get { return _value; }
            set
            {
                this._value = value;
                Type = value.Header;
                Icon = Icons.GetVariableImage(value);

				switch (Type)
				{
					case "Scalar":
						ShortInfo = "[1, 1]";
						break;
					case "Range":
					case "Matrix":
						{
							var m = value as MatrixValue;
                            ShortInfo = String.Format("[{0}, {1}]", m.DimensionY, m.DimensionX);
						}
						break;
					case "String":
						{
							var s = value as StringValue;
                            ShortInfo = String.Format("[{0}, 1]", s.Length);
						}
						break;
					case "Arguments":
						{
							var a = value as ArgumentsValue;
                            ShortInfo = String.Format("[{0}, 1]", a.Length);
						}
						break;
					case "PolarPlot":
						{
							var p = value as PolarPlotValue;
                            ShortInfo = String.Format("[α: {0}]", p.Count);
						}
						break;
					case "Plot2D":
						{
							var p = value as Plot2DValue;
                            ShortInfo = String.Format("[2D: {0}]", p.Count);
						}
						break;
					case "Plot3D":
						{
							var p3 = value as Plot3DValue;
							ShortInfo = String.Format("[3D: {0}", p3.Count);
						}
						break;
				}
            }
		}

		#endregion
    }
}
