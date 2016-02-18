namespace Sumerics.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

	public abstract class AbstractScriptObject : INotifyPropertyChanged, IDisposable
	{
		#region Fields

		Int32 _inConnectors;
		Int32 _outConnectors;

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
			get { return _inConnectors; }
			protected set
			{
				_inConnectors = value;

				if (InputConnectorsChanged != null)
					InputConnectorsChanged(this, EventArgs.Empty);
			}
		}

		public int OutputConnectors
		{
			get { return _outConnectors; }
			protected set
			{
				_outConnectors = value;

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
			if (!IsCopy)
            {
                var m = CreateCopy();
                m.Element = m.CreateContent();
                m.IsCopy = true;
                m.ValueMissing = ValueMissing;
                return m;
            }
				
            return this;
		}

		protected void RaisePropertyChanged([CallerMemberName] String property = null)
		{
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
		}

        public void Dispose()
        {
            if (Element != null && Element is IDisposable)
            {
                ((IDisposable)Element).Dispose();
            }
		}

		#endregion
    }
}
