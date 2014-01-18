using System;

namespace YAMP.Parser
{
    /// <summary>
    /// A set of special characters.
    /// </summary>
    static class Chars
    {
        /// <summary>
        /// The end of file character 26.
        /// </summary>
        public const char EOF = (char)0x1a;

        /// <summary>
        /// The tilde character (~).
        /// </summary>
        public const char Tilde = (char)0x7e;

        /// <summary>
        /// The pipe character (|).
        /// </summary>
        public const char Pipe = (char)0x7c;

        /// <summary>
        /// The null character.
        /// </summary>
        public const char NullPtr = (char)0x0;

        /// <summary>
        /// The ampersand character (&).
        /// </summary>
        public const char Ampersand = (char)0x26;

        /// <summary>
        /// The number sign character (#).
        /// </summary>
        public const char Hash = (char)0x23;

        /// <summary>
        /// The dollar sign character ($).
        /// </summary>
        public const char Dollar = (char)0x24;

        /// <summary>
        /// The semicolon sign (;).
        /// </summary>
        public const char SemiColon = (char)0x3b;

        /// <summary>
        /// The asterisk character (*).
        /// </summary>
        public const char Asterisk = (char)0x2a;

        /// <summary>
        /// The equals sign (=).
        /// </summary>
        public const char Equal = (char)0x3d;

        /// <summary>
        /// The comma character (,).
        /// </summary>
        public const char Comma = (char)0x2c;

        /// <summary>
        /// The full stop (.).
        /// </summary>
        public const char FullStop = (char)0x2e;

        /// <summary>
        /// The circumflex accent (^) character.
        /// </summary>
        public const char CircumflexAccent = (char)0x5e;

        /// <summary>
        /// The commercial at (@) character.
        /// </summary>
        public const char At = (char)0x40;

        /// <summary>
        /// The opening angle bracket (LESS-THAN-SIGN).
        /// </summary>
        public const char LessThan = (char)0x3c;

        /// <summary>
        /// The closing angle bracket (GREATER-THAN-SIGN).
        /// </summary>
        public const char GreaterThan = (char)0x3e;

        /// <summary>
        /// The single quote / quotation mark (').
        /// </summary>
        public const char SingleQuotationMark = (char)0x27;

        /// <summary>
        /// The (double) quotation mark (").
        /// </summary>
        public const char DoubleQuotationMark = (char)0x22;

        /// <summary>
        /// The (curved) quotation mark (`).
        /// </summary>
        public const char CurvedQuotationMark = (char)0x60;

        /// <summary>
        /// The question mark (?).
        /// </summary>
        public const char QuestionMark = (char)0x3f;

        /// <summary>
        /// The tab character.
        /// </summary>
        public const char Tab = (char)0x09;

        /// <summary>
        /// The line feed character.
        /// </summary>
        public const char LineFeed = (char)0x0a;

        /// <summary>
        /// The carriage return character.
        /// </summary>
        public const char CarriageReturn = (char)0x0d;

        /// <summary>
        /// The form feed character.
        /// </summary>
        public const char FormFeed = (char)0x0c;

        /// <summary>
        /// The space character.
        /// </summary>
        public const char Space = (char)0x20;

        /// <summary>
        /// The slash (solidus, /) character.
        /// </summary>
        public const char Slash = (char)0x2f;

        /// <summary>
        /// The backslash (reverse-solidus, \) character.
        /// </summary>
        public const char Backslash = (char)0x5c;

        /// <summary>
        /// The colon (:) character.
        /// </summary>
        public const char Colon = (char)0x3a;

        /// <summary>
        /// The exlamation mark (!) character.
        /// </summary>
        public const char ExclamationMark = (char)0x21;

        /// <summary>
        /// The dash (hypen minus, -) character.
        /// </summary>
        public const char Minus = (char)0x2d;

        /// <summary>
        /// The plus sign (+).
        /// </summary>
        public const char Plus = (char)0x2b;

        /// <summary>
        /// The low line (_) character.
        /// </summary>
        public const char Lowline = (char)0x5f;

        /// <summary>
        /// The percent (%) character.
        /// </summary>
        public const char Percent = (char)0x25;

        /// <summary>
        /// Opening a round bracket (.
        /// </summary>
        public const char OpenBracket = (char)0x28;

        /// <summary>
        /// Closing a round bracket ).
        /// </summary>
        public const char CloseBracket = (char)0x29;

        /// <summary>
        /// Opening an array bracket [.
        /// </summary>
        public const char OpenArray = (char)0x5b;

        /// <summary>
        /// Closing an array bracket ].
        /// </summary>
        public const char CloseArray = (char)0x5d;

        /// <summary>
        /// Opening a scope bracket {.
        /// </summary>
        public const char OpenScope = (char)0x7b;

        /// <summary>
        /// Closing a scope bracket }.
        /// </summary>
        public const char CloseScope = (char)0x7d;

        /// <summary>
        /// Checks if the given name is a valid identifier.
        /// </summary>
        /// <param name="name">The given proposed identifier.</param>
        /// <returns>True if the name is truly an identifier, otherwise false.</returns>
        public static bool IsIdentifier(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            if (!char.IsLetter(name[0]))
                return false;

            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsWhiteSpace(name[i]))
                    return false;

                if (!char.IsLetterOrDigit(name[i]))
                    return false;
            }

            return true;
        }
    }
}
