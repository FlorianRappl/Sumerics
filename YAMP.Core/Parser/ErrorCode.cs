using System;

namespace YAMP.Parser
{
    /// <summary>
    /// A list of possible error codes for the parser.
    /// </summary>
    enum ErrorCode
    {
        /// <summary>
        /// The given character is unexpected.
        /// </summary>
        UnexpectedCharacter = 0x10,
        /// <summary>
        /// The identifier's name is reserved.
        /// </summary>
        NameReserved = 0x11,
        /// <summary>
        /// An operator has been expected on this position.
        /// </summary>
        OperatorExpected = 0x12,
        /// <summary>
        /// An identifier has been expected on this position.
        /// </summary>
        IdentifierExpected = 0x13,
        /// <summary>
        /// A comma has been expected on this position.
        /// </summary>
        CommaExpected = 0x14,
        /// <summary>
        /// More input is required.
        /// </summary>
        InputMissing = 0x15,
        /// <summary>
        /// A parantheses is required.
        /// </summary>
        MissingParantheses = 0x16,
        /// <summary>
        /// A required keyword is missing.
        /// </summary>
        KeywordMissing = 0x17,
        /// <summary>
        /// The statement terminator is missing.
        /// </summary>
        TerminatorMissing = 0x18,
        /// <summary>
        /// The given comma has been placed on an unexpected position.
        /// </summary>
        CommaUnexpected = 0x19,
        /// <summary>
        /// Another operand has been expected.
        /// </summary>
        OperandExpected = 0x1a,
        /// <summary>
        /// A closing bracket is missing.
        /// </summary>
        BracketMissing = 0x20,
        /// <summary>
        /// A starting bracket is missing.
        /// </summary>
        BracketUnexpected = 0x21,
        /// <summary>
        /// The escape sequence is not recognized.
        /// </summary>
        EscapeSequenceUnrecognized = 0x30,
        /// <summary>
        /// The given string is not terminated.
        /// </summary>
        StringNotTerminated = 0x31,
        /// <summary>
        /// The given bracket is empty.
        /// </summary>
        EmptyBracket = 0x32,
        /// <summary>
        /// The variable list is not well-formatted. It should only consist of a single row with identifiers as columns.
        /// </summary>
        VariableList = 0x33,
        /// <summary>
        /// An else statement requires an if-statement before.
        /// </summary>
        ElseRequiresIf = 0x34,
        /// <summary>
        /// The specified name has already been defined.
        /// </summary>
        NameAlreadyDefined = 0x35,
    }
}
