namespace Sumerics
{
    using System;
    using System.Windows.Media.Imaging;
    using YAMP;

    sealed class VariableViewModel : BaseViewModel
	{
		#region Fields

		String type;
        String shortInfo;
		Value value;
        BitmapImage icon;

		#endregion

		#region ctor

		public VariableViewModel(String name, Value value, IContainer container)
            : base(container)
        {
            Name = name;
            Value = value;
		}

		#endregion

		#region Properties

		public string Name
        {
            get;
            private set;
        }

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                RaisePropertyChanged();
            }
        }

        public BitmapImage Icon
        {
            get { return icon; }
            set 
            { 
                icon = value; 
                RaisePropertyChanged();
            }
        }

        public string ShortInfo
        {
            get { return shortInfo; }
            set
            {
                shortInfo = value;
                RaisePropertyChanged();
            }
        }

        public Value Value
        {
            get { return value; }
            set
            {
                this.value = value;
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
							ShortInfo = string.Format("[{0}, {1}]", m.DimensionY, m.DimensionX);
						}
						break;
					case "String":
						{
							var s = value as StringValue;
							ShortInfo = string.Format("[{0}, 1]", s.Length);
						}
						break;
					case "Arguments":
						{
							var a = value as ArgumentsValue;
							ShortInfo = string.Format("[{0}, 1]", a.Length);
						}
						break;
					case "PolarPlot":
						{
							var p = value as PolarPlotValue;
							ShortInfo = string.Format("[α: {0}]", p.Count);
						}
						break;
					case "Plot2D":
						{
							var p = value as Plot2DValue;
							ShortInfo = string.Format("[2D: {0}]", p.Count);
						}
						break;
					case "Plot3D":
						{
							var p3 = value as Plot3DValue;
							ShortInfo = string.Format("[3D: {0}", p3.Count);
						}
						break;
				}
            }
		}

		#endregion
    }
}
