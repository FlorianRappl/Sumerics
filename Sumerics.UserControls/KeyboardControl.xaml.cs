using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sumerics.Controls
{
    /// <summary>
    /// Interaction logic for KeyboardControl.xaml
    /// </summary>
    public partial class KeyboardControl : UserControl
    {
        bool hidden = true;
        Action placeFocus;
        Action<string> insert;
        Action delete;

        public KeyboardControl()
        {
            InitializeComponent();
            InitializePanel();
        }

        public Action PlaceFocus
        {
            get { return placeFocus ?? (() => { }); }
            set { placeFocus = value; }
        }

        public Action<string> Insert
        {
            get { return insert ?? (c => { }); }
            set { insert = value; }
        }

        public Action Delete
        {
            get { return delete ?? (() => { }); }
            set { delete = value; }
        }

        public void Toggle()
        {
            if (!hidden)
            {
                hidden = true;
                SlideInputPanel(InputGrid.Height + InputGrid.Margin.Bottom + InputGrid.Margin.Top, 0.0);
            }
            else if (hidden)
            {
                hidden = false;
                SlideInputPanel(0.0, InputGrid.Height + InputGrid.Margin.Bottom + InputGrid.Margin.Top);
            }
        }

        void InitializePanel()
        {
            foreach (var child in InputGrid.Children)
            {
                if (child is Button)
                {
                    var bt = (Button)child;

                    if (bt.Tag == null)
                        bt.Click += InsertButtonClick;
                    else
                        bt.Click += InsertSpecialButtonClick;
                }
            }
        }

        void SlideInputPanel(double from, double to)
        {
            var sb = new Storyboard();
            var animation = new DoubleAnimation(from, to, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Height"));
            sb.Children.Add(animation);
            sb.Begin();
            PlaceFocus();
        }

        void InsertButtonClick(object sender, RoutedEventArgs e)
        {
            var bt = (Button)e.Source;
            var content = (string)bt.Content;
            Insert(content);
            PlaceFocus();
        }

        void InsertSpecialButtonClick(object sender, RoutedEventArgs e)
        {
            var bt = (Button)e.Source;
            var content = (string)bt.Tag;

            switch (content)
            {
                case "b":
                    Delete();
                    break;

                case "n":
                    Insert("\n");
                    break;
            }

            PlaceFocus();
        }
    }
}
