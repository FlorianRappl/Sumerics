namespace Sumerics
{
    using System;
    using System.Reflection;
    using System.Linq;

    sealed class SumericsInfo
    {
        public static SumericsInfo FromCurrentAssembly()
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

        public String Title { get; set; }

        public String ProductName { get; set; }

        public String Copyright { get; set; }

        public String Description { get; set; }

        public String Company { get; set; }

        public String Version { get; set; }
    }
}
