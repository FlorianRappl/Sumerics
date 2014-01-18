using System;

namespace YAMP.Parser
{
    /// <summary>
    /// All the different categories used by the tokenizer.
    /// </summary>
    enum TokenCategory : ushort
    {
        /// <summary>
        /// An operator like ==, ~=, +, -, ...
        /// </summary>
        Operator,
        /// <summary>
        /// An operator like .*, ./, .\, ...
        /// </summary>
        DotOperator,
        /// <summary>
        /// An operator like =, +=, -=, ...
        /// </summary>
        Assignment,
        /// <summary>
        /// Opening and closing brackets, comma, ...
        /// </summary>
        Group,
        /// <summary>
        /// Strings, Numbers, ...
        /// </summary>
        Literal,
        /// <summary>
        /// Identifier (non-definining), Function, ...
        /// </summary>
        Identifier,
        /// <summary>
        /// Reserved keywords class, function, ...
        /// </summary>
        Keyword
    }
}
