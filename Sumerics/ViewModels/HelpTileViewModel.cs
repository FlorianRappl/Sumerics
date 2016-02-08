namespace Sumerics
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using YAMP.Help;

	public class HelpTileViewModel : BaseViewModel, IPanoramaTile
    {
        #region Fields

        HelpEntry _entry;
        BitmapImage _icon;

        #endregion

        #region ctor

        public HelpTileViewModel(HelpEntry entry, IContainer container)
            : base(container)
		{
			_entry = entry;
            _icon = Icons.GetHighImage(entry.Topic.Kind);
		}

        #endregion

        #region Properties

        public String Name
		{
			get { return _entry.Name; }
		}

		public HelpTopic Topic
		{
			get { return _entry.Topic; }
		}

        public BitmapImage Icon
        {
            get { return _icon; }
        }

		public ICommand TileClickedCommand
		{
			get 
			{
				return new RelayCommand(x =>
				{
					var window = StaticHelpers.GetWindow<HelpWindow>(Container);
					window.Topic = Container.Get<DocumentationViewModel>().Document.Get(_entry.Name);
				});
			}
        }

        #endregion
    }
}
