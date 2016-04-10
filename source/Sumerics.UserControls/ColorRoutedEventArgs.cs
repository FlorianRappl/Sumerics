namespace Sumerics.Controls
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Routed version of the <see cref="ColorEventArgs"/>.
    /// </summary>
    public class ColorRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>The entity.</value>
        public Brush Brush { get; set; }

        #region Constructor

        ///<summary>
        ///Default constructor
        ///</summary>
        public ColorRoutedEventArgs(ColorEventArgs e)
        {

            Brush = e.Brush;
        }

        #endregion
    }
}
