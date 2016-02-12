namespace Sumerics.ViewModels
{
    using System;

    sealed class SaveImageViewModel : SaveFileViewModel
    {
        #region Fields

        Int32 _imageWidth;
        Int32 _imageHeight;

        #endregion

        #region ctor

        public SaveImageViewModel(String startFileOrFolder)
            : base(startFileOrFolder)
        {
            AddFilter("PNG File (*.png)", "*.png");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the height of the image to use.
        /// </summary>
        public Int32 ImageHeight
        {
            get { return _imageHeight; }
            set { _imageHeight = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the width of the image ot use.
        /// </summary>
        public Int32 ImageWidth
        {
            get { return _imageWidth; }
            set { _imageWidth = value; RaisePropertyChanged(); }
        }

        #endregion
    }
}
