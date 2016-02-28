namespace Sumerics.Controls
{
    using System;
    using System.Windows.Media;

    /// <summary>
    /// <see cref="DiagramEntity"/> event argument.
    /// </summary>
    public class ColorEventArgs : EventArgs
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
        public ColorEventArgs(Brush brush)
        {
            Brush = brush;
        }

        #endregion
    }
}
