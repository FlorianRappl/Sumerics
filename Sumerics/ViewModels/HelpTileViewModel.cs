using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using YAMP.Help;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Sumerics
{
	public class HelpTileViewModel : BaseViewModel, IPanoramaTile
    {
        #region Members

        HelpEntry _entry;
        BitmapImage _icon;

        #endregion

        #region ctor

        public HelpTileViewModel(HelpEntry entry)
		{
			_entry = entry;
            _icon = Icons.GetHighImage(entry.Topic.Kind);
		}

        #endregion

        #region Properties

        public string Name
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
					var window = StaticHelpers.GetWindow<HelpWindow>();
					window.Topic = DocumentationViewModel.Instance.Document.Get(_entry.Name);
				});
			}
        }

        #endregion
    }
}
