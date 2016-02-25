namespace Sumerics.ViewModels
{
    using MahApps.Metro.Controls;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using YAMP.Help;

	public sealed class DocumentationViewModel : BaseViewModel
	{
		#region Fields

        readonly ObservableCollection<PanoramaGroup> _groups;
        readonly Documentation _documentation;

		#endregion

		#region ctor

        public DocumentationViewModel(Documentation documentation)
		{
            _groups = new ObservableCollection<PanoramaGroup>();
            _documentation = documentation;

            foreach (var topic in _documentation.Topics)
            {
                var pg = new PanoramaGroup(topic.Kind);
                var content = new List<HelpTileViewModel>();

                foreach (var item in topic)
                {
                    var vm = new HelpTileViewModel(item, documentation);
                    content.Add(vm);
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
            get { return _documentation; }
		}

        /// <summary>
        /// Gets the various groups in the UI.
        /// </summary>
		public ObservableCollection<PanoramaGroup> Groups 
        {
            get { return _groups; }
        }

		#endregion
	}
}
