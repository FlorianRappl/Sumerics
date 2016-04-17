namespace Sumerics.ViewModels
{
    using MahApps.Metro.Controls;
    using Sumerics.Commands;
    using Sumerics.Resources;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using YAMP.Help;

	public sealed class DocumentationViewModel : BaseViewModel
	{
		#region Fields

        readonly ObservableCollection<PanoramaGroup> _groups;
        readonly ObservableCollection<HelpSection> _results;
        readonly Documentation _documentation;
        readonly ICommandFactory _commands;
        readonly ICommand _home;
        readonly ICommand _back;
        readonly ICommand _more;
        readonly WebBrowser _browser;
        readonly IConsole _console;
        String _searchText;
        HelpSection _topic;
        String _breadCrumb;
        String _title;
        Visibility _info;
        Visibility _loading;
        Int32 _tab;
        FlowDocument _currentDocument;
        HelpSection _searchSelection;

		#endregion

		#region ctor

        public DocumentationViewModel(Documentation documentation, ICommandFactory commands, IConsole console)
		{
            _searchText = String.Empty;
            _breadCrumb = BuildBreadCrumb(Messages.Documentation, Messages.Overview);
            _groups = new ObservableCollection<PanoramaGroup>();
            _title = Messages.Help;
            _results = new ObservableCollection<HelpSection>();
            _documentation = documentation;
            _commands = commands;
            _console = console;
            _browser = new WebBrowser
            {
                Visibility = Visibility.Hidden,
                Source = new Uri("about:blank")
            };
            _browser.LoadCompleted += (s, e) => IsLoading = Visibility.Hidden;
            _browser.Navigated += (s, e) => Browser.SetSilent();
            _info = Visibility.Visible;
            _loading = Visibility.Visible;
            _home = new RelayCommand(_ =>
            {
                TabIndex = 0;
                Title = Messages.Help;
                BreadCrumb = BuildBreadCrumb(Messages.Documentation, Messages.Overview);
            });
            _back = new RelayCommand(_ => 
            {
                TabIndex = 1;
                Title = Messages.Help;
                BreadCrumb = BuildBreadCrumb(Messages.Documentation, Messages.Overview);
                IsLoading = Visibility.Visible;
            });
            _more = new RelayCommand(_ =>
            {
                TabIndex = 2;
                _browser.Navigate(_topic.Link);
            });

            foreach (var topic in _documentation.Topics)
            {
                var pg = new PanoramaGroup(topic.Kind);
                var content = new List<HelpTileViewModel>();

                foreach (var item in topic)
                {
                    var vm = new HelpTileViewModel(this, item);
                    content.Add(vm);
                }

                pg.SetSource(content);
                Groups.Add(pg);
            }
		}

		#endregion

        #region Properties

        public HelpSection SearchSelection
        {
            get { return _searchSelection; }
            set
            {
                _searchSelection = value;

                if (value != null)
                {
                    Topic = value;
                }
            }
        }

        public WebBrowser Browser
        {
            get { return _browser; }
        }

        public Documentation Help
        {
            get { return _documentation; }
        }

        public ICommand Back
        {
            get { return _back; }
        }

        public ICommand Home
        {
            get { return _home; }
        }

        public String BreadCrumb
        {
            get { return _breadCrumb; }
            set { _breadCrumb = value; RaisePropertyChanged(); }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        public Visibility IsLoading
        {
            get { return _loading; }
            set 
            {
                _loading = value;
                RaisePropertyChanged();
                var other = value == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                _browser.Visibility = other;
            }
        }

        public ICommand Info
        {
            get { return _more; }
        }

        public Visibility HasInfo
        {
            get { return _info; }
            set { _info = value; RaisePropertyChanged(); }
        }

        public Int32 TabIndex
        {
            get { return _tab; }
            set { _tab = value; RaisePropertyChanged(); }
        }

        public FlowDocument CurrentDocument
        {
            get { return _currentDocument; }
            set { _currentDocument = value; RaisePropertyChanged(); }
        }

        public HelpSection Topic
        {
            get { return _topic; }
            set
            {
                _topic = value;
                BreadCrumb = BuildBreadCrumb(Messages.Documentation, _topic.Topic);
                Title = _topic.Name;
                HasInfo = _topic.HasLink ? Visibility.Visible : Visibility.Collapsed;
                CurrentDocument = FillDocumentation();
                TabIndex = 1;
            }
        }

		public ObservableCollection<PanoramaGroup> Groups 
        {
            get { return _groups; }
        }

        public ObservableCollection<HelpSection> Results
        {
            get { return _results; }
        }

        public String SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged();
                _results.Clear();

                foreach (var section in _documentation.Sections)
                {
                    if (section.Name.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1 ||
                        section.Description.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        _results.Add(section);
                    }
                }
            }
        }

		#endregion

        #region Methods

        static String BuildBreadCrumb(String front, String back)
        {
            return String.Format("{0} > {1}", front, back);
        }

        FlowDocument FillDocumentation()
        {
            var document = new FlowDocument();
            document.Blocks.Add(GetDescription());

            if (_topic is HelpFunctionSection)
            {
                var hasCommand = _commands.HasCommand(_topic.Name);
                var usages = (_topic as HelpFunctionSection).Usages;

                if (usages.Count > 0)
                {
                    foreach (var usage in usages)
                    {
                        document.Blocks.Add(GetUsage(hasCommand, usage));
                        document.Blocks.Add(GetDescription(usage.Description));
                        document.Blocks.Add(GetArguments(usage.Arguments));
                        document.Blocks.Add(GetReturns(usage.Returns));
                        document.Blocks.Add(GetExamples(usage.Examples));
                    }
                }
                else
                {
                    var paragraph = new Paragraph();
                    InsertIntoParagraph(paragraph, Messages.NoUsagesAvailable);
                    document.Blocks.Add(paragraph);
                }
            }

            return document;
        }

        #endregion

        #region Create Help

        Paragraph GetDescription()
        {
            var p = new Paragraph();
            InsertIntoParagraph(p, _topic.Description);
            return p;
        }

        Paragraph GetUsage(Boolean hasCommand, HelpFunctionUsage usage)
        {
            var p = new Paragraph();
            p.Inlines.Add(new Run(Messages.Usage)
            {
                FontWeight = FontWeights.Bold,
                FontSize = 20
            });
            p.Inlines.Add(new LineBreak());
            p.Inlines.Add(new Run(usage.Usage)
            {
                Foreground = new SolidColorBrush(Colors.SteelBlue),
                FontWeight = FontWeights.Bold
            });

            if (hasCommand && _commands.HasOverload(_topic.Name, usage.Arguments.Count))
            {
                p.Inlines.Add(new Run(_topic.Name + " " + String.Join(" ", usage.ArgumentNames))
                {
                    Foreground = new SolidColorBrush(Colors.SteelBlue),
                    FontWeight = FontWeights.Bold
                });
                p.Inlines.Add(new LineBreak());
            }

            return p;
        }

        static Paragraph GetDescription(String description)
        {
            var p = new Paragraph();

            if (String.IsNullOrEmpty(description))
            {
                InsertIntoParagraph(p, Messages.Description, Messages.NoDescriptionAvailable);
            }
            else
            {
                InsertIntoParagraph(p, Messages.Description, description);
            }

            return p;
        }

        static Paragraph GetArguments(List<String> arguments)
        {
            var p = new Paragraph();

            if (arguments.Count == 0)
            {
                InsertIntoParagraph(p, Messages.Arguments, "---");
            }
            else
            {
                InsertIntoParagraph(p, Messages.Arguments, arguments);
            }

            return p;
        }

        static Paragraph GetReturns(List<String> returns)
        {
            var p = new Paragraph();
            InsertIntoParagraph(p, Messages.Returns, returns);
            return p;
        }

        Paragraph GetExamples(List<HelpExample> examples)
        {
            var p = new Paragraph();
            InsertIntoParagraph(p, Messages.Examples, examples);
            return p;
        }

        void InsertIntoParagraph(Paragraph p, String title, List<HelpExample> examples)
        {
            p.Inlines.Add(new Run(title)
            {
                Foreground = new SolidColorBrush(Colors.Black)
            });
            p.Inlines.Add(new LineBreak());

            if (examples.Count > 0)
            {
                for (var i = 0; i < examples.Count; )
                {
                    InsertIntoParagraph(p, examples[i], ++i);
                }
            }
            else
            {
                p.Inlines.Add(new Run(Messages.NoExamplesAvailable)
                {
                    Foreground = new SolidColorBrush(Colors.DarkGray)
                });
            }

            p.Inlines.Add(new LineBreak());
        }

        void InsertIntoParagraph(Paragraph p, HelpExample example, Int32 nr)
        {
            p.Inlines.Add(new Run(Messages.CopyCommand)
            {
                Foreground = new SolidColorBrush(Colors.LightGray),
                FontSize = 10.0,
                Cursor = Cursors.Hand
            });
            p.Inlines.LastInline.MouseDown += (sndr, evnt) => Clipboard.SetText(example.Example);
            p.Inlines.Add(new Run(Messages.ExecuteCommand)
            {
                Foreground = new SolidColorBrush(Colors.LightGray),
                FontSize = 10.0,
                Cursor = Cursors.Hand
            });
            p.Inlines.LastInline.MouseDown += (sndr, evnt) => _console.Execute(example.Example);
            p.Inlines.Add(new Run(example.Example)
            {
                Foreground = new SolidColorBrush(Colors.Green)
            });
            p.Inlines.Add(new LineBreak());
            p.Inlines.Add(new Run(example.Description)
            {
                Foreground = new SolidColorBrush(Colors.DarkGray)
            });
            p.Inlines.Add(new LineBreak());
        }

        static void InsertIntoParagraph(Paragraph p, String title, IEnumerable<String> text)
        {
            p.Inlines.Add(new Run(title)
            {
                Foreground = new SolidColorBrush(Colors.Black)
            });
            p.Inlines.Add(new LineBreak());

            foreach (var txt in text)
            {
                p.Inlines.Add(new Run(txt)
                {
                    Foreground = new SolidColorBrush(Colors.Green)
                });
                p.Inlines.Add(new LineBreak());
            }
        }

        static void InsertIntoParagraph(Paragraph p, String title, String text)
        {
            p.Inlines.Add(new Run(title)
            {
                Foreground = new SolidColorBrush(Colors.Black),
                FontWeight = FontWeights.Bold
            });
            p.Inlines.Add(new LineBreak());
            p.Inlines.Add(new Run(text)
            {
                Foreground = new SolidColorBrush(Colors.Blue)
            });
        }

        static void InsertIntoParagraph(Paragraph p, String description)
        {
            p.Inlines.Add(new Run(Messages.Description)
            {
                Foreground = new SolidColorBrush(Colors.Black),
                FontWeight = FontWeights.Bold,
                FontSize = 20
            });
            p.Inlines.Add(new LineBreak());
            p.Inlines.Add(new Run(description)
            {
                Foreground = new SolidColorBrush(Colors.Blue)
            });
        }

        #endregion
    }
}
