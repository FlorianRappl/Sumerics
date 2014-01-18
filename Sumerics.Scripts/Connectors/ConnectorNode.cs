using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sumerics.Controls
{
	public abstract class ConnectorNode
    {
        #region Members

        Ellipse ellipse;

        #endregion

        #region ctor

        public ConnectorNode(Brush fill)
		{
            ellipse = new Ellipse();

            ellipse.PreviewMouseDown += ellipse_PreviewMouseDown;
            ellipse.PreviewMouseUp += ellipse_PreviewMouseUp;
            ellipse.PreviewMouseMove += ellipse_PreviewMouseMove;

			ellipse.Width = 10;
			ellipse.Height = 10;
			ellipse.Fill = fill;
		}

        #endregion

        #region Properties

        public Ellipse Control
        {
            get { return ellipse; }
        }

        #endregion

        #region Mouse / Touch events

        void ellipse_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            
        }

        void ellipse_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ellipse.ReleaseMouseCapture();
        }

        void ellipse_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ellipse.CaptureMouse();
            e.Handled = true;
        }

        #endregion

        #region Methods

        public abstract bool CanConnectTo(ConnectorNode node);

        #endregion
	}
}
