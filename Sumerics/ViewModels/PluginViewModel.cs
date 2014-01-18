using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Sumerics
{
	class PluginViewModel : BaseViewModel
	{
		#region Members

		string description;
		string title;
		string company;
		string version;
		bool active;
		BitmapImage icon;
		bool custom;
		string fileName;

		#endregion

		#region ctor

		public PluginViewModel(Assembly assembly)
		{
			custom = false;
			active = true;
			InspectAssembly(assembly);
		}

		public PluginViewModel(Assembly assembly, string fileName)
		{
			custom = true;
			this.fileName = fileName;
			InspectAssembly(assembly);
		}

		#endregion

		#region Properties

		public BitmapImage Icon
		{
			get { return icon; }
			set
			{
				icon = value;
				RaisePropertyChanged("Icon");
			}
		}

		public string Description
		{
			get { return description; }
			set
			{
				description = value;
				RaisePropertyChanged("Description");
			}
		}

		public string Title
		{
			get { return title; }
			set
			{
				title = value;
				RaisePropertyChanged("Title");
			}
		}

		public string Company
		{
			get { return company; }
			set
			{
				company = value;
				RaisePropertyChanged("Company");
			}
		}

		public string Version
		{
			get { return version; }
			set
			{
				version = value;
				RaisePropertyChanged("Version");
			}
		}

		public bool Active
		{
			get { return active; }
			set
			{
				active = value;
				RaisePropertyChanged("Active");
				ActiveChanged();
			}
		}

		public bool Custom
		{
			get { return custom; }
			set
			{
				custom = value;
				RaisePropertyChanged("Custom");
			}
		}

		#endregion

		#region Methods

		void ActiveChanged()
		{
			if (custom && !string.IsNullOrEmpty(fileName))
			{
				var settings = Properties.Settings.Default;

				if (settings.ActivePlugins == null)
					settings.ActivePlugins = new System.Collections.Specialized.StringCollection();

				if (active)
				{
					if (!settings.ActivePlugins.Contains(fileName))
					{
						settings.ActivePlugins.Add(fileName);
						settings.Save();
					}
				}
				else
				{
					if (settings.ActivePlugins.Contains(fileName))
					{
						settings.ActivePlugins.Remove(fileName);
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

			if (icon == null)
				icon = new BitmapImage(new Uri(@"..\Icons\plugin.png", UriKind.Relative));
		}

		void SetIcon(Assembly assembly, string name)
		{
			var stream = assembly.GetManifestResourceStream(name);

			if (stream != null)
			{
				icon = new BitmapImage();
				icon.BeginInit();
				icon.StreamSource = stream;
				icon.EndInit();
			}
		}

		T GetAttr<T>(Assembly assembly) where T : Attribute
		{
			return assembly.GetCustomAttributes(typeof(T), false)[0] as T;
		}

		#endregion
	}
}
