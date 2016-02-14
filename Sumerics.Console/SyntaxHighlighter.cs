namespace FastColoredTextBoxNS
{
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public class SyntaxHighlighter: IDisposable
    {
        //styles
        readonly static Style BlackStyle = new TextStyle(Brushes.Black, null, FontStyle.Regular);
        readonly static Style BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        readonly static Style BlueBoldStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        readonly static Style BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
        readonly static Style GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        readonly static Style MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
        readonly static Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        readonly static Style BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);
        readonly static Style RedStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);
        readonly static Style MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
        readonly static Style VioletStyle = new TextStyle(Brushes.DarkViolet, null, FontStyle.Regular);

        Regex YampStringRegex, 
              YampCommentRegex1, 
              YampCommentRegex2, 
              YampCommentRegex3,
              YampNumberRegex,
              YampIdentifierRegex, 
              YampOperatorRegex,
			  YampSpecialRegex,
              YampKeywordRegex;

        public static RegexOptions RegexCompiledOption
        {
            get { return RegexOptions.None; }
        }

        public Boolean AutoFoldBlocks
        {
            get;
            set;
        }

        public virtual void AutoIndentNeeded(Object sender, AutoIndentEventArgs args)
        {
            if(AutoFoldBlocks)
            {
			    if (Regex.IsMatch(args.PrevLineText, @"^\s*(if|for|function|while|[\}\s]*else)\b[^{]*$"))
			    {
                    //operator is unclosed
				    if (!Regex.IsMatch(args.PrevLineText, @"(;\s*$)|(;\s*//)"))
				    {
					    args.Shift = args.TabLength;
					    return;
				    }
			    }
            }
        }

        public SyntaxHighlighter()
        {
            InitYampRegex();
        }

        void InitYampRegex()
        {
            YampStringRegex = new Regex( @"""""|@""""|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")", RegexCompiledOption);
			YampIdentifierRegex = new Regex(@"\b[A-Za-z]+[A-Za-z0-9_]*\b", RegexCompiledOption);
            YampCommentRegex1 = new Regex(@"//.*$", RegexOptions.Multiline | RegexCompiledOption);
            YampCommentRegex2 = new Regex(@"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | RegexCompiledOption);
            YampCommentRegex3 = new Regex(@"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft | RegexCompiledOption);
            YampNumberRegex = new Regex(@"\b[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?i?\b", RegexCompiledOption);
			YampSpecialRegex = new Regex(@"\b((\$)|(@)|(:))\b", RegexCompiledOption);
			YampKeywordRegex = new Regex(@"\b(break|do|let|else|for|if|return|while|where|function)\b", RegexCompiledOption);
            YampOperatorRegex = new Regex(@"\b((\+|\-|\*|\/|\\|\.\*|\.\/|\.\\|\.\^|\^|\%|=|\+=|\-=|\*=|\^=|=\>|:)\b|(!))", RegexCompiledOption);
        }

        /// <summary>
        /// Highlights YAMP code
        /// </summary>
        /// <param name="range"></param>
        public virtual void HighlightSyntax(Range range)
        {
            range.tb.CommentPrefix = "//";
            range.tb.LeftBracket = '(';
            range.tb.RightBracket = ')';
            range.tb.LeftBracket2 = '[';
            range.tb.RightBracket2 = ']';

            //clear style of changed range
            range.ClearStyle(BlueStyle, BoldStyle, GrayStyle, MagentaStyle, GreenStyle, BrownStyle, BlackStyle, MaroonStyle, RedStyle, VioletStyle);

            //string highlighting
            range.SetStyle(BrownStyle, YampStringRegex);

            //comment highlighting
            range.SetStyle(GrayStyle, YampCommentRegex1);
            range.SetStyle(GrayStyle, YampCommentRegex2);
            range.SetStyle(GrayStyle, YampCommentRegex3);

			//number highlighting
			range.SetStyle(RedStyle, YampOperatorRegex);

			//operator highlighting
            range.SetStyle(BlueStyle, YampNumberRegex);

            //keyword highlighting
            range.SetStyle(VioletStyle, YampKeywordRegex);

            //identifier highlighting
            range.SetStyle(BlackStyle, YampIdentifierRegex);

			//special highlighting
			range.SetStyle(MaroonStyle, YampSpecialRegex);

            //clear folding markers
            range.ClearFoldingMarkers();


            //set folding markers
            if (AutoFoldBlocks)
            {
                //allow to collapse scope blocks
                range.SetFoldingMarkers("{", "}");
                //allow to collapse brackets block
                range.SetFoldingMarkers(@"\[", @"\]");
                //allow to collapse comment block
                range.SetFoldingMarkers(@"/\*", @"\*/");
                //allow to collapse #region blocks
                //range.SetFoldingMarkers(@"#region\b", @"#endregion\b");
            }
        }

        public void Dispose()
        {
        }
    }
}
