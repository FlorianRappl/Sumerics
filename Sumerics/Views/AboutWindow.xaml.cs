namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using System.Linq;
    using YAMP.Sensors;

    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {
        #region Fields

        Int32 longitude;
        Int32 latitude;
        Int32 compass;

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

            Loaded += WindowLoaded;
            Closed += WindowClosed;
            
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
                Position.Text = String.Format("Your current position is {0}° {1}° with heading {2}° north.", longitude, latitude, compass);
            });
        }

        static SumericsInfo PerformReflection()
        {
            var app = Assembly.GetExecutingAssembly();
            return new SumericsInfo
            {
                Title = app.GetCustomAttributes<AssemblyTitleAttribute>().First().Title,
                ProductName = app.GetCustomAttributes<AssemblyProductAttribute>().First().Product,
                Copyright = app.GetCustomAttributes<AssemblyCopyrightAttribute>().First().Copyright,
                Company = app.GetCustomAttributes<AssemblyCompanyAttribute>().First().Company,
                Description = app.GetCustomAttributes<AssemblyDescriptionAttribute>().First().Description,
                Version = app.GetName().Version.ToString()
            };
        }

        sealed class SumericsInfo
        {
            public String Title;
            public String ProductName;
            public String Copyright;
            public String Description;
            public String Company;
            public String Version;
        }

        static String GetCopyright(SumericsInfo infos)
        {
            return String.Concat(infos.Copyright, ", ", infos.Company);
        }

        static String GetVersion(SumericsInfo infos)
        {
            return String.Concat(infos.ProductName, ", Version: ", infos.Version);
        }

        #endregion
    }
}
