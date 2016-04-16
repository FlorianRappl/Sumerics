namespace Sumerics.ViewModels
{
    using Sumerics.Api;
    using Sumerics.Commands;
    using Sumerics.Controls;
    using Sumerics.MathInput;
    using Sumerics.Resources;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using YAMP;

    /// <summary>
    /// This is the main view model - the central point!
    /// </summary>
    public sealed class MainViewModel : BaseViewModel
    {
        #region Fields

        readonly ICommand _openDialog;
        readonly ICommand _runQuery;
        readonly IApplication _container;
        readonly Kernel _kernel;
        readonly ObservableCollection<VariableViewModel> _variables;
        readonly ObservableCollection<HelpViewModel> _functions;
        readonly ObservableCollection<AutocompleteItem> _availableItems;
        readonly NotificationsViewModel _notifications;

        VariableViewModel _selectedVariable;
        PlotViewModel _lastPlot;

        String _input;
        String _functionFilter;
        String _variableFilter;

        #endregion

        #region ctor

        public MainViewModel(IApplication container)
        {
            var provider = container.Get<IApiProvider>();

            _container = container;
            _kernel = container.Get<Kernel>();

            _variables = new ObservableCollection<VariableViewModel>();
            _functions = new ObservableCollection<HelpViewModel>();
			_availableItems = new ObservableCollection<AutocompleteItem>();
            _notifications = new NotificationsViewModel();

            _kernel.Load(provider);
            _kernel.Context.LastPlotChanged += PlotCreated;
            _kernel.Context.VariableChanged += VariableChanged;
            _kernel.Context.VariableCreated += VariableCreated;
            _kernel.Context.VariableRemoved += VariableRemoved;
            _kernel.Context.NotificationReceived += _notifications.Received;
            _kernel.Context.UserInputRequired += UserInput;

			FunctionFilter = String.Empty;
			VariableFilter = String.Empty;

			FillLists();

            _openDialog = new RelayCommand(x =>
            {
                var dialog = (Dialog)x;
                Container.Get<IDialogManager>().Open(dialog);
            });

            _runQuery = new RelayCommand(x =>
            {
                var cqr = x as ConsoleQueryReference;
                var qrvm = new QueryResultViewModel(cqr, _kernel.Context);
                var query = cqr.OriginalQuery;
                var newQuery = Container.Get<ICommandFactory>().TryCommand(query);

                if (newQuery != null)
                {
                    query = newQuery;
                }

                var task = _kernel.RunAsync(qrvm, query);
            });
        }

        #endregion

        #region Properties

        public IApplication Container
        {
            get { return _container; }
        }

        public String FunctionFilter
        {
            get { return _functionFilter; }
            set 
            { 
                _functionFilter = value;
                Functions.Clear();
				var sections = _kernel.Help.Sections;

                foreach (var section in sections)
                {
                    if (section.Name.Contains(_functionFilter))
                    {
                        var dialogs = Container.Get<IDialogManager>();
                        Functions.Add(new HelpViewModel(section, dialogs));
                    }
                }

                RaisePropertyChanged();
            }
        }

		public String VariableFilter
		{
			get { return _variableFilter; }
			set
			{
				_variableFilter = value;
				Variables.Clear();
				var variables = _kernel.Context.AllVariables;

				foreach (var variable in variables)
				{
                    if (variable.Key.Contains(_variableFilter))
                    {
                        var vm = new VariableViewModel(variable.Key, variable.Value);
                        Variables.Add(vm);
                    }
				}

				RaisePropertyChanged();
			}
		}

		public List<PluginViewModel> Plugins
		{
			get { return _kernel.Plugins; }
		}

        public NotificationsViewModel Notifications
        {
            get { return _notifications; }
        }

		public ObservableCollection<AutocompleteItem> AvailableItems
		{
			get { return _availableItems; }
		}

		public ObservableCollection<VariableViewModel> Variables
		{
			get { return _variables; }
		}

        public ObservableCollection<HelpViewModel> Functions
        {
            get { return _functions; }
        }

        public PlotViewModel LastPlot
        {
            get { return _lastPlot; }
            set { _lastPlot = value; RaisePropertyChanged(); }
        }

        public String InputCommand
        {
            get { return _input; }
            set { _input = value; RaisePropertyChanged(); }
        }

		public HelpViewModel SelectedHelp
		{
			get { return null; }
			set { value.ShowMore.Execute(value); RaisePropertyChanged(); }
		}

        public VariableViewModel SelectedVariable
        {
            get { return _selectedVariable; }
            set
            {
                _selectedVariable = value;
                RaisePropertyChanged();
                RaisePropertyChanged("SelectedValue");

                if (value != null && value.Value is PlotValue)
                {
                    _kernel.Context.ChangeLastPlotTo((PlotValue)value.Value);
                }
            }
        }

        public Value SelectedValue
        {
            get 
            {
                if (SelectedVariable == null)
                {
                    return null;
                }

                return SelectedVariable.Value; 
            }
        }

        #endregion

        #region Commands

        public ICommand OpenDialog
        {
            get { return _openDialog; }
        }

        public ICommand RunQuery
        {
            get { return _runQuery; }
        }

        #endregion

        #region Methods

        public void OpenEditor(IEnumerable<String> files)
        {
            var dialogs = Container.Get<IDialogManager>();

            foreach (var file in files)
            {
                dialogs.Open(Dialog.Editor, file);
            }
        }

        void FillLists()
		{
            foreach (var k in _kernel.Parser.Keywords)
            {
                var message = String.Format(Messages.Keyword, k);
                var item = new AutocompleteItem(k, message, IconFactory.KeywordIcon);
                EditorViewModel.BasicItems.Add(item);
                _availableItems.Add(item);
            }

			foreach (var f in _kernel.Help.Sections)
			{
                if (f.Topic.Equals("Constant"))
                {
                    var item = new AutocompleteItem(f.Name, f.Description, IconFactory.ConstantIcon);
                    EditorViewModel.BasicItems.Add(item);
                    _availableItems.Add(item);
                }
                else
                {
                    var item = new AutocompleteItem(f.Name, f.Description, IconFactory.FunctionIcon);
                    EditorViewModel.BasicItems.Add(item);
                    _availableItems.Add(item);
                }
			}
		}

        #endregion

        #region Event-Handling

        void VariableChanged(Object sender, VariableEventArgs e)
        {
            Dispatch(() =>
            {
                for (var i = 0; i < _variables.Count; i++)
                {
                    if (_variables[i].Name.Equals(e.Name))
                    {
                        _variables[i].Value = e.Value;

                        if (SelectedVariable != null && e.Name == SelectedVariable.Name)
                        {
                            SelectedVariable = _variables[i];
                        }

                        break;
                    }
                }
            });
        }

        void VariableCreated(Object sender, VariableEventArgs e)
        {
            Dispatch(() =>
            {
                if (e.Name.Contains(_variableFilter))
                {
                    var vm = new VariableViewModel(e.Name, e.Value);
                    _variables.Add(vm);
                }

                _availableItems.Add(new AutocompleteItem(e.Name, Messages.Variable, IconFactory.VariableIcon));
            });
        }

        void VariableRemoved(Object sender, VariableEventArgs e)
        {
            Dispatch(() =>
            {
                for (var i = 0; i < _variables.Count; i++)
                {
                    if (_variables[i].Name.Equals(e.Name))
                    {
                        _variables.RemoveAt(i);
                        break;
                    }
                }

                if (SelectedVariable != null && e.Name == SelectedVariable.Name)
                {
                    SelectedVariable = null;
                }

                for (var k = 0; k < _availableItems.Count; k++)
                {
                    if (_availableItems[k].Text == e.Name)
                    {
                        _availableItems.RemoveAt(k);
                        break;
                    }
                }
            });
        }

        void UserInput(Object sender, UserInputEventArgs e)
        {
            Dispatch(() =>
            {
                var service = Container.Get<IMathInputService>();
                var context = new InputViewModel(service);

                if (!String.IsNullOrEmpty(e.Message))
                {
                    context.UserMessage = e.Message;
                }

                var window = WindowFactory.Instance.Create(context);
                window.Closed += (s, ev) => e.Continue(context.Result);
                window.Show();
            });
        }

        void PlotCreated(Object sender, PlotEventArgs e)
        {
            Dispatch(() =>
            {
                var visualizer = Container.Get<IVisualizer>();
                visualizer.Show(e.Value);
            });
        }

        #endregion
    }
}
