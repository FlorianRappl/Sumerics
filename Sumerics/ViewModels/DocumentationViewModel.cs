using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using YAMP.Help;

namespace Sumerics
{
	class DocumentationViewModel : BaseViewModel
	{
		#region Members

		static DocumentationViewModel instance;

		#endregion

		#region ctor

		private DocumentationViewModel ()
		{
			Groups = new ObservableCollection<PanoramaGroup>();

            foreach (var topic in Core.Help.Topics)
            {
                var pg = new PanoramaGroup(topic.Kind);
                var content = new List<HelpTileViewModel>();

                foreach (var item in topic)
                    content.Add(new HelpTileViewModel(item));

                pg.SetSource(content);
                Groups.Add(pg);
            }
		}

		#endregion

		#region Singleton

		public static DocumentationViewModel Instance
		{
			get
			{
				if (instance == null)
					instance = new DocumentationViewModel();

				return instance;
			}
		}

		#endregion

		#region Properties

        /// <summary>
        /// Gets the associated YAMP documentation.
        /// </summary>
		public Documentation Document
		{
			get { return Core.Help; }
		}

        /// <summary>
        /// Gets the various groups in the UI.
        /// </summary>
		public ObservableCollection<PanoramaGroup> Groups { get; private set; }

		#endregion
	}
}
