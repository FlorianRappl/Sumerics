using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Sumerics.Controls
{
    /// <summary>
    /// Item of autocomplete menu
    /// </summary>
    public class AutocompleteItem
    {
        #region ctor

        public AutocompleteItem() : this(string.Empty)
        {
        }

        public AutocompleteItem(string text)
		{
            Text = text;
        }

        public AutocompleteItem(string text, string toolTip)
            : this(text)
        {
            ToolTip = toolTip;
        }

        public AutocompleteItem(string text, string toolTip, BitmapImage icon)
        {
            Text = text;
            ToolTip = toolTip;
            Icon = icon;
        }

        /// <summary>
        /// Text for tooltip.
        /// </summary>
        /// <remarks>Return null for disable tooltip for this item</remarks>
		public virtual string ToolTip
		{
			get;
			set;
		}

        #endregion

		#region Properties

		public string Text { get; set; }

        public BitmapImage Icon
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns text for inserting into Textbox
        /// </summary>
        public virtual string GetTextForReplace()
        {
            return Text;
        }

        /// <summary>
        /// Compares fragment text with this item
        /// </summary>
        public virtual CompareResult Compare(string fragmentText)
        {
            if (Text.StartsWith(fragmentText, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.VisibleAndSelected;

            return CompareResult.Hidden;
        }

        /// <summary>
        /// Returns text for display into popup menu
        /// </summary>
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// This method is called after item inserted into text
        /// </summary>
        public virtual void OnSelected(object sender, SelectedEventArgs e)
        {
        }

        #endregion
    }
}
