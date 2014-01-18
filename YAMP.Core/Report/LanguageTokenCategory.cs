using System;

namespace YAMP.Report
{
    /// <summary>
    /// Represents the categories of tokens.
    /// </summary>
    public enum LanguageTokenCategory : ushort
    {
        /// <summary>
        /// A string (or string literal) in quotation marks.
        /// </summary>
        String,
        /// <summary>
        /// A number literal.
        /// </summary>
        Number,
        /// <summary>
        /// An identifier, either a variable or function.
        /// </summary>
        Identifier,
        /// <summary>
        /// A reserved keyword.
        /// </summary>
        Keyword,
        /// <summary>
        /// An operator.
        /// </summary>
        Operator,
        /// <summary>
        /// A comment block.
        /// </summary>
        Comment,
        /// <summary>
        /// Something different.
        /// </summary>
        Other
    }
}
