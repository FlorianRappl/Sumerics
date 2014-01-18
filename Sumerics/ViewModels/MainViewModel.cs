using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Sumerics.Controls;
using YAMP;
using YAMP.Help;

namespace Sumerics
{
    /// <summary>
    /// This is the main view model - the central point!
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        #region Members

        IPlotViewModel lastPlot;
        VariableViewModel selectedVariable;
        ObservableCollection<VariableViewModel> variables;
        ObservableCollection<HelpViewModel> functions;
		ObservableCollection<AutocompleteItem> availableItems;
        ObservableCollection<NotificationViewModel> notifications;

        bool hasNotification;
        string input;
        string functionFilter;
        string variableFilter;

        System.Timers.Timer popupTimer;

        #endregion

        #region ctor

        public MainViewModel()
        {
            popupTimer = new System.Timers.Timer();
            popupTimer.Interval = 5000;
            popupTimer.Elapsed += NotifyTimerElapsed;

            variables = new ObservableCollection<VariableViewModel>();
            functions = new ObservableCollection<HelpViewModel>();
			availableItems = new ObservableCollection<AutocompleteItem>();
            notifications = new ObservableCollection<NotificationViewModel>();

            Core.Load();

            Core.PlotCreated += PlotCreated;
            Core.VariableChanged += VariableChanged;
            Core.VariableCreated += VariableCreated;
            Core.VariableRemoved += VariableRemoved;
            Core.NotificationReceived += NotificationReceived;
            Core.UserInputRequired += UserInputRequired;

			FunctionFilter = string.Empty;
			VariableFilter = string.Empty;

			FillLists();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets if notifications are available.
        /// </summary>
        public bool HasNotification
        {
            get { return hasNotification; }
            set
            {
                hasNotification = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currently applied function (help) filter.
        /// </summary>
        public string FunctionFilter
        {
            get { return functionFilter; }
            set 
            { 
                functionFilter = value;
                Functions.Clear();
				var sections = Core.Help.Sections;

                foreach (var section in sections)
                {
                    if (section.Name.Contains(functionFilter))
                        Functions.Add(new HelpViewModel(section));
                }

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currently applied variable filter.
        /// </summary>
		public string VariableFilter
		{
			get { return variableFilter; }
			set
			{
				variableFilter = value;
				Variables.Clear();
				var variables = Core.Context.AllVariables;

				foreach (var variable in variables)
				{
					if (variable.Key.Contains(variableFilter))
						Variables.Add(new VariableViewModel(variable.Key, variable.Value));
				}

				RaisePropertyChanged();
			}
		}

        /// <summary>
        /// Gets or sets the available plugins.
        /// </summary>
		public List<PluginViewModel> Plugins
		{
			get { return Core.Plugins; }
		}

        /// <summary>
        /// Gets or sets the available notifications.
        /// </summary>
        public ObservableCollection<NotificationViewModel> Notifications
        {
            get { return notifications; }
            set
            {
                notifications = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the available autocomplete items.
        /// </summary>
		public ObservableCollection<AutocompleteItem> AvailableItems
		{
			get { return availableItems; }
			set
			{
				availableItems = value;
				RaisePropertyChanged();
			}
		}

        /// <summary>
        /// Gets or sets the variables contained in the current workspace.
        /// </summary>
		public ObservableCollection<VariableViewModel> Variables
		{
			get { return variables; }
			set
			{
				variables = value;
				RaisePropertyChanged();
			}
		}

        /// <summary>
        /// Gets or sets the functions available in the help.
        /// </summary>
        public ObservableCollection<HelpViewModel> Functions
        {
            get { return functions; }
            set
            {
                functions = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current input command.
        /// </summary>
        public string InputCommand
        {
            get { return input; }
            set
            {
                input = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected help.
        /// </summary>
		public HelpViewModel SelectedHelp
		{
			get { return null; }
			set
			{
				value.ShowMore.Execute(value);
				RaisePropertyChanged();
			}
		}

        /// <summary>
        /// Gets or sets the selected variable.
        /// </summary>
        public VariableViewModel SelectedVariable
        {
            get { return selectedVariable; }
            set
            {
                selectedVariable = value;
                RaisePropertyChanged();
                RaisePropertyChanged("SelectedValue");

                if (value != null && value.Value is PlotValue)
                    Core.Context.ChangeLastPlotTo((PlotValue)value.Value);
            }
        }

        /// <summary>
        /// Gets or sets the created plot or the currently selected plot.
        /// </summary>
        public IPlotViewModel LastPlot
        {
            get { return lastPlot; }
            set
            {
                lastPlot = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        public Value SelectedValue
        {
            get 
            {
                if(SelectedVariable == null)
                    return null;

                return SelectedVariable.Value; 
            }
        }

        #endregion

        #region Commands

        public ICommand OpenEditor
        {
            get
            {
                return new RelayCommand(x =>
                {
                    App.Window.OpenEditorWindow();
                });
            }
        }

        public ICommand OpenDocumentation
        {
            get
            {
                return new RelayCommand(x =>
                {
                    App.Window.OpenHelpWindow();
                });
            }
        }

        public ICommand WatchSamples
        {
            get
            {
                return new RelayCommand(x =>
                {
                    App.Window.OpenDocumentationWindow();
                });
            }
        }

        public ICommand ChangeDirectory
        {
            get
            {
                return new RelayCommand(x =>
                {
                    App.Window.OpenDirectoryWindow();
                });
            }
        }

        public ICommand LoadWorksheet
        {
            get
            {
                return new RelayCommand(x =>
                {
                    App.Window.OpenLoadWindow();
                });
            }
        }

        public ICommand SaveWorksheet
        {
            get
            {
                return new RelayCommand(x =>
                {
                    App.Window.OpenSaveWindow();
                });
            }
        }

        public ICommand RunQuery
        {
            get
            {
                return new RelayCommand(x =>
                {
                    var qrvm = x as QueryResultViewModel;
                    var query = qrvm.Query;
                    var newQuery = YCommand.TryCommand(query);

                    if (newQuery != null)
                        query = newQuery;

                    Core.RunAsync(qrvm, query);
                });
            }
        }

        #endregion

        #region Methods

        void FillLists()
		{
            foreach (var k in Parser.Keywords)
            {
                var item = new AutocompleteItem(k, "The " + k + " keyword.", Icons.KeywordIcon);
                EditorViewModel.BasicItems.Add(item);
                availableItems.Add(item);
            }

			foreach (var f in Core.Help.Sections)
			{
                if (f.Topic.Equals("Constant"))
                {
                    var item = new AutocompleteItem(f.Name, f.Description, Icons.ConstantIcon);
                    EditorViewModel.BasicItems.Add(item);
                    availableItems.Add(item);
                }
                else
                {
                    var item = new AutocompleteItem(f.Name, f.Description, Icons.FunctionIcon);
                    EditorViewModel.BasicItems.Add(item);
                    availableItems.Add(item);
                }
			}
		}

        #endregion

        #region Event-Handling

        void NotifyTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            popupTimer.Stop();
            Dispatcher.CurrentDispatcher.Invoke(() => HasNotification = false);
        }

        void VariableChanged(object sender, VariableEventArgs e)
        {
            for (var i = 0; i < variables.Count; i++)
            {
                if (variables[i].Name.Equals(e.Name))
                {
					variables[i].Value = e.Value;

                    if (SelectedVariable != null && e.Name == SelectedVariable.Name)
                        SelectedVariable = variables[i];

                    break;
                }
            }
        }

        void VariableCreated(object sender, VariableEventArgs e)
        {
            if (e.Name.Contains(variableFilter))
                variables.Add(new VariableViewModel(e.Name, e.Value));

            availableItems.Add(new AutocompleteItem(e.Name, "Variable", Icons.VariableIcon));
        }

        void VariableRemoved(object sender, VariableEventArgs e)
        {
            for (var i = 0; i < variables.Count; i++)
            {
                if (variables[i].Name.Equals(e.Name))
                {
                    variables.RemoveAt(i);
                    break;
                }
            }

            if (SelectedVariable != null && e.Name == SelectedVariable.Name)
                SelectedVariable = null;

            for (var k = 0; k < availableItems.Count; k++)
            {
                if (availableItems[k].Text == e.Name)
                {
                    availableItems.RemoveAt(k);
                    break;
                }
            }
        }

        void PlotCreated(object sender, PlotEventArgs e)
        {
            LastPlot = new PlotViewModel(e.Value);
        }

        void UserInputRequired(object sender, UserInputEventArgs e)
        {
            var input = new InputDialog();
            input.UserMessage = e.Message;
            input.Closed += (s, ev) => { e.Continue(input.UserInput); };
            input.Show();
        }

        void NotificationReceived(object sender, NotificationEventArgs e)
        {
            notifications.Insert(0, new NotificationViewModel(e));
            HasNotification = true;
            popupTimer.Stop();
            popupTimer.Start();
        }

        #endregion
	}
}
