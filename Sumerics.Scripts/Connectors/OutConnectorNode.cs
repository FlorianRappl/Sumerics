using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sumerics.Controls
{
	public class OutConnectorNode : ConnectorNode
	{
		public OutConnectorNode() : base(Brushes.Purple)
		{
		}

        public override bool CanConnectTo(ConnectorNode node)
        {
            return node is InConnectorNode;
        }
    }
}
