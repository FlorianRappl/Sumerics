using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics
{
    class SaveImageViewModel : SaveFileViewModel
    {
        #region Members

        int imageWidth;
        int imageHeight;

        #endregion

        #region ctor

        public SaveImageViewModel(string startFileOrFolder)
            : base(startFileOrFolder)
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
