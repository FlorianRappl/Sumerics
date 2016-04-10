using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sumerics.Controls
{
	public class InConnectorNode : ConnectorNode
	{
		public InConnectorNode() : base(Brushes.Green)
		{

		}

        public override bool CanConnectTo(ConnectorNode node)
        {
            return node is OutConnectorNode;
        }
    }
}
