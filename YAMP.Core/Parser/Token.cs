using System;

namespace YAMP.Parser
{
    /// <summary>
    /// A token class.
    /// </summary>
    sealed class Token
    {
        #region Members

        int _row;
        int _col;
        object _value;
        TokenType _type;
        TokenCategory _category;

        #endregion

        #region ctor

        private Token(object value, TokenType type, TokenCategory category, int row, int column)
        {
            _value = value;
            _type = type;
            _category = category;
            _row = row;
            _col = column;
        }

        #endregion

        #region Creators

        /// <summary>
        /// Gets the comment token.
        /// </summary>
        /// <param name="content">The comment's content.</param>
        /// <param name="row">The starting row of the comment.</param>
        /// <param name="column">The starting column of the comment.</param>
        /// <returns>The created token.</returns>
        public static Token Comment(string content, int row, int column)
        {
            return new Token(content, TokenType.Comment, TokenCategory.Group, row, column);
        }

        /// <summary>
        /// Gets the End-Of-File token.
        /// </summary>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token End(int row, int column)
        {
            return new Token(null, TokenType.EOF, TokenCategory.Group, row, column);
        }

        /// <summary>
        /// Gets the comma token.
        /// </summary>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Comma(int row, int column)
        {
            return new Token(null, TokenType.Comma, TokenCategory.Group, row, column);
        }

        /// <summary>
        /// Gets the semicolon token.
        /// </summary>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Semicolon(int row, int column)
        {
            return new Token(null, TokenType.Semicolon, TokenCategory.Group, row, column);
        }

        /// <summary>
        /// Creates a new assignment token.
        /// </summary>
        /// <param name="type">The type of the operator.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Assignment(TokenType type, int row, int column)
        {
            return new Token(null, type, TokenCategory.Assignment, row, column);
        }

        /// <summary>
        /// Creates an operator token.
        /// </summary>
        /// <param name="type">The type of the operator.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Operator(TokenType type, int row, int column)
        {
            return new Token(null, type, TokenCategory.Operator, row, column);
        }

        /// <summary>
        /// Creates a dot operator token.
        /// </summary>
        /// <param name="type">The type of the dot operator.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token DotOperator(TokenType type, int row, int column)
        {
            return new Token(null, type, TokenCategory.DotOperator, row, column);
        }

        /// <summary>
        /// Creates a real number token.
        /// </summary>
        /// <param name="value">The real value of the number.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Number(Double value, int row, int column)
        {
            return new Token(value, TokenType.Real, TokenCategory.Literal, row, column);
        }

        /// <summary>
        /// Creates a complex number token.
        /// </summary>
        /// <param name="value">The complex value of the number.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Number(Complex value, int row, int column)
        {
            return new Token(value, TokenType.Real, TokenCategory.Literal, row, column);
        }

        /// <summary>
        /// Creates an integer number token.
        /// </summary>
        /// <param name="value">The integer value of the number.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Number(Int64 value, int row, int column)
        {
            return new Token(value, TokenType.Integer, TokenCategory.Literal, row, column);
        }

        /// <summary>
        /// Creates an boolean number token.
        /// </summary>
        /// <param name="value">The boolean value of the number.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Boolean(Boolean value, int row, int column)
        {
            return new Token(value, TokenType.Boolean, TokenCategory.Literal, row, column);
        }

        /// <summary>
        /// Creates a new string token.
        /// </summary>
        /// <param name="value">The value of the string.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token String(String value, int row, int column)
        {
            return new Token(value, TokenType.String, TokenCategory.Literal, row, column);
        }

        /// <summary>
        /// Creates a new identifier token.
        /// </summary>
        /// <param name="identifier">The value of the string.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Identifier(String identifier, int row, int column)
        {
            return new Token(identifier, TokenType.String, TokenCategory.Identifier, row, column);
        }

        /// <summary>
        /// Creates a new keyword token.
        /// </summary>
        /// <param name="type">The type of the keyword.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Keyword(TokenType type, int row, int column)
        {
            return new Token(null, type, TokenCategory.Keyword, row, column);
        }

