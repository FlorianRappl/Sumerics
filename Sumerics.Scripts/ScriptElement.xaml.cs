namespace Sumerics.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

	/// <summary>
	/// Interaction logic for ScriptElement.xaml
	/// </summary>
	public partial class ScriptElement : UserControl
    {
        #region Fields

        readonly List<ConnectorNode> _connectors = new List<ConnectorNode>();
        readonly ScriptControl _owner;
        Boolean _mouseDown;
        Point _point;
		AbstractScriptObject _host;

        #endregion

        #region ctor

        public ScriptElement(ScriptControl owner)
		{
            _owner = owner;
			InitializeComponent();
		}
		
		public ScriptElement(ScriptElement copy) : 
            this(copy._owner)
		{
			ScriptHost = copy.ScriptHost.Copy();
		}

        #endregion

        #region Properties

        public AbstractScriptObject ScriptHost 
		{
			get { return _host; }
			set 
			{
				_host = value;

                if (value.IsCopy)
                {
					_host.InputConnectorsChanged += InputConnectorsChanged;
					_host.OutputConnectorsChanged += OutputConnectorsChanged;
                    MouseDown += OnMouseDown;
                    MouseMove += OnMouseMove;
                    MouseUp += OnMouseUp;
                    Height = value.Height;
                    Width = value.Width;
                    CanvasBorder.Height = Height;
                    CanvasBorder.Width = Width;
                    var c = CenterCanvas;
                    CreateTitle(c);
                    CreateNodes(c);
                    CreateDeleteButton(c);
					CreateButtons(c);
                    CenterElement = _host.Element;
                }
                else
                {
                    CenterElement = new TextBlock
                    {
                        Text = _host.Title,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                    };
                }
			}
        }

        UIElement CenterElement
        {
            get { return CanvasBorder.Child; }
            set { CanvasBorder.Child = value; }
        }

        Border CanvasBorder
        {
            get { return CenterCanvas.Children[0] as Border; }
        }

        Canvas CenterCanvas
        {
            get { return Content as Canvas; }
        }

        #endregion

        #region Mouse and Touch events

        void OnMouseUp(Object sender, MouseButtonEventArgs e)
        {
            _mouseDown = false;
            ReleaseMouseCapture();
        }

        void OnMouseMove(Object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                var p = e.GetPosition(_owner);
                _owner.Shift(this, p.X - _point.X, p.Y - _point.Y);
                _point = p;
                e.Handled = true;
            }
        }

        void OnMouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Ellipse == false && e.OriginalSource != _host.Element)
            {
                CaptureMouse();
                _mouseDown = true;
                _point = e.GetPosition(_owner);
            }
        }

        #endregion

		#region Events

		void OutputConnectorsChanged(Object sender, EventArgs e)
		{
			
		}

		void InputConnectorsChanged(Object sender, EventArgs e)
		{
			
		}

		#endregion

		#region Graphics

		void CreateTitle(Canvas c)
        {
            var tb = new TextBlock();
            tb.Text = _host.Title;
            tb.Width = Width;
            tb.Height = 15;
            Canvas.SetLeft(tb, 5);
            Canvas.SetTop(tb, 5);
            c.Children.Add(tb);
        }

		void CreateDeleteButton(Canvas c)
		{
			var bt = new Button();
			bt.SetResourceReference(Button.StyleProperty, "ScriptMetroCircleButton");
            var path = new Uri(@"Images\remove.png", UriKind.Relative);
			var img = new Image();
			img.Source = new BitmapImage(path);
			bt.Width = 30;
			bt.Height = 30;
			bt.Content = img;
			bt.ToolTip = "Remove element";
			Canvas.SetLeft(bt, 5);
			Canvas.SetBottom(bt, 0);
			c.Children.Add(bt);
            bt.PreviewMouseDown += RemoveClick;
		}

		void CreateButtons(Canvas c)
		{
            if (_host.Buttons != null)
            {
                var offset = 5.0;

                foreach (var button in _host.Buttons)
                {
                    var action = button.Clicked;
                    var bt = new Button();
                    bt.SetResourceReference(Button.StyleProperty, "ScriptMetroCircleButton");
                    var img = new Image();
                    img.Source = button.Image;
                    bt.Width = 30;
                    bt.Height = 30;
                    bt.Content = img;
                    bt.ToolTip = button.ToolTip;
                    Canvas.SetRight(bt, offset);
                    Canvas.SetBottom(bt, 0.0);
                    c.Children.Add(bt);
                    bt.PreviewMouseDown += (s, e) => { action(); };
                    offset += bt.Width + 5.0;
                }
            }
		}

        void RemoveClick(Object sender, MouseButtonEventArgs e)
        {
            _owner.RemoveChild(this);
        }

		void CreateNodes(Canvas c)
		{
			CreateNodes(c, 0, _host.InputConnectors, typeof(InConnectorNode));
			CreateNodes(c, Height, _host.OutputConnectors, typeof(OutConnectorNode));
		}

		void CreateNodes(Canvas c, Double position, Int32 count, Type connection)
		{
            if (count != 0)
            {
                var cstr = connection.GetConstructor(Type.EmptyTypes);
                var width = Width;
                var shift = width / (count + 1.0);
                var pos = shift;

                for (var i = 0; i < count; i++)
                {
                    var ctrl = (ConnectorNode)cstr.Invoke(null);
                    var ellipse = ctrl.Control;
                    Canvas.SetLeft(ellipse, pos - ellipse.Width / 2);
                    Canvas.SetTop(ellipse, position - ellipse.Height / 2);
                    pos += shift;
                    c.Children.Add(ellipse);
                    _connectors.Add(ctrl);
                }
            }
		}

        #endregion
	}
}
