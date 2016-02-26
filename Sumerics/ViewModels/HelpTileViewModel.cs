namespace Sumerics.ViewModels
{
    using MahApps.Metro.Controls;
    using System;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using YAMP.Help;

	public class HelpTileViewModel : BaseViewModel, IPanoramaTile
    {
        #region Fields

        readonly HelpEntry _entry;
        readonly BitmapImage _icon;
        readonly Documentation _documentation;
        readonly ICommand _clicked;

        #endregion

        #region ctor

        public HelpTileViewModel(DocumentationViewModel parent, HelpEntry entry)
		{
			_entry = entry;
            _icon = IconFactory.GetHighImage(entry.Topic.Kind);
            _documentation = parent.Help;
            _clicked = new RelayCommand(x =>
            {
                var topic = _documentation.Get(_entry.Name);
                parent.Topic = topic;
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