        /// <summary>
        /// Creates a new bracket token.
        /// </summary>
        /// <param name="type">The exact bracket type.</param>
        /// <param name="row">The row of token.</param>
        /// <param name="column">The column of the token.</param>
        /// <returns>The created token.</returns>
        public static Token Bracket(TokenType type, int row, int column)
        {
            return new Token(null, type, TokenCategory.Group, row, column);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the column of the token.
        /// </summary>
        public int Column
        {
            get { return _col; }
        }

        /// <summary>
        /// Gets the row of the token.
        /// </summary>
        public int Row
        {
            get { return _row; }
        }

        /// <summary>
        /// Gets the specified type.
        /// </summary>
        public TokenType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the specified category.
        /// </summary>
        public TokenCategory Category
        {
            get { return _category; }
        }

        /// <summary>
        /// Gets the value as a string.
        /// </summary>
        public string ValueAsString 
        {
            get { return _value != null ? _value.ToString() : ToCode(); }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public object Value
        {
            get { return _value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// A string containing the important token information.
        /// </summary>
        /// <returns>The string with the information.</returns>
        public override string ToString()
        {
            return string.Format("Ln {3} Col {4} : ({0}, {1}) [ {2} ]", _type, _category, _value, _row, _col);
        }

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public string ToCode()
        {
            switch (_type)
            {
                case TokenType.Adjungate:
                    return "'";
                case TokenType.And:
                    return "&&";
                case TokenType.Dot:
                    return ".";
                case TokenType.Assignment:
                    return "=";
                case TokenType.Add:
                    return _category == TokenCategory.Assignment ? "+=" : "+";
                case TokenType.LeftDivide:
                    return _category == TokenCategory.Assignment ? "\\=" : "\\";
                case TokenType.Modulo:
                    return _category == TokenCategory.Assignment ? "%=" : "%";
                case TokenType.Multiply:
                    return _category == TokenCategory.Assignment ? "*=" : "*";
                case TokenType.Power:
                    return _category == TokenCategory.Assignment ? "^=" : "^";
                case TokenType.RightDivide:
                    return _category == TokenCategory.Assignment ? "/=" : "/";
                case TokenType.Subtract:
                    return _category == TokenCategory.Assignment ? "-=" : "-";
                case TokenType.Range:
                    return ":";
                case TokenType.Or:
                    return "||";
                case TokenType.Equal:
                    return "==";
                case TokenType.NotEqual:
                    return "~=";
                case TokenType.LessEqual:
                    return "<=";
                case TokenType.LessThan:
                    return "<";
                case TokenType.GreaterEqual:
                    return ">=";
                case TokenType.GreaterThan:
                    return ">";
                case TokenType.Break:
                    return "break";
                case TokenType.Class:
                    return "class";
                case TokenType.Function:
                    return "function";
                case TokenType.Do:
                    return "do";
                case TokenType.While:
                    return "while";
                case TokenType.For:
                    return "for";
                case TokenType.Let:
                    return "let";
                case TokenType.Return:
                    return "return";
                case TokenType.If:
                    return "if";
                case TokenType.Else:
                    return "else";
                case TokenType.Integer:
                    return _value.ToString();
                case TokenType.Real:
                    return _value.ToString();
                case TokenType.Complex:
                    return ((Complex)_value).Im.ToString() + "i";
                case TokenType.Boolean:
                    return _value.ToString();
                case TokenType.Increment:
                    return "++";
                case TokenType.Decrement:
                    return "--";
                case TokenType.Comma:
                    return ",";
                case TokenType.Transpose:
                    return ".'";
                case TokenType.OpenBracket:
                    return "(";
                case TokenType.OpenArray:
                    return "[";
                case TokenType.OpenScope:
                    return "{";
                case TokenType.CloseBracket:
                    return ")";
                case TokenType.CloseArray:
                    return "]";
                case TokenType.CloseScope:
                    return "}";
                case TokenType.Factorial:
                    return "!";
                case TokenType.FatArrow:
                    return "=>";
                case TokenType.Semicolon:
                    return ";";
                case TokenType.String:
                    return _category == TokenCategory.Identifier ? ValueAsString : "\"" + ValueAsString + "\"";
                case TokenType.Condition:
                    return "?";
                case TokenType.Whitespace:
                    return " ";
                case TokenType.Comment:
                    return ValueAsString;
            }

            return string.Empty;
        }

        #endregion
    }
}
