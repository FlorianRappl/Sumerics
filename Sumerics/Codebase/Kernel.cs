namespace Sumerics
{
    using Sumerics.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using YAMP;
    using YAMP.Exceptions;
    using YAMP.Help;
    using YAMP.Io;
    using YAMP.Physics;
    using YAMP.Sensors;

    public sealed class Kernel : IKernel
    {
        #region Fields

        readonly Parser _parser;
        readonly List<PluginViewModel> _plugins;
        readonly Documentation _documentation;
        readonly ILogger _logger;

        #endregion

        #region ctor

        public Kernel(ILogger logger)
        {
            _logger = logger;
            _parser = new Parser { UseScripting = true };
            _plugins = new List<PluginViewModel>();

            LoadPlugins();
            _documentation = Documentation.Create(Context);

            Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            ExecuteGlobalScript();
            ExecuteLocalScript();

            _parser.InteractiveMode = true;
        }

        #endregion

        #region Properties

        public Boolean IsWindows8
        {
            get { return Environment.OSVersion.Version >= new Version(6, 2); }
        }

        public Documentation Help
        {
            get { return _documentation; }
        }

        public Parser Parser
        {
            get { return _parser; }
        }

        public ParseContext Context
        {
            get { return _parser.Context; }
        }

        public List<PluginViewModel> Plugins
        {
            get { return _plugins; }
        }

        #endregion

        #region Files

        public static String ErrorLog
        {
            get
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return directory + @"\Sumerics\errors.log";
            }
        }

        public static String GlobalScript
        {
            get
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return directory + @"\Sumerics\profile.ys";
            }
        }

        public static String LocalScript
        {
            get
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return directory + @"\Sumerics\profile.ys";
            }
        }

        #endregion

        #region Methods

        public void StopAll()
        {
            if (QueryResultViewModel.HasRunningQueries)
            {
                foreach (var query in QueryResultViewModel.RunningQueries)
                {
                    query.Cancel();
                }
            }
        }

        public Task RunAsync(String query)
        {
            return Task.Run(() => _parser.Evaluate(query.Replace("\r\n", "\n")));
        }

        public async Task RunAsync(QueryResultViewModel state, String query)
        {
            if (String.IsNullOrEmpty(state.Query))
            {
                state.Running = false;
                return;
            }

            try
            {
                var result = await Task.Run(() =>
                {
                    state.Thread = Thread.CurrentThread;
                    return _parser.Evaluate(query.Replace("\r\n", "\n"));
                });
                state.Running = false;

                //if (!result.IsMuted)
                {
                    state.Value = result;
                }
            }
            catch (YAMPParseException ex)
            {
                var error = ex.Errors.FirstOrDefault();

                if (error != null)
                {
                    state.Error = new YAMPException(error.Message);
                }
                else
                {
                    state.Error = ex;
                }
            }
            catch (YAMPRuntimeException ex)
            {
                state.Error = ex;
            }
            catch (ThreadAbortException)
            {
                state.Error = new Exception("The computation has been aborted.");
            }
            catch (Exception ex)
            {
                state.Error = ex;
                _logger.Error(ex);
            }
        }

        public Task LoadWorkspaceAsync(String fileName)
        {
            return Task.Run(() => Context.Load(fileName));
        }

        public Task SaveWorkspaceAsync(String fileName)
        {
            return Task.Run(() => Context.Save(fileName));
        }

        #endregion

        #region Plugins

        void LoadPlugins()
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

        void LoadPlugin(String assemblyPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                var model = new PluginViewModel(assembly, assemblyPath);
                model.Active = PluginActive(assemblyPath);
                _plugins.Add(model);

                if (model.Active)
                {
                    _parser.LoadPlugin(assembly);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void LoadPlugin(Assembly assembly)
        {
            _plugins.Add(new PluginViewModel(assembly));
            _parser.LoadPlugin(assembly);
        }

        Boolean PluginActive(String path)
        {
            if (Properties.Settings.Default.ActivePlugins != null)
            {
                return Properties.Settings.Default.ActivePlugins.Contains(path);
            }

            return false;
        }

        #endregion

        #region Script execution

        void ExecuteLocalScript()
        {
            var path = LocalScript;

            if (File.Exists(path))
            {
                var script = File.ReadAllText(path);

                try { _parser.Evaluate(script); }
                catch (Exception ex) { _logger.Error(ex); }
            }
        }

        void ExecuteGlobalScript()
        {
            var path = GlobalScript;

            if (File.Exists(path))
            {
                var script = File.ReadAllText(path);

                try { _parser.Evaluate(script); }
                catch (Exception ex) { _logger.Error(ex); }
            }
        }

        #endregion
    }
}
