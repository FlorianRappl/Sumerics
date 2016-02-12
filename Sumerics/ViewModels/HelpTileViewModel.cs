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

        readonly HelpWindow _window;
        readonly HelpEntry _entry;
        readonly BitmapImage _icon;
        readonly Documentation _documentation;
        readonly ICommand _clicked;

        #endregion

        #region ctor

        public HelpTileViewModel(HelpWindow window, HelpEntry entry, Documentation documentation)
		{
            _window = window;
			_entry = entry;
            _icon = Icons.GetHighImage(entry.Topic.Kind);
            _documentation = documentation;
            _clicked = new RelayCommand(x =>
            {
                _window.Topic = _documentation.Get(_entry.Name);
            });
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
			get { return _clicked; }
        }

        #endregion
    }
}
