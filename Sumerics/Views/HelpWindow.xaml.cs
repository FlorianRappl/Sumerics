namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using System;

    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : MetroWindow
	{
		#region ctor

		public HelpWindow()
        {
			InitializeComponent();
        }

		#endregion

        #region Handlers

        void SearchGotFocus(Object sender, EventArgs e)
        {
            SearchPopup.IsOpen = true;
        }

        void SearchLostFocus(Object sender, EventArgs e)
        {
            SearchPopup.IsOpen = false;
        }

        #endregion
	}
}
