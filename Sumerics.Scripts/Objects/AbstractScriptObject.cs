using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sumerics.Controls
{
	public abstract class AbstractScriptObject : INotifyPropertyChanged, IDisposable
	{
		#region Members

		int inConnectors;
		int outConnectors;

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler InputConnectorsChanged;
		public event EventHandler OutputConnectorsChanged;

		#endregion

		#region ctor

		public AbstractScriptObject()
		{
            Width = 200;
            Height = 100;
			InputConnectors = 1;
			OutputConnectors = 1;
			ValueMissing = true;
		}

		#endregion

		#region Properties

		public CircleButton[] Buttons
		{
			get;
			protected set;
		}

		public int Height
        {
            get;
            protected set;
        }

        public int Width
        {
            get;
            protected set;
        }

		public bool IsCopy
		{
			get;
			protected set;
		}

		public string Title
		{
			get;
			protected set;
		}

		public bool ShowConnectors
		{
			get { return IsCopy; }
		}

		public int InputConnectors
		{
			get { return inConnectors; }
			protected set
			{
				inConnectors = value;

				if (InputConnectorsChanged != null)
					InputConnectorsChanged(this, EventArgs.Empty);
			}
		}

		public int OutputConnectors
		{
			get { return outConnectors; }
			protected set
			{
				outConnectors = value;

				if (OutputConnectorsChanged != null)
					OutputConnectorsChanged(this, EventArgs.Empty);
			}
		}

		public bool ValueMissing
		{
			get;
			protected set;
		}

		public UIElement Element 
		{
			get;
			private set; 
		}

		#endregion

		#region Methods

		protected abstract AbstractScriptObject CreateCopy();

		protected abstract UIElement CreateContent();

		internal AbstractScriptObject Copy()
		{
			if (IsCopy)
				return this;

			var m = CreateCopy();
			m.Element = m.CreateContent();
			m.IsCopy = true;
			m.ValueMissing = ValueMissing;
			return m;
		}

		protected void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(property));
		}

        public void Dispose()
        {
            if (Element != null && Element is IDisposable)
                ((IDisposable)Element).Dispose();
		}

		#endregion
    }
}
