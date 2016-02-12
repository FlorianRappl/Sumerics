namespace Sumerics
{
    using System;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Windows.Media.Imaging;

	public sealed class PluginViewModel : BaseViewModel
	{
        #region Fields

        readonly Boolean _custom;
        readonly String _fileName;

		String _description;
		String _title;
		String _company;
		String _version;
		Boolean _active;
		BitmapImage _icon;

		#endregion

		#region ctor

		public PluginViewModel(Assembly assembly)
		{
			_custom = false;
			_active = true;
			InspectAssembly(assembly);
		}

        public PluginViewModel(Assembly assembly, String fileName)
		{
			_custom = true;
			_fileName = fileName;
			InspectAssembly(assembly);
		}

		#endregion

		#region Properties

		public BitmapImage Icon
		{
			get { return _icon; }
			set
			{
				_icon = value;
				RaisePropertyChanged();
			}
		}

		public String Description
		{
			get { return _description; }
			set
			{
				_description = value;
				RaisePropertyChanged();
			}
		}

		public String Title
		{
			get { return _title; }
			set
			{
				_title = value;
				RaisePropertyChanged();
			}
		}

		public String Company
		{
			get { return _company; }
			set
			{
				_company = value;
				RaisePropertyChanged();
			}
		}

		public String Version
		{
			get { return _version; }
			set
			{
				_version = value;
				RaisePropertyChanged();
			}
		}

		public Boolean Active
		{
			get { return _active; }
			set
			{
				_active = value;
				RaisePropertyChanged();
				ActiveChanged();
			}
		}

		public Boolean Custom
		{
			get { return _custom; }
		}

		#endregion

		#region Methods

		void ActiveChanged()
		{
			if (_custom && !String.IsNullOrEmpty(_fileName))
			{
				var settings = Properties.Settings.Default;

                if (settings.ActivePlugins == null)
                {
                    settings.ActivePlugins = new StringCollection();
                }

				if (_active)
				{
					if (!settings.ActivePlugins.Contains(_fileName))
					{
						settings.ActivePlugins.Add(_fileName);
						settings.Save();
					}
				}
				else
				{
					if (settings.ActivePlugins.Contains(_fileName))
					{
						settings.ActivePlugins.Remove(_fileName);
						settings.Save();
					}
				}
			}
		}

		void InspectAssembly(Assembly assembly)
		{
			var ver = assembly.GetName().Version;
			var names = assembly.GetManifestResourceNames();
			Title = GetAttr<AssemblyTitleAttribute>(assembly).Title;
			Description = GetAttr<AssemblyDescriptionAttribute>(assembly).Description;
			Company = GetAttr<AssemblyCompanyAttribute>(assembly).Company;
			Version = string.Format("{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build);

			foreach (var name in names)
				if (name.EndsWith("icon.png"))
					SetIcon(assembly, name);

			if (_icon == null)
				_icon = new BitmapImage(new Uri(@"..\Icons\plugin.png", UriKind.Relative));
		}

		void SetIcon(Assembly assembly, string name)
		{
			var stream = assembly.GetManifestResourceStream(name);

			if (stream != null)
			{
				_icon = new BitmapImage();
				_icon.BeginInit();
				_icon.StreamSource = stream;
				_icon.EndInit();
			}
		}

		T GetAttr<T>(Assembly assembly) where T : Attribute
		{
			return assembly.GetCustomAttributes(typeof(T), false)[0] as T;
		}

		#endregion
	}
}
