namespace Sumerics
{
    using System;
    using System.IO;

    sealed class FileLogger : ILogger
    {
        public void Error(Exception exception)
        {
            var path = Kernel.ErrorLog;

            try
            {
                using (var file = File.AppendText(path))
                {
                    var dt = DateTime.Now;
                    var methName = String.Empty;

                    if (exception.TargetSite != null)
                    {
                        methName = exception.TargetSite.Name;
                    }

                    file.WriteLine("{0} [ {1}, {2} ] {3}",
                        dt.ToString("yyyy/MM/dd hh:mm:ss"), exception.Source, exception.Message, methName);
                }
            }
            catch { }
        }
    }
}
