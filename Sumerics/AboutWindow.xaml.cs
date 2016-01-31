namespace Sumerics
{
    using MahApps.Metro.Controls;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    //using YAMP.Sensors;

    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {
        #region Members

        int longitude;
        int latitude;
        int compass;

        //GPSFunction gps;
        //CompFunction cmp;

        #endregion

        #region ctor

        public AboutWindow()
        {
            InitializeComponent();
            var infos = PerformReflection();
            Version.Text = GetVersion(infos);
            Copyright.Text = GetCopyright(infos);

            if (Core.IsWindows8)
            {
                Loaded += WindowLoaded;
                Closed += WindowClosed;
            }
            
            SetPosition();
        }

        #endregion

        #region Events

        void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //gps = new GPSFunction();
            //gps.ReadingChanged += OnSensorChanged;
            //cmp = new CompFunction();
            //cmp.ReadingChanged += OnSensorChanged;
        }

        void WindowClosed(object sender, EventArgs e)
        {
            //gps.ReadingChanged -= OnSensorChanged;
            //cmp.ReadingChanged -= OnSensorChanged;
        }

        void OnSensorChanged(object sender, object e)
        {
            //longitude = (int)GPSFunction.Longitude;
            //latitude = (int)GPSFunction.Latitude;
            //compass = (int)CompFunction.HeadingMagneticNorth;
            //SetPosition();
        }

        #endregion

        #region UI Text

        void SetPosition()
        {
            Dispatcher.Invoke(() =>
            {
                Position.Text = string.Format("Your current position is {0}° {1}° with heading {2}° north.", longitude, latitude, compass);
            });
        }

        IDictionary<string, string> PerformReflection()
        {
            var values = new Dictionary<string, string>();
            var app = Assembly.GetExecutingAssembly();

            var title = app.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute;
            values.Add("Title", title.Title);

            var product = app.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0] as AssemblyProductAttribute;
            values.Add("Product", product.Product);

            var copyright = app.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute;
            values.Add("Copyright", copyright.Copyright);

            var company = app.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0] as AssemblyCompanyAttribute;
            values.Add("Company", company.Company);

            var description = app.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute;
            values.Add("Description", description.Description);

            var version = app.GetName().Version;
            values.Add("Version", version.ToString());

            return values;
        }

        string GetCopyright(IDictionary<string, string> infos)
        {
            return infos["Copyright"] + ", " + infos["Company"];
        }

        string GetVersion(IDictionary<string, string> infos)
        {
            return infos["Product"] + ", Version: " + infos["Version"];
        }

        #endregion
    }
}
