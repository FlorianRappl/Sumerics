namespace Sumerics
{
    using System;

    sealed class SaveImageViewModel : SaveFileViewModel
    {
        #region Fields

        Int32 imageWidth;
        Int32 imageHeight;

        #endregion

        #region ctor

        public SaveImageViewModel(String startFileOrFolder, IContainer container)
            : base(startFileOrFolder, container)
        {
            AddFilter("PNG File (*.png)", "*.png");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the height of the image to use.
        /// </summary>
        public int ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the width of the image ot use.
        /// </summary>
        public int ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = value; RaisePropertyChanged(); }
        }

        #endregion
    }
}
