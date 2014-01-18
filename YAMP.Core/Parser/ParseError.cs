using System;

namespace YAMP.Parser
{
    /// <summary>
    /// A structure encapsulating data of a parse error.
    /// </summary>
    struct ParseError
    {
        /// <summary>
        /// Creates a new parse error object with these properties.
        /// </summary>
        /// <param name="code">The code of the error.</param>
        /// <param name="row">The row of the error.</param>
        /// <param name="column">The column of the error.</param>
        public ParseError(ErrorCode code, int row, int column) : this()
        {
            Code = code;
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Gets or sets the row of the error.
        /// </summary>
        public int Row
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the column of the error.
        /// </summary>
        public int Column
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the code of the error.
        /// </summary>
        public ErrorCode Code
        {
            get;
            set;
        }

        /// <summary>
        /// Transforms the error information into a string.
        /// </summary>
        /// <returns>The string with the information.</returns>
        public override string ToString()
        {
            return string.Format("({0}, {1}) ERR {2}.", Row, Column, Code);
        }
    }
}
