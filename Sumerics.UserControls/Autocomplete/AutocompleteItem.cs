namespace Sumerics.Controls
{
    using System;
    using System.Windows.Media.Imaging;

    public class AutocompleteItem
    {
        #region ctor

        public AutocompleteItem() : 
            this(String.Empty)
        {
        }

        public AutocompleteItem(String text)
		{
            Text = text;
        }

        public AutocompleteItem(String text, String toolTip)
            : this(text)
        {
            ToolTip = toolTip;
        }

        public AutocompleteItem(String text, String toolTip, BitmapImage icon)
        {
            Text = text;
            ToolTip = toolTip;
            Icon = icon;
        }

        #endregion

        #region Properties

        public String ToolTip
        {
            get;
            set;
        }

		public String Text 
        { 
            get;
            set; 
        }

        public BitmapImage Icon
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public virtual String GetTextForReplace()
        {
            return Text;
        }

        public virtual CompareResult Compare(String fragmentText)
        {

            if (Text.StartsWith(fragmentText, StringComparison.InvariantCultureIgnoreCase))
            {
                return CompareResult.VisibleAndSelected;
            }

            return CompareResult.Hidden;
        }

        public override String ToString()
        {
            return Text;
        }

        public virtual void OnSelected(Object sender, SelectedEventArgs e)
        {
        }

        #endregion
    }
}
