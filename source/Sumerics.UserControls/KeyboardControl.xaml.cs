namespace Sumerics.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Interaction logic for KeyboardControl.xaml
    /// </summary>
    public partial class KeyboardControl : UserControl
    {
        #region Fields

        Boolean _hidden = true;
        Action _placeFocus;
        Action<String> _insert;
        Action _delete;

        #endregion

        #region ctor

        public KeyboardControl()
        {
            InitializeComponent();
            InitializePanel();
        }

        #endregion

        #region Properties

        public Action PlaceFocus
        {
            get { return _placeFocus ?? (() => { }); }
            set { _placeFocus = value; }
        }

        public Action<String> Insert
        {
            get { return _insert ?? (c => { }); }
            set { _insert = value; }
        }

        public Action Delete
        {
            get { return _delete ?? (() => { }); }
            set { _delete = value; }
        }

        #endregion

        #region Methods

        public void Toggle()
        {
            if (!_hidden)
            {
                _hidden = true;
                SlideInputPanel(InputGrid.Height + InputGrid.Margin.Bottom + InputGrid.Margin.Top, 0.0);
            }
            else if (_hidden)
            {
                _hidden = false;
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
                    {
                        bt.Click += InsertButtonClick;
                    }
                    else
                    {
                        bt.Click += InsertSpecialButtonClick;
                    }
                }
            }
        }

        void SlideInputPanel(Double from, Double to)
        {
            var sb = new Storyboard();
            var animation = new DoubleAnimation(from, to, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Height"));
            sb.Children.Add(animation);
            sb.Begin();
            PlaceFocus();
        }

        void InsertButtonClick(Object sender, RoutedEventArgs e)
        {
            var bt = (Button)e.Source;
            var content = (String)bt.Content;
            Insert(content);
            PlaceFocus();
        }

        void InsertSpecialButtonClick(Object sender, RoutedEventArgs e)
        {
            var bt = (Button)e.Source;
            var content = (String)bt.Tag;

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

        #endregion
    }
}
