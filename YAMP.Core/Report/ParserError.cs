using System;

namespace YAMP.Report
{
    /// <summary>
    /// Represents a compilation error.
    /// </summary>
    public class ParserError
    {
        /// <summary>
        /// Gets or sets the message of the compilation error.
        /// </summary>
        public String Message 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the code of the compilation error.
        /// </summary>
        public Int32 Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the column of the compilation error.
        /// </summary>
        public Int32 Column
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the row of the compilation error.
        /// </summary>
        public Int32 Row
        {
            get;
            set;
        }
    }
}
