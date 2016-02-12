namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Commands;
    using Sumerics.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Navigation;
    using YAMP.Help;

    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : MetroWindow
	{
		#region Fields

        readonly CommandFactory _commands;
        readonly ObservableCollection<HelpSection> _results;

		HelpSection _topic;

        const String UA = "Mozilla/5.0 (iPad; U; CPU OS 3_2_1 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Mobile/7B405";

		#endregion

		#region ctor

		public HelpWindow(IContainer container)
        {
			_results = new ObservableCollection<HelpSection>();
            _commands = container.Get<CommandFactory>();
			InitializeComponent();
			SearchResults.ItemsSource = _results;
            DataContext = container.Get<DocumentationViewModel>();
            Browser.Navigated += Browser_Navigated;
        }

        void Browser_Navigated(Object sender, NavigationEventArgs e)
        {
            SetSilent(Browser, true);
        }

		#endregion

        #region Properties

        public HelpSection Topic
        {
            get { return _topic; }
            set
            {
                _topic = value;
                HelpSource.Text = "Documentation > " + _topic.Topic;
                HelpTitle.Text = _topic.Name;
                InfoButton.Visibility = _topic.HasLink ? Visibility.Visible : Visibility.Collapsed;
                FillDocumentation();
                Tabs.SelectedIndex = 1;
            }
        }

        #endregion

        #region Methods

		void FillDocumentation()
		{
			var doc = new FlowDocument();
			doc.Blocks.Add(GetDescription());

			if (_topic is HelpFunctionSection)
			{
				var hasCommand = _commands.HasCommand(_topic.Name);
				var usages = (_topic as HelpFunctionSection).Usages;

				if (usages.Count > 0)
				{
					foreach (var usage in usages)
					{
						doc.Blocks.Add(GetUsage(hasCommand, usage));
						doc.Blocks.Add(GetDescription(usage.Description));
						doc.Blocks.Add(GetArguments(usage.Arguments));
						doc.Blocks.Add(GetReturns(usage.Returns));
						doc.Blocks.Add(GetExamples(usage.Examples));
					}
				}
				else
				{
					var paragraph = new Paragraph();
					InsertIntoParagraph(paragraph, "No usages available.");
					doc.Blocks.Add(paragraph);
				}
			}

			HelpText.Document = doc;
		}

		void BackClick(Object sender, RoutedEventArgs e)
		{
			Tabs.SelectedIndex = 0;
			HelpTitle.Text = "Help";
			HelpSource.Text = "Documentation > Overview";
		}

        void InternetBackClick(Object sender, RoutedEventArgs e)
        {
            Tabs.SelectedIndex = 1;
            HelpTitle.Text = "Help";
            HelpSource.Text = "Documentation > Overview";
            Loading.Visibility = System.Windows.Visibility.Visible;
            Browser.Visibility = System.Windows.Visibility.Hidden;
        }

        void InfoClick(Object sender, RoutedEventArgs e)
        {
            Tabs.SelectedIndex = 2;
            Browser.Navigate(_topic.Link);
        }

        void ContentLoaded(Object sender, NavigationEventArgs e)
        {
            Loading.Visibility = System.Windows.Visibility.Hidden;
            Browser.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region Set Browser Silent

        static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            var sp = browser.Document as IOleServiceProvider;

            if (sp != null)
            {
                var IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                var IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);

                if (webBrowser != null)
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
            }
        }

        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
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
			var r = new Run("Usage");
			r.FontWeight = FontWeights.Bold;
			r.FontSize = 20;
			p.Inlines.Add(r);
			p.Inlines.Add(new LineBreak());
			var u = new Run(usage.Usage);
			u.Foreground = new SolidColorBrush(Colors.SteelBlue);
			u.FontWeight = FontWeights.Bold;
			p.Inlines.Add(u);

            if (hasCommand && _commands.HasOverload(_topic.Name, usage.Arguments.Count))
			{
				var o = new Run(_topic.Name + " " + String.Join(" ", usage.ArgumentNames));
				o.Foreground = new SolidColorBrush(Colors.SteelBlue);
				o.FontWeight = FontWeights.Bold;
				p.Inlines.Add(o); 
				p.Inlines.Add(new LineBreak());
			}

			return p;
		}

		Paragraph GetDescription(String description)
		{
			var p = new Paragraph();

            if (String.IsNullOrEmpty(description))
            {
                InsertIntoParagraph(p, "Description", "No description available.");
            }
            else
            {
                InsertIntoParagraph(p, "Description", description);
            }

			return p;
		}

		Paragraph GetArguments(List<String> arguments)
		{
			var p = new Paragraph();

            if (arguments.Count == 0)
            {
                InsertIntoParagraph(p, "Arguments", "---");
            }
            else
            {
                InsertIntoParagraph(p, "Arguments", arguments);
            }

			return p;
		}

		Paragraph GetReturns(List<String> returns)
		{
			var p = new Paragraph();
			InsertIntoParagraph(p, "Returns", returns);
			return p;
		}

		Paragraph GetExamples(List<HelpExample> examples)
		{
			var p = new Paragraph();
			InsertIntoParagraph(p, "Examples", examples);
			return p;
		}

		void InsertIntoParagraph(Paragraph p, String title, List<HelpExample> examples)
		{
			var r = new Run(title);
			r.Foreground = new SolidColorBrush(Colors.Black);
			p.Inlines.Add(r);
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
				var no = new Run("No examples available.");
				no.Foreground = new SolidColorBrush(Colors.DarkGray);
				p.Inlines.Add(no);
			}

			p.Inlines.Add(new LineBreak());
		}

		void InsertIntoParagraph(Paragraph p, HelpExample example, Int32 nr)
		{
			var c = new Run("( copy ) ");
			c.Foreground = new SolidColorBrush(Colors.LightGray);
			c.FontSize = 10.0;
			p.Inlines.Add(c);
			c.Cursor = Cursors.Hand;
			c.MouseDown += (sndr, evnt) => { Clipboard.SetText(example.Example); };
			var d = new Run(example.Example);
			d.Foreground = new SolidColorBrush(Colors.Green);
			p.Inlines.Add(d);
			p.Inlines.Add(new LineBreak());
			var e = new Run(example.Description);
			e.Foreground = new SolidColorBrush(Colors.DarkGray);
			p.Inlines.Add(e);
			p.Inlines.Add(new LineBreak());
		}

		void InsertIntoParagraph(Paragraph p, String title, IEnumerable<String> text)
		{
			var r = new Run(title);
			r.Foreground = new SolidColorBrush(Colors.Black);
			p.Inlines.Add(r);
			p.Inlines.Add(new LineBreak());

			foreach (var txt in text)
			{
				var d = new Run(txt);
				d.Foreground = new SolidColorBrush(Colors.Green);
				p.Inlines.Add(d);
				p.Inlines.Add(new LineBreak());
			}
		}

		void InsertIntoParagraph(Paragraph p, String title, String text)
		{
			var r = new Run(title);
			r.Foreground = new SolidColorBrush(Colors.Black);
			r.FontWeight = FontWeights.Bold;
			p.Inlines.Add(r);
			p.Inlines.Add(new LineBreak());
			var d = new Run(text);
			d.Foreground = new SolidColorBrush(Colors.Blue);
			p.Inlines.Add(d);
		}

		void InsertIntoParagraph(Paragraph p, string description)
		{
			var r = new Run("Description");
			r.Foreground = new SolidColorBrush(Colors.Black);
			r.FontWeight = FontWeights.Bold;
			r.FontSize = 20;
			p.Inlines.Add(r);
			p.Inlines.Add(new LineBreak());
			var d = new Run(description);
			d.Foreground = new SolidColorBrush(Colors.Blue);
			p.Inlines.Add(d);
		}

		#endregion

		#region Search

		void SearchChanged(Object sender, KeyEventArgs e)
		{
			_results.Clear();
			var search = Search.Text.ToLower();
            var documentation = DataContext as DocumentationViewModel;

            foreach (var section in documentation.Document.Sections)
			{
                if (section.Name.ToLower().Contains(search))
                {
                    _results.Add(section);
                }
                else if (section.Description.ToLower().Contains(search))
                {
                    _results.Add(section);
                }
			}
		}

		void SearchGotFocus(Object sender, EventArgs e)
		{
			SearchPopup.IsOpen = true;
		}

        void SearchLostFocus(Object sender, EventArgs e)
		{
			SearchPopup.IsOpen = false;
		}

        void SelectionChanged(Object sender, SelectionChangedEventArgs e)
		{
            if (e.AddedItems != null)
            {
                foreach (var item in e.AddedItems)
                {
                    var section = item as HelpSection;

                    if (section != null)
                    {
                        Topic = section;
                    }
                }
            }
		}

		#endregion
	}
}
