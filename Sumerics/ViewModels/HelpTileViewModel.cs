namespace Sumerics.ViewModels
{
    using MahApps.Metro.Controls;
    using Sumerics.Dialogs;
    using Sumerics.Views;
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

        public HelpTileViewModel(HelpEntry entry, Documentation documentation)
		{
			_entry = entry;
            _icon = IconFactory.GetHighImage(entry.Topic.Kind);
            _documentation = documentation;
            _clicked = new RelayCommand(x =>
            {
                var window = DialogExtensions.Get<HelpWindow>();

                if (window != null)
                {
                    var topic = _documentation.Get(_entry.Name);
                    window.Topic = topic;
                }
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
