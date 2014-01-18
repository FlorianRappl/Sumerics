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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sumerics.Controls
{
	/// <summary>
	/// Interaction logic for ScriptElement.xaml
	/// </summary>
	public partial class ScriptElement : UserControl
    {
        #region

        bool mouseDown;
        Point point;
		AbstractScriptObject host;
		List<ConnectorNode> connectors = new List<ConnectorNode>();

        #endregion

        #region ctor

        public ScriptElement()
		{
			InitializeComponent();
		}
		
		public ScriptElement(ScriptElement copy) : this()
		{
			ScriptHost = copy.ScriptHost.Copy();
		}

        #endregion

        #region Properties

        public AbstractScriptObject ScriptHost 
		{
			get { return host; }
			set 
			{
				host = value;

                if (value.IsCopy)
                {
					host.InputConnectorsChanged += InputConnectorsChanged;
					host.OutputConnectorsChanged += OutputConnectorsChanged;
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
                    CenterElement = host.Element;
                }
                else
                {
                    CenterElement = new TextBlock
                    {
                        Text = host.Title,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                    };
                }
			}
        }

        UIElement CenterElement
        {
            get
            {
                return CanvasBorder.Child;
            }
            set
            {
                CanvasBorder.Child = value;
            }
        }

        Border CanvasBorder
        {
            get
            {
                return CenterCanvas.Children[0] as Border;
            }
        }

        Canvas CenterCanvas
        {
            get
            {
                return Content as Canvas;
            }
        }

        #endregion

        #region Mouse and Touch events

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            ReleaseMouseCapture();
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                var p = e.GetPosition(ScriptControl.Instance);
                ScriptControl.Instance.Shift(this, p.X - point.X, p.Y - point.Y);
                point = p;
                e.Handled = true;
            }
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Ellipse || e.OriginalSource == host.Element)
                return;

            CaptureMouse();
            mouseDown = true;
            point = e.GetPosition(ScriptControl.Instance);
        }

        #endregion

		#region Events

		void OutputConnectorsChanged(object sender, EventArgs e)
		{
			
		}

		void InputConnectorsChanged(object sender, EventArgs e)
		{
			
		}

		#endregion

		#region Graphics

		void CreateTitle(Canvas c)
        {
            var tb = new TextBlock();
            tb.Text = host.Title;
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
			var img = new Image();
			img.Source = new BitmapImage(new Uri(@"Images\remove.png", UriKind.Relative));
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
			if (host.Buttons == null)
				return;

			var offset = 5.0;

			foreach (var button in host.Buttons)
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

        void RemoveClick(object sender, MouseButtonEventArgs e)
        {
            ScriptControl.Instance.RemoveChild(this);
        }

		void CreateNodes(Canvas c)
		{
			CreateNodes(c, 0, host.InputConnectors, typeof(InConnectorNode));
			CreateNodes(c, Height, host.OutputConnectors, typeof(OutConnectorNode));
		}

		void CreateNodes(Canvas c, double position, int count, Type connection)
		{
			if (count == 0)
				return;

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
				connectors.Add(ctrl);
			}
		}

        #endregion
	}
}
