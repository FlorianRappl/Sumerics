using System;

namespace YAMP.Parser
{
    /// <summary>
    /// A list of possible token types.
    /// </summary>
    enum TokenType : ushort
    {
        /// <summary>
        /// A whitespace character.
        /// </summary>
        Whitespace,
        /// <summary>
        /// A comment token.
        /// </summary>
        Comment,
        /// <summary>
        /// A comma character.
        /// </summary>
        Comma,
        /// <summary>
        /// A semicolon character.
        /// </summary>
        Semicolon,
        /// <summary>
        /// The dot character.
        /// </summary>
        Dot,
        /// <summary>
        /// The plus operator.
        /// </summary>
        Add,
        /// <summary>
        /// The minus operator.
        /// </summary>
        Subtract,
        /// <summary>
        /// The times operator *.
        /// </summary>
        Multiply,
        /// <summary>
        /// The left division \ operator.
        /// </summary>
        LeftDivide,
        /// <summary>
        /// The right division / operator.
        /// </summary>
        RightDivide,
        /// <summary>
        /// The modulo (%) operator.
        /// </summary>
        Modulo,
        /// <summary>
        /// The range (:) operator.
        /// </summary>
        Range,
        /// <summary>
        /// The power (^) operator.
        /// </summary>
        Power,
        /// <summary>
        /// The transpose (.') operator.
        /// </summary>
        Transpose,
        /// <summary>
        /// The adjungate (') operator.
        /// </summary>
        Adjungate,
        /// <summary>
        /// The increment (++) operator.
        /// </summary>
        Increment,
        /// <summary>
        /// The decrement (--) operator.
        /// </summary>
        Decrement,
        /// <summary>
        /// The assignment (=) operator.
        /// </summary>
        Assignment,
        /// <summary>
        /// The equals (==) operator.
        /// </summary>
        Equal,
        /// <summary>
        /// The not equals (~=) operator.
        /// </summary>
        NotEqual,
        /// <summary>
        /// The factorial or not (!) operator.
        /// </summary>
        Factorial,
        /// <summary>
        /// The greater-than operator.
        /// </summary>
        GreaterThan,
        /// <summary>
        /// The less-than operator.
        /// </summary>
        LessThan,
        /// <summary>
        /// The greater-or-equal-to operator.
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// The less-or-equal-to operator.
        /// </summary>
        LessEqual,
        /// <summary>
        /// The ternary condition (?) operator.
        /// </summary>
        Condition,
        /// <summary>
        /// The fat arrow operator.
        /// </summary>
        FatArrow,
        /// <summary>
        /// The absolute pipe.
        /// </summary>
        Abs,
        /// <summary>
        /// The or operator.
        /// </summary>
        Or,
        /// <summary>
        /// The and operator.
        /// </summary>
        And,
        /// <summary>
        /// Boolean number.
        /// </summary>
        Boolean,
        /// <summary>
        /// Integer number.
        /// </summary>
        Integer,
        /// <summary>
        /// Double number.
        /// </summary>
        Real,
        /// <summary>
        /// Complex (double) number.
        /// </summary>
        Complex,
        /// <summary>
        /// A string.
        /// </summary>
        String,
        /// <summary>
        /// The open array [ character.
        /// </summary>
        OpenArray,
        /// <summary>
        /// The close array ] character.
        /// </summary>
        CloseArray,
        /// <summary>
        /// The open scope { character.
        /// </summary>
        OpenScope,
        /// <summary>
        /// The close scope } character.
        /// </summary>
        CloseScope,
        /// <summary>
        /// The open bracket ( character.
        /// </summary>
        OpenBracket,
        /// <summary>
        /// The close bracket ) character.
        /// </summary>
        CloseBracket,
        /// <summary>
        /// The if keyword.
        /// </summary>
        If,
        /// <summary>
        /// The else keyword.
        /// </summary>
        Else,
        /// <summary>
        /// The for keyword.
        /// </summary>
        For,
        /// <summary>
        /// The while keyword.
        /// </summary>
        While,
        /// <summary>
        /// The function keyword.
        /// </summary>
        Function,
        /// <summary>
        /// The class keyword.
        /// </summary>
        Class,
        /// <summary>
        /// The do keyword.
        /// </summary>
        Do,
        /// <summary>
        /// The let keyword.
        /// </summary>
        Let,
        /// <summary>
        /// The break keyword.
        /// </summary>
        Break,
        /// <summary>
        /// The return keyword.
        /// </summary>
        Return,
        /// <summary>
        /// The EOF token.
        /// </summary>
        EOF,
    }
}
