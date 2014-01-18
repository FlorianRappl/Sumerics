using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using YAMP.Help;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Sumerics
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : MetroWindow
	{
		#region Members

		HelpSection topic;
		ObservableCollection<HelpSection> results;

        const string UA = "Mozilla/5.0 (iPad; U; CPU OS 3_2_1 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Mobile/7B405";

		#endregion

		#region ctor

		public HelpWindow()
        {
			results = new ObservableCollection<HelpSection>();
			InitializeComponent();
			SearchResults.ItemsSource = results;
			Loaded += HelpWindowLoaded;
            Browser.Navigated += Browser_Navigated;
        }

        void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            SetSilent(Browser, true);
        }

		#endregion

		#region Properties
		
		public HelpSection Topic
		{
			get { return topic; }
			set
			{
				topic = value;
				HelpSource.Text = "Documentation > " + topic.Topic;
				HelpTitle.Text = topic.Name;
                InfoButton.Visibility = topic.HasLink ? Visibility.Visible : Visibility.Collapsed;
				FillDocumentation();
				Tabs.SelectedIndex = 1;
			}
		}

		#endregion

		#region Methods

		void HelpWindowLoaded(object sender, RoutedEventArgs e)
		{
			DataContext = DocumentationViewModel.Instance;
		}

		void FillDocumentation()
		{
			var doc = new FlowDocument();
			doc.Blocks.Add(GetDescription());

			if (topic is HelpFunctionSection)
			{
				var hasCommand = YCommand.HasCommand(topic.Name);
				var usages = (topic as HelpFunctionSection).Usages;

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
					var p = new Paragraph();
					InsertIntoParagraph(p, "No usages available.");
					doc.Blocks.Add(p);
				}
			}

			HelpText.Document = doc;
		}

		void BackClick(object sender, RoutedEventArgs e)
		{
			Tabs.SelectedIndex = 0;
			HelpTitle.Text = "Help";
			HelpSource.Text = "Documentation > Overview";
		}

        void InternetBackClick(object sender, RoutedEventArgs e)
        {
            Tabs.SelectedIndex = 1;
            HelpTitle.Text = "Help";
            HelpSource.Text = "Documentation > Overview";
            Loading.Visibility = System.Windows.Visibility.Visible;
            Browser.Visibility = System.Windows.Visibility.Hidden;
        }

        void InfoClick(object sender, RoutedEventArgs e)
        {
            Tabs.SelectedIndex = 2;
            Browser.Navigate(topic.Link);
        }

        void ContentLoaded(object sender, NavigationEventArgs e)
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
			InsertIntoParagraph(p, topic.Description);
			return p;
		}

		Paragraph GetUsage(bool hasCommand, HelpFunctionUsage usage)
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

			if (hasCommand && YCommand.HasOverload(topic.Name, usage.Arguments.Count))
			{
				var o = new Run(topic.Name + " " + string.Join(" ", usage.ArgumentNames));
				o.Foreground = new SolidColorBrush(Colors.SteelBlue);
				o.FontWeight = FontWeights.Bold;
				p.Inlines.Add(o); 
				p.Inlines.Add(new LineBreak());
			}

			return p;
		}

		Paragraph GetDescription(string description)
		{
			var p = new Paragraph();

			if(string.IsNullOrEmpty(description))
				InsertIntoParagraph(p, "Description", "No description available.");
			else
				InsertIntoParagraph(p, "Description", description);

			return p;
		}

		Paragraph GetArguments(List<string> arguments)
		{
			var p = new Paragraph();

			if(arguments.Count == 0)
				InsertIntoParagraph(p, "Arguments", "---");
			else
				InsertIntoParagraph(p, "Arguments", arguments);

			return p;
		}

		Paragraph GetReturns(List<string> returns)
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

		void InsertIntoParagraph(Paragraph p, string title, List<HelpExample> examples)
		{
			var r = new Run(title);
			r.Foreground = new SolidColorBrush(Colors.Black);
			p.Inlines.Add(r);
			p.Inlines.Add(new LineBreak());

			if (examples.Count > 0)
			{
				for (var i = 0; i < examples.Count; )
					InsertIntoParagraph(p, examples[i], ++i);
			}
			else
			{
				var no = new Run("No examples available.");
				no.Foreground = new SolidColorBrush(Colors.DarkGray);
				p.Inlines.Add(no);
			}

			p.Inlines.Add(new LineBreak());
		}

		void InsertIntoParagraph(Paragraph p, HelpExample example, int nr)
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

		void InsertIntoParagraph(Paragraph p, string title, IEnumerable<string> text)
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

		void InsertIntoParagraph(Paragraph p, string title, string text)
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

		void SearchChanged(object sender, KeyEventArgs e)
		{
			results.Clear();
			var search = Search.Text.ToLower();

			foreach (var c in DocumentationViewModel.Instance.Document.Sections)
			{
				if (c.Name.ToLower().Contains(search))
					results.Add(c);
				else if (c.Description.ToLower().Contains(search))
					results.Add(c);
			}
		}

		void SearchGotFocus(object sender, EventArgs e)
		{
			SearchPopup.IsOpen = true;
		}

		void SearchLostFocus(object sender, EventArgs e)
		{
			SearchPopup.IsOpen = false;
		}

		void SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems == null)
				return;

			foreach (var item in e.AddedItems)
			{
				var section = item as HelpSection;

				if (section != null)
					Topic = section;
			}
		}

		#endregion
	}
}
