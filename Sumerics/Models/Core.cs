namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using YAMP;
    using YAMP.Exceptions;
    using YAMP.Help;
    using YAMP.Io;
    using YAMP.Physics;
    using YAMP.Sensors;

    /// <summary>
    /// Contains the main YAMP core and libraries.
    /// </summary>
    static class Core
    {
        #region Events

        /// <summary>
        /// Is invoked once an internal YAMP plot is created.
        /// </summary>
        public static event EventHandler<PlotEventArgs> PlotCreated;

        /// <summary>
        /// Is invoked once an internal YAMP variable changes.
        /// </summary>
        public static event EventHandler<VariableEventArgs> VariableChanged;

        /// <summary>
        /// Is invoked once an internal YAMP variable is created.
        /// </summary>
        public static event EventHandler<VariableEventArgs> VariableCreated;

        /// <summary>
        /// Is invoked once an internal YAMP variable is removed.
        /// </summary>
        public static event EventHandler<VariableEventArgs> VariableRemoved;

        /// <summary>
        /// Is invoked if the YAMP parser notifies about something.
        /// </summary>
        public static event EventHandler<NotificationEventArgs> NotificationReceived;

        /// <summary>
        /// Is invoked once YAMP requires some user input.
        /// </summary>
        public static event EventHandler<UserInputEventArgs> UserInputRequired;

        #endregion

        #region Fields

        static Documentation documentation;

        static Parser parser = new Parser();
        static Dispatcher dispatcher = Application.Current.Dispatcher;
        static List<PluginViewModel> plugins = new List<PluginViewModel>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parser.
        /// </summary>
        public static Parser Parser
        {
            get { return parser; }
        }

        /// <summary>
        /// Gets the path to the error log.
        /// </summary>
        public static String ErrorLog
        {
            get
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return directory + @"\Sumerics\errors.log";
            }
        }

        /// <summary>
        /// Gets the path to the global startup script.
        /// </summary>
        public static String GlobalScript
        {
            get
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return directory + @"\Sumerics\profile.ys";
            }
        }

        /// <summary>
        /// Gets the path to the local (user bound) startup script.
        /// </summary>
        public static String LocalScript
        {
            get
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return directory + @"\Sumerics\profile.ys";
            }
        }

        /// <summary>
        /// Gets the status of the OS. Is it sensor capable (in theory)?
        /// </summary>
        public static Boolean IsWindows8
        {
            //Only load sensors if the OS is version is >= 6.2 [ Build 9200 ]
            get { return Environment.OSVersion.Version >= new Version(6, 2); }
        }

        /// <summary>
        /// Gets the status of the core. Is it ready?
        /// </summary>
        public static Boolean IsLoaded 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the documentation system.
        /// </summary>
        public static Documentation Help 
        {
            get { return documentation; } 
        }

        /// <summary>
        /// Gets the primary parse context of YAMP.
        /// </summary>
        public static ParseContext Context 
        {
            get { return parser.Context; } 
        }

        /// <summary>
        /// Gets the plugins loaded by the YAMP parse context.
        /// </summary>
        public static List<PluginViewModel> Plugins 
        { 
            get { return plugins; } 
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the YAMP core.
        /// </summary>
        public static void Load()
        {
            if (!IsLoaded)
            {
                parser.UseScripting = true;
                
                //TODO Add API
                LoadPlugins();

                Context.OnLastPlotChanged += RaisePlotCreated;
                Context.OnVariableChanged += RaiseVariableChanged;
                Context.OnVariableCreated += RaiseVariableCreated;
                Context.OnVariableRemoved += RaiseVariableRemoved;

                documentation = Documentation.Create(Context);

                Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                ExecuteGlobalScript();
                ExecuteLocalScript();

                parser.InteractiveMode = true;
                parser.NotificationReceived += RaiseNotificationReceived;
                parser.UserInputRequired += RaiseUserInputRequired;

                IsLoaded = true;
            }
        }

        /// <summary>
        /// Runs an arbitrary query asynchronously.
        /// </summary>
        /// <param name="query">The query to evaluate.</param>
        /// <returns>The query context.</returns>
        public static async Task<Value> RunAsync(String query)
        {
            return await Task.Run(() => parser.Evaluate(query.Replace("\r\n", "\n")));
        }

        /// <summary>
        /// Runs a query that is contained within a QueryResultViewModel async.
        /// </summary>
        /// <param name="qrvm">The query context to operate within.</param>
        /// <param name="query">The (maybe modified) query to execute.</param>
        public static async void RunAsync(QueryResultViewModel qrvm, String query)
        {
            if (query.Equals(String.Empty))
            {
                qrvm.Running = false;
                return;
            }

            try
            {
                var result = await Task.Run(() =>
                {
                    qrvm.Thread = Thread.CurrentThread;
                    return parser.Evaluate(query.Replace("\r\n", "\n"));
                });
                qrvm.Context = Context;
                qrvm.Running = false;

                //if (!result.IsMuted)
                {
                    qrvm.Value = result;
                }
            }
            catch (YAMPParseException ex)
            {
                var firsterr = ex.Errors.FirstOrDefault();

                if (firsterr != null)
                {
                    qrvm.Error = new YAMPException(firsterr.Message);
                }
                else
                {
                    qrvm.Error = ex;
                }
            }
            catch (YAMPRuntimeException ex)
            {
                qrvm.Error = ex;
            }
            catch (ThreadAbortException)
            {
                qrvm.Error = new Exception("The computation has been aborted.");
            }
            catch (Exception ex)
            {
                qrvm.Error = ex;
                LogError(ex);
            }
        }

        /// <summary>
        /// Loads some workspace from the specified file asynchronously.
        /// </summary>
        /// <param name="fileName">The path to the file.</param>
        public static async void LoadWorkspaceAsync(String fileName)
        {
            await Task.Run(() => Context.Load(fileName));
        }

        /// <summary>
        /// Loads the workspace to the specified file asynchronously.
        /// </summary>
        /// <param name="fileName">The path to the file.</param>
        public static async void SaveWorkspaceAsync(String fileName)
        {
            await Task.Run(() => Context.Save(fileName));
        }

        #endregion

        #region Internal Event Handling

        static void RaiseVariableCreated(object sender, VariableEventArgs e)
        {
            if (VariableCreated != null)
                dispatcher.InvokeAsync(() => VariableCreated(sender, e));
        }

        static void RaiseVariableRemoved(object sender, VariableEventArgs e)
        {
            if (VariableRemoved != null)
                dispatcher.InvokeAsync(() => VariableRemoved(sender, e));
        }

        static void RaiseVariableChanged(object sender, VariableEventArgs e)
        {
            if (VariableChanged != null)
                dispatcher.InvokeAsync(() => VariableChanged(sender, e));
        }

        static void RaisePlotCreated(object sender, PlotEventArgs e)
        {
            if (PlotCreated != null)
                dispatcher.InvokeAsync(() => PlotCreated(sender, e));
        }

        static void RaiseUserInputRequired(object sender, UserInputEventArgs e)
        {
            if (UserInputRequired != null)
                dispatcher.InvokeAsync(() => UserInputRequired(sender, e));
        }

        static void RaiseNotificationReceived(object sender, NotificationEventArgs e)
        {
            if (NotificationReceived != null)
                dispatcher.InvokeAsync(() => NotificationReceived(sender, e));
        }

        #endregion

        #region Plugins

        static void LoadPlugins()
        {
            LoadPlugin(typeof(IoPlugin).Assembly);
            LoadPlugin(typeof(PhysicsPlugin).Assembly);

            if (IsWindows8)
            {
                LoadPlugin(typeof(SensorsPlugin).Assembly);
            }

            var files = new String[0];

            try
            {
                if (Directory.Exists("Plugins"))
                {
                    files = Directory.GetFiles("Plugins", "*.dll");
                }
            }
            catch { }

            foreach (var file in files)
            {
                LoadPlugin(file);
            }
        }

        static void LoadPlugin(String assemblyPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                var model = new PluginViewModel(assembly, assemblyPath, null);//TODO
                model.Active = PluginActive(assemblyPath);
                plugins.Add(model);

                if (model.Active)
                {
                    parser.LoadPlugin(assembly);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        static void LoadPlugin(Assembly assembly)
        {
            plugins.Add(new PluginViewModel(assembly, null));//TODO
            parser.LoadPlugin(assembly);
        }

        static Boolean PluginActive(String path)
        {
            if (Properties.Settings.Default.ActivePlugins != null)
            {
                return Properties.Settings.Default.ActivePlugins.Contains(path);
            }

            return false;
        }

        #endregion

        #region Script execution

        /// <summary>
        /// Loads the local script (from the user's profile).
        /// </summary>
        static void ExecuteLocalScript()
        {
            var path = LocalScript;

            if (File.Exists(path))
            {
                var script = File.ReadAllText(path);

                try { parser.Evaluate(script); }
                catch (Exception ex) { LogError(ex); }
            }
        }

        /// <summary>
        /// Loads the global script (from the directory).
        /// </summary>
        static void ExecuteGlobalScript()
        {
            var path = GlobalScript;

            if (File.Exists(path))
            {
                var script = File.ReadAllText(path);

                try { parser.Evaluate(script); }
                catch (Exception ex) { LogError(ex); }
            }
        }

        /// <summary>
        /// Logs the error to the file "errors.log" in the directory of the application.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public static void LogError(Exception ex)
        {
            var path = ErrorLog;

            try
            {
                using (var file = File.AppendText(path))
                {
                    var dt = DateTime.Now;
                    var methName = string.Empty;

                    if (ex.TargetSite != null)
                        methName = ex.TargetSite.Name;

                    file.WriteLine("{0}/{1}/{2} {3}:{4}:{5} [ {6}, {8} ] {7}", 
                        dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, ex.Source, ex.Message, methName);
                }
            }
            catch { }
        }

        #endregion
    }
}
