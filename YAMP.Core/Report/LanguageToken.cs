using System;

namespace YAMP.Report
{
    /// <summary>
    /// Represents the tokens found by the compilation.
    /// </summary>
    public sealed class LanguageToken
    {
        /// <summary>
        /// Gets or sets the content of the token.
        /// </summary>
        public String Content 
        { 
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the length of the token.
        /// </summary>
        public Int32 Length
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets in which column the token starts.
        /// </summary>
        public Int32 Column
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets in which row the token starts.
        /// </summary>
        public Int32 Row
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the category of the token.
        /// </summary>
        public LanguageTokenCategory Category
        {
            get;
            set;
        }
    }
}
