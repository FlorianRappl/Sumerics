/*
	Copyright (c) 2012-2013, Florian Rappl et al.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Text;
using YAMP.Expressions;
using YAMP.Statements;

namespace YAMP.Parser
{
    /// <summary>
    /// Performs the tokenization and parsing of M# code.
    /// </summary>
    sealed class Frontend
    {
        #region Members

        char[] _source;
        Stack<int> _cols;
        Stack<TokenType> _brackets;
        Stack<Token> _tokenBuffer;
        int _col;
        int _row;
        int _position;
        int _length;
        char _current;
        StatementList _result;
        List<ParseError> _errors;
        int _ticks;
        int _tokens;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new parser.
        /// </summary>
        private Frontend()
        {
            _tokenBuffer = new Stack<Token>();
            _brackets = new Stack<TokenType>();
            _cols = new Stack<int>();
            _col = 1;
            _row = 1;
            _position = 0;
            _result = new StatementList();
            _errors = new List<ParseError>();
        }

        /// <summary>
        /// Creates a new parser with the given source code.
        /// </summary>
        /// <param name="source">The code to parse.</param>
        private Frontend(char[] source)
            : this()
        {
            _source = source;
            _length = _source.Length;
            _current = _length != 0 ? _source[0] : Chars.EOF;
        }

        /// <summary>
        /// Creates a new parser with the given source code.
        /// </summary>
        /// <param name="source">The code to parse.</param>
        public Frontend(string source)
            : this()
        {
            _source = source.ToCharArray();
            _length = _source.Length;
            _current = _length != 0 ? _source[0] : Chars.EOF;
        }

        /// <summary>
        /// Creates a new parser with the given source code.
        /// </summary>
        /// <param name="source">The code to parse.</param>
        public Frontend(MathQuery query)
            : this()
        {
            _source = query.Text.ToCharArray();
            _length = _source.Length;
            _current = _length != 0 ? _source[0] : Chars.EOF;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of tokens.
        /// </summary>
        public int TokenCount
        {
            get { return _tokens; }
        }

        /// <summary>
        /// Gets the compilation time in ticks.
        /// </summary>
        public int Ticks
        {
            get { return _ticks; }
        }

        /// <summary>
        /// Gets the tokens of this source code.
        /// </summary>
        public IEnumerable<Token> Tokens
        {
            get
            {
                var copy = new Frontend(_source);
                Token token;

                do
                {
                    do token = copy.GetToken();
                    while (token == null);
                    yield return token;
                }
                while (token.Type != TokenType.EOF);
            }
        }

        /// <summary>
        /// Gets the next token.
        /// </summary>
        IEnumerable<Token> NextToken
        {
            get
            {
                _tokens = 0;
                Token token;
                bool abs = false;

                do
                {
                    do
                    {
                        token = GetToken();
                    }
                    while (token == null);

                    switch (token.Type)
                    {
                        case TokenType.Comment:
                            continue;

                        case TokenType.Abs:
                            if (abs)
                            {
                                abs = false;
                                token = Token.Bracket(TokenType.CloseBracket, token.Row, token.Column);
                                goto case TokenType.CloseBracket;
                            }
                            else
                            {
                                abs = true;
                                yield return Token.Identifier("abs", token.Row, token.Column);
                                token = Token.Bracket(TokenType.OpenBracket, token.Row, token.Column);
                                goto case TokenType.OpenBracket;
                            }
                        case TokenType.OpenBracket:
                            _brackets.Push(TokenType.CloseBracket);
                            break;
                        case TokenType.OpenScope:
                            _brackets.Push(TokenType.CloseScope);
                            break;

                        case TokenType.CloseBracket:
                        case TokenType.CloseScope:
                            var tk = TokenType.Whitespace;

                            while (_brackets.Count > 0 && (tk = _brackets.Pop()) != token.Type)
                            {
                                _errors.Add(new ParseError(ErrorCode.BracketMissing, token.Row, token.Column));
                                yield return Token.Bracket(tk, token.Row, token.Column);
                            }

                            if (tk != token.Type)
                            {
                                _errors.Add(new ParseError(ErrorCode.BracketUnexpected, token.Row, token.Column));
                                continue;
                            }

                            break;
                    }

                    yield return token;
                }
                while (token.Type != TokenType.EOF);
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        public StatementList Result
        {
            get { return _result; }
        }

        /// <summary>
        /// Gets the encountered errors of the parser.
        /// </summary>
        public IEnumerable<ParseError> Errors
        {
            get { foreach (var error in _errors) yield return error; }
        }

        /// <summary>
        /// Gets the number of errors that have been encountered.
        /// </summary>
        public int ErrorCount
        {
            get { return _errors.Count; }
        }

        /// <summary>
        /// Gets if the parser has encountered any errors.
        /// </summary>
        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        #endregion

        #region Stream Manipulation

        /// <summary>
        /// Advances in the source.
        /// </summary>
        void Advance()
        {
            if (_current == Chars.LineFeed)
            {
                _cols.Push(_col);
                _col = 1;
                _row++;
            }
            else
            {
                _col++;
            }

            _position++;
            _current = _position < _length ? _source[_position] : Chars.EOF;
        }

        /// <summary>
        /// Goes back in the source.
        /// </summary>
        void Back()
        {
            _position--;
            _current = _position >= 0 ? _source[_position] : Chars.NullPtr;

            if (_current == Chars.LineFeed)
            {
                _col = _cols.Pop();
                _row--;
            }
            else
            {
                _col--;
            }
        }

        /// <summary>
        /// Skips to the end of block comment starting (until */ is found).
        /// </summary>
        /// <returns>The block comment.</returns>
        Token SkipBlockComment()
        {
            var col = _col - 2;
            var row = _row;
            var pos = _position - 2;
            char _next;

            do
            {
                if (_position == _length)
                    break;
                else if (_position + 1 == _length)
                {
                    Advance();
                    break;
                }

                Advance();

                if (_position + 1 == _length)
                {
                    Advance();
                    break;
                }

                _next = _source[_position + 1];

                if (_current == Chars.Asterisk && _next == Chars.Slash)
                {
                    Advance();
                    Advance();
                    break;
                }
            }
            while (true);

            return Token.Comment(new string(_source, pos, _position - pos), row, col);
        }

        /// <summary>
        /// Skips to the end of a line comment (until the next line is reached).
        /// </summary>
        /// <returns>The line comment.</returns>
        Token SkipLineComment()
        {
            var col = _col - 2;
            int row = _row;
            var pos = _position - 2;

            do
            {
                if (_position == _length)
                    break;

                Advance();
            }
            while (_row == row);

            return Token.Comment(new string(_source, pos, _position - pos), row, col);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Runs the parser with the given query.
        /// </summary>
        /// <param name="query">The query to parse.</param>
        /// <returns>The parser instance.</returns>
        public static Frontend Run(string query)
        {
            var parser = new Frontend(query);
            parser.Run();
            return parser;
        }

        /// <summary>
        /// Resets the state of the parser.
        /// </summary>
        public void Reset()
        {
            _cols.Clear();
            _brackets.Clear();
            _col = 1;
            _row = 1;
            _position = 0;
            _current = _length != 0 ? _source[0] : Chars.EOF;
            _result = new StatementList();
            _errors.Clear();

        }

        /// <summary>
        /// Runs the parser of the source.
        /// </summary>
        public void Run()
        {
            var init = Environment.TickCount;
            var tokens = NextToken.GetEnumerator();
            ParseStatements(tokens);

            while (_brackets.Count > 0)
            {
                _brackets.Pop();
                _errors.Add(new ParseError(ErrorCode.BracketMissing, _row, _col));
            }

            _ticks = Environment.TickCount - init;
        }

        #endregion

        #region Tokenization

        Token GetToken()
        {
            _tokens++;

            // eat white space 
            while (_position < _length && _current <= Chars.Space)
                Advance();

            // are we done?
            if (_current == Chars.EOF)
                return Token.End(_row, _col);

            // operators
            // this gets called a lot, so it's pretty optimized.
            // note that operators must start with non-letter/digit characters.
            var row = _row;
            var col = _col;
            var isLetter = char.IsLetter(_current);
            var isDigit = char.IsDigit(_current);

            if (!isLetter && !isDigit)
            {
                // if this is a number starting with a decimal, don't parse as operator
                var nxt = _position + 1 < _length ? _source[_position + 1] : Chars.EOF;

                if (_current == Chars.FullStop)
                {
                    if (!char.IsDigit(nxt))
                    {
                        Advance();

                        switch (nxt)
                        {
                            case Chars.SingleQuotationMark:
                                Advance();
                                return Token.Operator(TokenType.Transpose, row, col);

                            case Chars.CircumflexAccent:
                                Advance();
                                return Token.DotOperator(TokenType.Power, row, col);

                            case Chars.Asterisk:
                                Advance();
                                return Token.DotOperator(TokenType.Multiply, row, col);

                            case Chars.Slash:
                                Advance();
                                return Token.DotOperator(TokenType.RightDivide, row, col);

                            case Chars.Backslash:
                                Advance();
                                return Token.DotOperator(TokenType.LeftDivide, row, col);

                            default:
                                return Token.Operator(TokenType.Dot, row, col);
                        }
                    }
                }
                else
                {
                    switch (_current)
                    {
                        case Chars.Comma:
                            Advance();
                            return Token.Comma(row, col);

                        case Chars.Plus:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Assignment(TokenType.Add, row, col);
                            }
                            else if (nxt == Chars.Plus)
                            {
                                Advance();
                                return Token.Operator(TokenType.Increment, row, col);
                            }

                            return Token.Operator(TokenType.Add, row, col);

                        case Chars.Minus:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Assignment(TokenType.Subtract, row, col);
                            }
                            else if (nxt == Chars.Minus)
                            {
                                Advance();
                                return Token.Operator(TokenType.Decrement, row, col);
                            }

                            return Token.Operator(TokenType.Subtract, row, col);

                        case Chars.GreaterThan:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Operator(TokenType.GreaterEqual, row, col);
                            }

                            return Token.Operator(TokenType.GreaterThan, row, col);

                        case Chars.LessThan:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Operator(TokenType.LessEqual, row, col);
                            }

                            return Token.Operator(TokenType.LessThan, row, col);

                        case Chars.CircumflexAccent:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Assignment(TokenType.Power, row, col);
                            }

                            return Token.Operator(TokenType.Power, row, col);

                        case Chars.ExclamationMark:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Operator(TokenType.NotEqual, row, col);
                            }

                            return Token.Operator(TokenType.Factorial, row, col);

                        case Chars.Slash:
                            Advance();

                            switch (nxt)
                            {
                                case Chars.Equal:
                                    Advance();
                                    return Token.Assignment(TokenType.RightDivide, row, col);

                                case Chars.Asterisk:
                                    Advance();
                                    return SkipBlockComment();

                                case Chars.Slash:
                                    Advance();
                                    return SkipLineComment();

                                default:
                                    return Token.Operator(TokenType.RightDivide, row, col);
                            }

                        case Chars.Backslash:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Assignment(TokenType.LeftDivide, row, col);
                            }

                            return Token.Operator(TokenType.LeftDivide, row, col);

                        case Chars.Asterisk:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Assignment(TokenType.Multiply, row, col);
                            }

                            return Token.Operator(TokenType.Multiply, row, col);

                        case Chars.Percent:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Assignment(TokenType.Modulo, row, col);
                            }

                            return Token.Operator(TokenType.Modulo, row, col);

                        case Chars.Colon:
                            Advance();
                            return Token.Operator(TokenType.Range, row, col);

                        case Chars.SemiColon:
                            Advance();
                            return Token.Semicolon(row, col);

                        case Chars.Equal:
                            Advance();

                            if (nxt == Chars.Equal)
                            {
                                Advance();
                                return Token.Operator(TokenType.Equal, row, col);
                            }
                            else if (nxt == Chars.GreaterThan)
                            {
                                Advance();
                                return Token.Operator(TokenType.FatArrow, row, col);
                            }

                            return Token.Assignment(TokenType.Assignment, row, col);

                        case Chars.QuestionMark:
                            Advance();
                            return Token.Operator(TokenType.Condition, row, col);

                        case Chars.Pipe:
                            Advance();

                            if (nxt == Chars.Pipe)
                            {
                                Advance();
                                return Token.Operator(TokenType.Or, row, col);
                            }

                            //_errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, _row, _col));
                            return Token.Operator(TokenType.Abs, _row, _col);

                        case Chars.Ampersand:
                            Advance();

                            if (nxt == Chars.Ampersand)
                            {
                                Advance();
                                return Token.Operator(TokenType.And, row, col);
                            }

                            _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, _row, _col));
                            return null;

                        case Chars.SingleQuotationMark:
                            Advance();
                            return Token.Operator(TokenType.Adjungate, row, col);

                        case Chars.OpenBracket:
                            Advance();
                            return Token.Bracket(TokenType.OpenBracket, row, col);

                        case Chars.CloseBracket:
                            Advance();
                            return Token.Bracket(TokenType.CloseBracket, row, col);

                        case Chars.OpenArray:
                            Advance();
                            return Token.Bracket(TokenType.OpenArray, row, col);

                        case Chars.CloseArray:
                            Advance();
                            return Token.Bracket(TokenType.CloseArray, row, col);

                        case Chars.OpenScope:
                            Advance();
                            return Token.Bracket(TokenType.OpenScope, row, col);

                        case Chars.CloseScope:
                            Advance();
                            return Token.Bracket(TokenType.CloseScope, row, col);
                    }
                }
            }

            // parse numbers
            if (isDigit || _current == Chars.FullStop)
            {
                var cmplx = false;
                var sci = 0;
                var div = -1.0; // use double, not int (this may get really big)
                var val = 0L;
                var tmp = 0L;

                while(_position < _length)
                {
                    // digits always OK
                    if (char.IsDigit(_current))
                    {
                        val = val * 10 + (_current - '0');
                        if (div > -1.0 && sci == 0.0) div *= 10.0;
                    }
                    else if(_current == 'i' || _current == 'I')
                    {
                        cmplx = true;
                        Advance();
                        break;
                    }
                    else if (sci != 0)
                    {
                        break;
                    }
                    else if (_position + 1 < _length)
                    {
                        var nxt = _source[_position + 1];

                        // one decimal is OK
                        if (_current == Chars.FullStop && div < 0 && char.IsDigit(nxt))
                        {
                            div = 1.0;
                        }
                        // scientific notation?
                        else if ((_current == 'E' || _current == 'e') && sci == 0)
                        {
                            sci = -1;
                            tmp = val;
                            val = 0L;

                            if (nxt == Chars.Plus || nxt == Chars.Minus)
                            {
                                sci = nxt == Chars.Minus ? 1 : -1;
                                Advance();
                            }
                        }
                        else// end of literal
                            break;
                    }
                    else// end of literal
                        break;

                    Advance();
                }

                var divide = div > 1;

                if (sci != 0)
                {
                    div = (div < 1 ? 1 : div) * Math.Pow(10.0, sci * val);
                    val = tmp;
                    divide = true;
                }

                // end of number, get value
                if (cmplx)
                    return divide ? Token.Number(new Complex(0.0, val / div), row, col) : Token.Number(new Complex(0.0, val), row, col);

                return divide ? Token.Number(val / div, row, col) : Token.Number(val, row, col);
            }

            // parse strings
            if (IsStringStart(Chars.DoubleQuotationMark))
                return ScanString(Chars.DoubleQuotationMark, _current == Chars.At, row, col);

            return ScanIdentifier(row, col, isLetter);
        }

        #endregion

        #region Scanning

        Token ScanIdentifier(Int32 row, Int32 col, Boolean isLetter)
        {
            // identifiers (functions, objects) must start with alpha or underscore or dollar.
            if (!isLetter && _current != Chars.Lowline && _current != Chars.Dollar)
            {
                _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, row, col));
                return null;
            }

            var start = _position;
            var length = 1;
            Advance();

            // and must contain only letters/digits/_idChars
            while (_position < _length)
            {
                if (!Char.IsLetterOrDigit(_current) && _current != Chars.Lowline && _current != Chars.Dollar)
                    break;

                length++;
                Advance();
            }

            var s = new string(_source, start, length);

            switch (s)
            {
                // got boolean
                case "true":
                    return Token.Boolean(true, row, col);
                case "false":
                    return Token.Boolean(false, row, col);
                // got keyword
                case "if":
                    return Token.Keyword(TokenType.If, row, col);
                case "else":
                    return Token.Keyword(TokenType.Else, row, col);
                case "for":
                    return Token.Keyword(TokenType.For, row, col);
                case "while":
                    return Token.Keyword(TokenType.While, row, col);
                case "function":
                    return Token.Keyword(TokenType.Function, row, col);
                case "class":
                    return Token.Keyword(TokenType.Class, row, col);
                case "do":
                    return Token.Keyword(TokenType.Do, row, col);
                case "let":
                    return Token.Keyword(TokenType.Let, row, col);
                case "return":
                    return Token.Keyword(TokenType.Return, row, col);
                case "break":
                    return Token.Keyword(TokenType.Break, row, col);

                // got identifier
                default:
                    return Token.Identifier(s, row, col);
            }
        }

        Boolean IsStringStart(Char start)
        {
            return _current == start || (_position + 1 < _length && _current == Chars.At && _source[_position + 1] == start);
        }

        Token ScanString(Char end, Boolean literal, Int32 row, Int32 col)
        {
            var escape = false;
            var terminated = false;
            var sb = new StringBuilder();

            if (literal)
                Advance();

            // look for end quote, skip double quotes
            while (_position < _length)
            {
                Advance();

                if (escape)
                {
                    switch (_current)
                    {
                        case 't':
                            sb.Append("\t");
                            break;
                        case 'n':
                            sb.AppendLine();
                            break;
                        case Chars.Backslash:
                            sb.Append(Chars.Backslash);
                            break;
                        default:
                            if (_current == end)
                                sb.Append(end);
                            else
                                _errors.Add(new ParseError(ErrorCode.EscapeSequenceUnrecognized, _row, _col));
                            break;
                    }

                    escape = false;
                }
                else
                {
                    if (!literal && _current == Chars.Backslash)
                        escape = true;
                    else if (_current == end)
                    {
                        terminated = true;
                        Advance();
                        break;
                    }
                    else
                        sb.Append(_current);
                }
            }

            // check that we got the end of the string
            if (!terminated)
                _errors.Add(new ParseError(ErrorCode.StringNotTerminated, _row, _col));

            // end of string
            return Token.String(sb.ToString(), row, col);
        }

        #endregion

        #region Parsing

        /// <summary>
        /// Parses as many statements as possible until the EOF or
        /// a scope closing bracket is found.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        void ParseStatements(IEnumerator<Token> tokens)
        {
            do
            {
                tokens.MoveNext();
                var statement = ParseStatement(tokens);

                if (statement != null)
                    _result.Add(statement);
            }
            while (tokens.Current.Type == TokenType.Semicolon || tokens.Current.Type == TokenType.CloseScope);

            if (tokens.Current.Type != TokenType.EOF)
                _errors.Add(new ParseError(ErrorCode.TerminatorMissing, tokens.Current.Row, tokens.Current.Column));
        }

        /// <summary>
        /// Parses a statement starting at the first letter or bracket.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The statement.</returns>
        Statement ParseStatement(IEnumerator<Token> tokens)
        {
            if (tokens.Current.Type == TokenType.OpenScope)
                return ParseScope(tokens);
            else if (tokens.Current.Type == TokenType.Semicolon)
                return null;
            else if (tokens.Current.Type == TokenType.EOF)
                return null;
            else if (tokens.Current.Category == TokenCategory.Keyword)
            {
                switch (tokens.Current.Type)
                {
                    case TokenType.Class:
                        return ParseClass(tokens);

                    case TokenType.Function:
                        return ParseFunction(tokens);

                    case TokenType.If:
                        return ParseIf(tokens);

                    case TokenType.Else:
                        return ParseElse(tokens);

                    case TokenType.For:
                        return ParseFor(tokens);

                    case TokenType.Break:
                        return ParseBreak(tokens);

                    case TokenType.Return:
                        return ParseReturn(tokens);

                    case TokenType.While:
                        return ParseWhile(tokens);

                    case TokenType.Do:
                        return ParseDo(tokens);

                    case TokenType.Let:
                        return ParseLet(tokens);

                    default:
                        _errors.Add(new ParseError(ErrorCode.NameReserved, tokens.Current.Row, tokens.Current.Column));
                        tokens.MoveNext();
                        break;
                }
            }

            return ParseExpression(tokens);
        }

        /// <summary>
        /// Parses a let statement starting at the keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The let statement.</returns>
        LetStatement ParseLet(IEnumerator<Token> tokens)
        {
            var ls = new LetStatement(tokens.Current);
            tokens.MoveNext();

            if (tokens.Current.Category != TokenCategory.Identifier)
            {
                _errors.Add(new ParseError(ErrorCode.IdentifierExpected, tokens.Current.Row, tokens.Current.Column));
                return ls;
            }

            ls.Name = tokens.Current;
            tokens.MoveNext();

            if (tokens.Current.Type == TokenType.Semicolon)
                return ls;

            if (tokens.Current.Type != TokenType.Assignment)
            {
                _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, tokens.Current.Row, tokens.Current.Column));
                return ls;
            }

            tokens.MoveNext();
            ls.Assignment = ParseExpression(tokens);
            return ls;
        }

        /// <summary>
        /// Parses a do-while loop starting at the do keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The do-while-loop statement.</returns>
        DoLoopStatement ParseDo(IEnumerator<Token> tokens)
        {
            var ds = new DoLoopStatement(tokens.Current);
            tokens.MoveNext();
            ParseInNewList(ds, tokens);

            if (tokens.Current.Type != TokenType.While)
            {
                _errors.Add(new ParseError(ErrorCode.KeywordMissing, tokens.Current.Row, tokens.Current.Column));
                return ds;
            }

            tokens.MoveNext();

            if (tokens.Current.Type != TokenType.OpenBracket)
            {
                _errors.Add(new ParseError(ErrorCode.MissingParantheses, tokens.Current.Row, tokens.Current.Column));
                return ds;
            }

            tokens.MoveNext();
            ds.Condition = ParseExpression(tokens);

            if (tokens.Current.Type != TokenType.CloseBracket)
            {
                _errors.Add(new ParseError(ErrorCode.BracketMissing, tokens.Current.Row, tokens.Current.Column));
                return ds;
            }

            tokens.MoveNext();
            return ds;
        }

        /// <summary>
        /// Parses a while loop starting at the while keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The while-loop statement.</returns>
        WhileLoopStatement ParseWhile(IEnumerator<Token> tokens)
        {
            var ws = new WhileLoopStatement(tokens.Current);
            tokens.MoveNext();

            if (tokens.Current.Type != TokenType.OpenBracket)
            {
                _errors.Add(new ParseError(ErrorCode.MissingParantheses, tokens.Current.Row, tokens.Current.Column));
                return ws;
            }

            tokens.MoveNext();
            ws.Condition = ParseExpression(tokens);

            if (tokens.Current.Type != TokenType.CloseBracket)
            {
                _errors.Add(new ParseError(ErrorCode.BracketMissing, tokens.Current.Row, tokens.Current.Column));
                return ws;
            }

            tokens.MoveNext();
            ParseInNewList(ws, tokens);
            return ws;
        }

        /// <summary>
        /// Parses a return statement starting at the keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The return statement.</returns>
        ReturnStatement ParseReturn(IEnumerator<Token> tokens)
        {
            var rs = new ReturnStatement(tokens.Current);
            tokens.MoveNext();

            if(tokens.Current.Type == TokenType.Semicolon)
                return rs;
            else
            {
                var x = ParseExpression(tokens);

                if(x != null)
                    rs.Payload = x;
            }

            return rs;
        }

        /// <summary>
        /// Parses a break statement starting at the keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The break statement.</returns>
        BreakStatement ParseBreak(IEnumerator<Token> tokens)
        {
            var bs = new BreakStatement(tokens.Current);
            tokens.MoveNext();
            return bs;
        }

        /// <summary>
        /// Parses a for loop starting at the for keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The for-loop statement.</returns>
        ForLoopStatement ParseFor(IEnumerator<Token> tokens)
        {
            var fs = new ForLoopStatement(tokens.Current);
            tokens.MoveNext();

            if (tokens.Current.Type != TokenType.OpenBracket)
            {
                _errors.Add(new ParseError(ErrorCode.MissingParantheses, tokens.Current.Row, tokens.Current.Column));
                return fs;
            }
            
            tokens.MoveNext();

            if (tokens.Current.Type == TokenType.Let)
                fs.Initializer = ParseLet(tokens);
            else if (tokens.Current.Type != TokenType.Semicolon)
                fs.Initializer = ParseExpression(tokens);

            tokens.MoveNext();

            if (tokens.Current.Type != TokenType.Semicolon)
                fs.Condition = ParseExpression(tokens);

            tokens.MoveNext();

            if (tokens.Current.Type != TokenType.CloseBracket)
            {
                fs.Step = ParseExpression(tokens);

                if (tokens.Current.Type != TokenType.CloseBracket)
                {
                    _errors.Add(new ParseError(ErrorCode.BracketMissing, tokens.Current.Row, tokens.Current.Column));
                    return fs;
                }
            }

            tokens.MoveNext();
            ParseInNewList(fs, tokens);
            return fs;
        }

        /// <summary>
        /// Parses an if-statement starting at the if keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The if statement.</returns>
        ConditionalStatement ParseIf(IEnumerator<Token> tokens)
        {
            var cs = new ConditionalStatement(tokens.Current);
            tokens.MoveNext();

            if (tokens.Current.Type != TokenType.OpenBracket)
            {
                _errors.Add(new ParseError(ErrorCode.MissingParantheses, tokens.Current.Row, tokens.Current.Column));
                return cs;
            }

            cs.Condition = ParseExpression(tokens);
            cs.Body = ParseStatement(tokens);
            return cs;
        }

        /// <summary>
        /// Parses an if-statement starting at the if keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The if statement.</returns>
        ConditionalStatement ParseElse(IEnumerator<Token> tokens)
        {
            var cs = _result.Last as ConditionalStatement;

            if (cs == null)
            {
                cs = new ConditionalStatement(tokens.Current);
                _errors.Add(new ParseError(ErrorCode.ElseRequiresIf, tokens.Current.Row, tokens.Current.Column));
            }

            tokens.MoveNext();
            cs.Otherwise = ParseStatement(tokens);
            return cs;
        }

        /// <summary>
        /// Parses a function starting at the function keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The function statement.</returns>
        FunctionStatement ParseFunction(IEnumerator<Token> tokens)
        {
            var fs = new FunctionStatement(tokens.Current);
            tokens.MoveNext();

            if (tokens.Current.Category == TokenCategory.Identifier)
            {
                fs.Name = tokens.Current;
                tokens.MoveNext();
            }

            if (tokens.Current.Type != TokenType.OpenBracket)
            {
                _errors.Add(new ParseError(ErrorCode.MissingParantheses, tokens.Current.Row, tokens.Current.Column));
                return fs;
            }

            fs.Arguments = ParseArguments(tokens);
            CheckArguments(fs.Arguments);

            if (tokens.Current.Type != TokenType.OpenScope)
            {
                _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, tokens.Current.Row, tokens.Current.Column));
                return fs;
            }

            ParseInNewList(fs, tokens);
            return fs;
        }

        /// <summary>
        /// Parses a function as an expression starting at the function keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The function expression.</returns>
        LambdaExpression ParseFunctionExpression(IEnumerator<Token> tokens)
        {
            var start = tokens.Current;
            tokens.MoveNext();

            if (tokens.Current.Type != TokenType.OpenBracket)
            {
                _errors.Add(new ParseError(ErrorCode.MissingParantheses, tokens.Current.Row, tokens.Current.Column));
                return new LambdaExpression(start, null, null);
            }

            var arguments = ParseArguments(tokens);
            CheckArguments(arguments);

            if (tokens.Current.Type != TokenType.OpenScope)
            {
                _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, tokens.Current.Row, tokens.Current.Column));
                return new LambdaExpression(start, arguments, null);
            }

            var list = ParseScope(tokens);
            return new LambdaExpression(start, arguments, list);
        }

        /// <summary>
        /// Parses a class starting at the class keyword.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The class statement.</returns>
        ClassStatement ParseClass(IEnumerator<Token> tokens)
        {
            var cs = new ClassStatement(tokens.Current);
            tokens.MoveNext();

            if (tokens.Current.Category != TokenCategory.Identifier)
            {
                _errors.Add(new ParseError(ErrorCode.IdentifierExpected, tokens.Current.Row, tokens.Current.Column));
                return cs;
            }

            cs.Name = tokens.Current;
            tokens.MoveNext();

            if (tokens.Current.Type != TokenType.OpenScope)
            {
                _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, tokens.Current.Row, tokens.Current.Column));
                return cs;
            }

            while (true)
            {
                tokens.MoveNext();

                switch (tokens.Current.Type)
                {
                    case TokenType.Function:
                        var func = ParseFunction(tokens);

                        if (func.Name != null && cs.TryAddFunction(func) == false)
                            _errors.Add(new ParseError(ErrorCode.NameAlreadyDefined, func.Name.Row, func.Name.Column));

                        continue;

                    //case TokenType.Class:
                    //    var cls = ParseClass(tokens);

                    //    if (cls.Name != null && cs.TryAddClass(cls) == false)
                    //        _errors.Add(new ParseError(ErrorCode.NameAlreadyDefined, cls.Name.Row, cls.Name.Column));
                        
                    //    continue;

                    case TokenType.Let:
                        var let = ParseLet(tokens);

                        if (let.Name != null && cs.TryAddInitializer(let) == false)
                            _errors.Add(new ParseError(ErrorCode.NameAlreadyDefined, let.Name.Row, let.Name.Column));

                        break;

                    case TokenType.CloseScope:
                    case TokenType.EOF:
                        break;

                    default:
                        _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, tokens.Current.Row, tokens.Current.Column));
                        break;
                }

                if (tokens.Current.Type == TokenType.EOF)
                {
                    _errors.Add(new ParseError(ErrorCode.TerminatorMissing, tokens.Current.Row, tokens.Current.Column));
                    break;
                }
                else if (tokens.Current.Type == TokenType.CloseScope)
                {
                    break;
                }
            }

            return cs;
        }

        /// <summary>
        /// Parses a scope starting at the curly bracket.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The scope statement.</returns>
        Statement ParseScope(IEnumerator<Token> tokens)
        {
            var current = new StatementList();
            ParseInNewList(current, tokens);
            return current;
        }

        /// <summary>
        /// Parses an expression starting at the first operand / operator.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The expression.</returns>
        Expression ParseExpression(IEnumerator<Token> tokens)
        {
            if (tokens.Current.Type == TokenType.Function)
                return ParseFunctionExpression(tokens);
            else if (tokens.Current.Type == TokenType.Range)
            {
                var t = tokens.Current;
                tokens.MoveNext();
                return TernaryExpression.Create(t, new LiteralExpression(Token.Number(0, 0, 0)), null, new VariableExpression(Token.Identifier("end", 0, 0)));
            }

            var x = ParseTernary(tokens);

            // assignment
            if (tokens.Current.Category == TokenCategory.Assignment)
            {
                var token = tokens.Current;
                tokens.MoveNext();
                CheckAssignment(x);
                x = BinaryExpression.Create(token, x, ParseExpression(tokens));
            }

            return x;
        }

        /// <summary>
        /// Parses a ternary (::, ?:) expression starting at the operator.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The ternary or ordinary expression.</returns>
        Expression ParseTernary(IEnumerator<Token> tokens)
        {
            var x = ParseLogical(tokens);

            if (tokens.Current.Type == TokenType.Condition)
            {
                var t = tokens.Current;
                tokens.MoveNext();
                var left = ParseLogical(tokens);

                if (tokens.Current.Type == TokenType.Range)
                {
                    tokens.MoveNext();
                    var right = ParseLogical(tokens);
                    return TernaryExpression.Create(t, x, left, right);
                }
                else
                {
                    _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, tokens.Current.Row, tokens.Current.Column));
                    return BinaryExpression.Create(t, x, left);
                }
            }
            else if(tokens.Current.Type == TokenType.Range)
            {
                var t = tokens.Current;
                tokens.MoveNext();
                var y = ParseLogical(tokens);

                if (tokens.Current.Type == TokenType.Range)
                {
                    tokens.MoveNext();
                    var z = ParseLogical(tokens);
                    return TernaryExpression.Create(t, x, y, z);
                }

                return TernaryExpression.Create(t, x, null, y);
            }

            return x;
        }

        /// <summary>
        /// Parses a logical (&& or ||) expression starting at the operator.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The binary or ordinary expression.</returns>
        Expression ParseLogical(IEnumerator<Token> tokens)
        {
            var x = ParseComparison(tokens);

            while (true)
            {
                switch (tokens.Current.Type)
                {
                    case TokenType.And:
                    case TokenType.Or:
                        break;

                    default:
                        return x;
                }

                var t = tokens.Current;
                tokens.MoveNext();
                var exprArg = ParseComparison(tokens);
                x = BinaryExpression.Create(t, x, exprArg);
            }
        }

        /// <summary>
        /// Parses a comparison expression starting at the operator.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The binary or ordinary expression.</returns>
        Expression ParseComparison(IEnumerator<Token> tokens)
        {
            var x = ParseAddSub(tokens);

            while (true)
            {
                switch (tokens.Current.Type)
                {
                    case TokenType.Equal:
                    case TokenType.NotEqual:
                    case TokenType.GreaterEqual:
                    case TokenType.GreaterThan:
                    case TokenType.LessEqual:
                    case TokenType.LessThan:
                        break;

                    default:
                        return x;
                }

                var t = tokens.Current;
                tokens.MoveNext();
                var exprArg = ParseAddSub(tokens);
                x = BinaryExpression.Create(t, x, exprArg);
            }
        }

        /// <summary>
        /// Parses a addition or subtractionexpression starting at the operator.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The binary or ordinary expression.</returns>
        Expression ParseAddSub(IEnumerator<Token> tokens)
        {
            var x = ParseMulDiv(tokens);

            while (true)
            {
                switch (tokens.Current.Type)
                {
                    case TokenType.Add:
                    case TokenType.Subtract:
                        break;

                    default:
                        return x;
                }

                var t = tokens.Current;
                tokens.MoveNext();
                var exprArg = ParseMulDiv(tokens);
                x = BinaryExpression.Create(t, x, exprArg);
            }
        }

        /// <summary>
        /// Parses a multiplication or division expression starting at the operator.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The binary or ordinary expression.</returns>
        Expression ParseMulDiv(IEnumerator<Token> tokens)
        {
            var x = ParsePower(tokens);

            while (true)
            {
                switch (tokens.Current.Type)
                {
                    case TokenType.Multiply:
                    case TokenType.LeftDivide:
                    case TokenType.RightDivide:
                    case TokenType.Modulo:
                        break;

                    default:
                        return x;
                }

                var t = tokens.Current;
                tokens.MoveNext();
                var a = ParsePower(tokens);
                x = BinaryExpression.Create(t, x, a);
            }
        }

        /// <summary>
        /// Parses a power expression (from right to left) starting at the power
        /// operator ^ or .^.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The binary or ordinary expression.</returns>
        Expression ParsePower(IEnumerator<Token> tokens)
        {
            var y = ParseUnary(tokens);

            if (tokens.Current.Type != TokenType.Power)
                return y;

            var _expressionBuffer = new Stack<Expression>();
            _expressionBuffer.Push(y);

            while (true)
            {
                switch (tokens.Current.Type)
                {
                    case TokenType.Power:
                        break;

                    default:
                        y = _expressionBuffer.Pop();

                        while (_expressionBuffer.Count > 0)
                            y = BinaryExpression.Create(_tokenBuffer.Pop(), _expressionBuffer.Pop(), y);

                        return y;
                }

                _tokenBuffer.Push(tokens.Current);
                tokens.MoveNext();
                _expressionBuffer.Push(ParseUnary(tokens));
            }
        }

        /// <summary>
        /// Parses a (left) unary expression starting at the
        /// unary operator token.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The unary expression or ordinary expression.</returns>
        Expression ParseUnary(IEnumerator<Token> tokens)
        {
            switch (tokens.Current.Type)
            {
                case TokenType.Increment:
                case TokenType.Decrement:
                    {
                        var t = tokens.Current;
                        tokens.MoveNext();
                        var a = ParseAtom(tokens);
                        CheckAssignment(a);
                        return LeftUnaryExpression.Create(t, a);
                    }

                case TokenType.Add:
                    tokens.MoveNext();
                    return ParseUnary(tokens);

                case TokenType.Subtract:
                case TokenType.Factorial:
                    {
                        var t = tokens.Current;
                        tokens.MoveNext();
                        var a = ParseUnary(tokens);
                        return LeftUnaryExpression.Create(t, a);
                    }

                default:
                    // not unary, return atom
                    return ParseAtom(tokens);
            }
        }

        /// <summary>
        /// Parses a list of arguments, starting at the ( bracket open token.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>A list expression with the contents.</returns>
        ListExpression ParseArguments(IEnumerator<Token> tokens)
        {
            var exps = new ListExpression(tokens.Current);

            do
            {
                tokens.MoveNext();

                if (tokens.Current.Type == TokenType.CloseBracket)
                    break;

                var exp = ParseExpression(tokens);

                if (exp != null)
                    exps.Add(exp);
            }
            while (tokens.Current.Type == TokenType.Comma);

            if (tokens.Current.Type != TokenType.CloseBracket)
                _errors.Add(new ParseError(ErrorCode.BracketMissing, tokens.Current.Row, tokens.Current.Column));
            else
                tokens.MoveNext();

            return exps;
        }

        /// <summary>
        /// Parses a bracket, starting at the ( bracket open token.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>A dynamically created expression or a lambda expression.</returns>
        Expression ParseBracket(IEnumerator<Token> tokens)
        {
            var o = tokens.Current;
            var content = ParseMatrix(tokens);

            if (tokens.Current.Type == TokenType.FatArrow)
            {
                if (content.Rows != 1)
                    _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, _row, _col));

                var args = content[0];
                CheckArguments(args);
                return ParseLambda(tokens, args);
            }
            else if (tokens.Current.Type == TokenType.OpenBracket)
                return new FunctionExpression(tokens.Current, content[0], ParseArguments(tokens));
            else if (content.Count == 1)
                return content[0, 0];

            return content;
        }

        /// <summary>
        /// Parses an array, starting at the [ matrix open token.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>A dynamically created array expression.</returns>
        Expression ParseArray(IEnumerator<Token> tokens)
        {
            var array = new ArrayExpression(tokens.Current);

            do
            {
                tokens.MoveNext();

                if (tokens.Current.Type == TokenType.CloseArray)
                    break;

                var exp = ParseExpression(tokens);

                if (exp != null)
                    array.Add(exp);
            }
            while (tokens.Current.Type == TokenType.Comma);

            if (tokens.Current.Type != TokenType.CloseArray)
                _errors.Add(new ParseError(ErrorCode.BracketMissing, tokens.Current.Row, tokens.Current.Column));
            else
            {
                tokens.MoveNext();

                if (tokens.Current.Type == TokenType.OpenBracket)
                    return new FunctionExpression(tokens.Current, array, ParseArguments(tokens));
            }

            return array;
        }

        /// <summary>
        /// Parses a matrix, starting at the &lt;&lt; matrix open token.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>A dynamically created matrix expression.</returns>
        MatrixExpression ParseMatrix(IEnumerator<Token> tokens)
        {
            var matrix = new MatrixExpression(tokens.Current);

            do
            {
                matrix.AddRow();

                do
                {
                    tokens.MoveNext();

                    if (tokens.Current.Type == TokenType.CloseBracket)
                        break;

                    var exp = ParseExpression(tokens);

                    if (exp != null)
                        matrix.AddExpression(exp);
                }
                while (tokens.Current.Type == TokenType.Comma);
            }
            while (tokens.Current.Type == TokenType.Semicolon);

            if (tokens.Current.Type != TokenType.CloseBracket)
                _errors.Add(new ParseError(ErrorCode.BracketMissing, tokens.Current.Row, tokens.Current.Column));
            else
                tokens.MoveNext();

            return matrix;
        }

        /// <summary>
        /// Parses a variable, starting at the identifier token.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>A dynamically created variable expression, lambda expression or function expression.</returns>
        Expression ParseVariable(IEnumerator<Token> tokens)
        {
            var name = tokens.Current;
            tokens.MoveNext();

            var x = new VariableExpression(name);

            if (tokens.Current.Type == TokenType.OpenBracket)
                return new FunctionExpression(tokens.Current, x, ParseArguments(tokens));
            
            if (tokens.Current.Type == TokenType.FatArrow)
            {
                var args = new ListExpression(name);
                args.Add(x);
                return ParseLambda(tokens, args);
            }

            return x;
        }

        /// <summary>
        /// Parses a lambda expression, starting at the fat arrow operator.
        /// </summary>
        /// <param name="tokens">The current token source.</param>
        /// <param name="args">The arguments of the lambda expression.</param>
        /// <returns>A dynamically created lambda expression.</returns>
        LambdaExpression ParseLambda(IEnumerator<Token> tokens, ListExpression args)
        {
            var t = tokens.Current;
            tokens.MoveNext();
            Statement statement = null;

            if (tokens.Current.Type == TokenType.OpenScope)
                statement = ParseStatement(tokens);
            else
            {
                var ret = new ReturnStatement(tokens.Current);
                ret.Payload = ParseStatement(tokens);
                statement = ret;
            }

            return new LambdaExpression(t, args, statement);
        }

        /// <summary>
        /// Parses an atomic expression, starting at the current position.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>A dynamically created expression (anything from
        /// bracket content, literals, variables, lambdas, functions, matrix, 
        /// dot operators, assignemnts, ...)</returns>
        Expression ParseAtom(IEnumerator<Token> tokens)
        {
            Expression x = null;

            switch (tokens.Current.Category)
            {
                // literals
                case TokenCategory.Literal:
                    x = new LiteralExpression(tokens.Current);
                    tokens.MoveNext();
                    break;

                // identifiers
                case TokenCategory.Identifier:
                    x = ParseVariable(tokens);
                    break;

                // sub-expressions
                case TokenCategory.Group:
                    switch (tokens.Current.Type)
                    {
                        case TokenType.OpenBracket:
                            x = ParseBracket(tokens);
                            break;

                        case TokenType.OpenArray:
                            x = ParseArray(tokens);
                            break;

                        case TokenType.Semicolon:
                            return null;

                        case TokenType.EOF:
                            _errors.Add(new ParseError(ErrorCode.OperandExpected, tokens.Current.Row, tokens.Current.Column));
                            break;

                        case TokenType.Comma:
                            _errors.Add(new ParseError(ErrorCode.CommaUnexpected, tokens.Current.Row, tokens.Current.Column));
                            tokens.MoveNext();
                            break;

                        default:
                            _errors.Add(new ParseError(ErrorCode.BracketUnexpected, tokens.Current.Row, tokens.Current.Column));
                            tokens.MoveNext();
                            break;
                    }
                    break;

                default:
                    _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, tokens.Current.Row, tokens.Current.Column));
                    tokens.MoveNext();
                    break;
            }

            if (x != null)
            {
                // dot access
                while (tokens.Current.Type == TokenType.Dot)
                {
                    var token = tokens.Current;
                    tokens.MoveNext();

                    if (tokens.Current.Category == TokenCategory.Identifier)
                    {
                        var y = ParseVariable(tokens);

                        if (y is LambdaExpression)
                            _errors.Add(new ParseError(ErrorCode.UnexpectedCharacter, token.Row, token.Column));

                        x = BinaryExpression.Create(token, x, y);
                    }
                    else
                        _errors.Add(new ParseError(ErrorCode.IdentifierExpected, tokens.Current.Row, tokens.Current.Column));
                }

                // right unary operators
                while (true)
                {
                    switch (tokens.Current.Type)
                    {
                        case TokenType.Increment:
                        case TokenType.Decrement:
                            {
                                var token = tokens.Current;
                                tokens.MoveNext();
                                CheckAssignment(x);
                                x = RightUnaryExpression.Create(token, x);
                                continue;
                            }
                        case TokenType.Factorial:
                        case TokenType.Transpose:
                        case TokenType.Adjungate:
                            {
                                var token = tokens.Current;
                                tokens.MoveNext();
                                x = RightUnaryExpression.Create(token, x);
                                continue;
                            }
                    }

                    break;
                }
            }

            return x;
        }

        #endregion

        #region Helpers

        void ParseInNewList(StatementList list, IEnumerator<Token> tokens)
        {
            var parent = _result;
            _result = list;

            if (tokens.Current.Type == TokenType.OpenScope)
            {
                var buffer = StatementListTokens(tokens).GetEnumerator();
                ParseStatements(buffer);
            }
            else
            {
                var stmt = ParseStatement(tokens);
                list.Add(stmt);
            }

            _result = parent;
        }

        IEnumerable<Token> StatementListTokens(IEnumerator<Token> tokens)
        {
            int nested = 1;

            while (nested > 0 && tokens.Current.Type != TokenType.EOF)
            {
                tokens.MoveNext();

                if (tokens.Current.Type == TokenType.OpenScope)
                    nested++;
                else if (tokens.Current.Type == TokenType.CloseScope)
                {
                    nested--;

                    if (nested == 0)
                    {
                        yield return Token.End(tokens.Current.Row, tokens.Current.Column);
                        break;
                    }
                }

                yield return tokens.Current;
            }
        }

        IEnumerable<Token> BracketTokens(IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
                yield return token;
        }

        void CheckAssignment(Expression leftSide)
        {
            if (leftSide is VariableExpression || leftSide is FunctionExpression)
                return;
            else if (leftSide is BinaryExpression && ((BinaryExpression)leftSide).Token.Type == TokenType.Dot && ((BinaryExpression)leftSide).Right is VariableExpression)
                return;

            _errors.Add(new ParseError(ErrorCode.IdentifierExpected, leftSide.Token.Row, leftSide.Token.Column));
        }

        void CheckArguments(ListExpression arguments)
        {
            foreach (var argument in arguments)
            {
                if (argument.Token.Category != TokenCategory.Identifier)
                    _errors.Add(new ParseError(ErrorCode.IdentifierExpected, argument.Token.Row, argument.Token.Column));
            }
        }

        #endregion
    }
}
