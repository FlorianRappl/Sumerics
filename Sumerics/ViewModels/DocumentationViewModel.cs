namespace Sumerics
{
    using MahApps.Metro.Controls;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using YAMP.Help;

	sealed class DocumentationViewModel : BaseViewModel
	{
		#region Fields

		static DocumentationViewModel instance;

		#endregion

		#region ctor

		public DocumentationViewModel(IContainer container)
            : base(container)
		{
			Groups = new ObservableCollection<PanoramaGroup>();

            foreach (var topic in Core.Help.Topics)
            {
                var pg = new PanoramaGroup(topic.Kind);
                var content = new List<HelpTileViewModel>();

                foreach (var item in topic)
                {
                    content.Add(new HelpTileViewModel(item, container));
                }

                pg.SetSource(content);
                Groups.Add(pg);
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
