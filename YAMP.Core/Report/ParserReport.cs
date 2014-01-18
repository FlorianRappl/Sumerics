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
using YAMP.Parser;

namespace YAMP.Report
{
    /// <summary>
    /// Summarizes the compilation process.
    /// The depth (level of details) of this report
    /// depends on the configured requirement.
    /// </summary>
    public sealed class ParserReport
    {
        #region Members

        readonly Frontend _cmp;

        #endregion

        #region ctor

        internal ParserReport(Frontend cmp)
        {
            _cmp = cmp;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the compilation resulted in errors.
        /// </summary>
        public Boolean HasErrors
        {
            get { return _cmp.HasErrors; }
        }

        /// <summary>
        /// Gets the number of errors in the compilation.
        /// </summary>
        public Int32 ErrorCount
        {
            get { return _cmp.ErrorCount; }
        }

        /// <summary>
        /// Gets an iteration over the compilation errors.
        /// </summary>
        public IEnumerable<ParserError> Errors
        {
            get
            {
                foreach (var error in _cmp.Errors)
                {
                    yield return new ParserError
                    {
                        Code = (Int32)error.Code,
                        Column = error.Column,
                        Message = error.ToString(),
                        Row = error.Row
                    };
                }
            }
        }

        /// <summary>
        /// Gets an iteration over the tokens of the query.
        /// </summary>
        public IEnumerable<LanguageToken> Tokens
        {
            get
            {
                foreach (var token in _cmp.Tokens)
                {
                    if (token.Type == TokenType.EOF)
                        yield break;

                    var str = token.ToCode();

                    yield return new LanguageToken
                    {
                        Category = GetCategory(token),
                        Column   = token.Column,
                        Row      = token.Row,
                        Content  = token.ValueAsString,
                        Length   = token.ValueAsString.Length
                    };
                }
            }
        }

        /// <summary>
        /// Gets the compilation time in ticks.
        /// </summary>
        public Int32 Time
        {
            get { return _cmp.Ticks; }
        }

        /// <summary>
        /// Gets the token count.
        /// </summary>
        public Int32 TokenCount
        {
            get { return _cmp.TokenCount; }
        }

        #endregion

        #region Helpers

        LanguageTokenCategory GetCategory(Token token)
        {
            if (token.Category == TokenCategory.Identifier)
                return LanguageTokenCategory.Identifier;
            else if (token.Category == TokenCategory.Operator)
                return LanguageTokenCategory.Operator;
            else if (token.Category == TokenCategory.DotOperator)
                return LanguageTokenCategory.Operator;
            else if (token.Category == TokenCategory.Assignment)
                return LanguageTokenCategory.Operator;

            switch (token.Type)
            {
                case TokenType.Integer:
                case TokenType.Real:
                case TokenType.Complex:
                case TokenType.Boolean:
                    return LanguageTokenCategory.Number;

                case TokenType.String:
                    return LanguageTokenCategory.String;

                case TokenType.Comment:
                    return LanguageTokenCategory.Comment;

                case TokenType.Do:
                case TokenType.Else:
                case TokenType.For:
                case TokenType.If:
                case TokenType.While:
                case TokenType.Function:
                case TokenType.Class:
                case TokenType.Let:
                case TokenType.Return:
                case TokenType.Break:
                    return LanguageTokenCategory.Keyword;

                default:
                    return LanguageTokenCategory.Other;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string with the most important information from the report.
        /// </summary>
        /// <returns>The string with the information.</returns>
        public override String ToString()
        {
            var errors = String.Empty;

            if (HasErrors)
            {
                var sb = new StringBuilder();

                foreach (var error in _cmp.Errors)
                    sb.AppendLine().Append("\tError: ").Append(error.ToString());

                errors = sb.ToString();
            }

            return String.Format("Compilation summary\n{{\n\tTime: {0} ms{2}\n\tTokens: {3}\n}}",
                Time, ErrorCount, errors, TokenCount);
        }

        #endregion
    }
}
