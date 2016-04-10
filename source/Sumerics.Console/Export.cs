namespace FastColoredTextBoxNS
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Exports colored text as HTML
    /// </summary>
    /// <remarks>At this time only TextStyle renderer is supported. Other styles is not exported.</remarks>
    public class ExportToHTML
    {
        public String LineNumbersCSS = "<style type=\"text/css\"> .lineNumber{font-family : monospace; font-size : small; font-style : normal; font-weight : normal; color : Teal; background-color : ThreedFace;} </style>";

        /// <summary>
        /// Use nbsp; instead space
        /// </summary>
        public Boolean UseNbsp { get; set; }

        /// <summary>
        /// Use nbsp; instead space in beginning of line
        /// </summary>
        public Boolean UseForwardNbsp { get; set; }

        /// <summary>
        /// Use original font
        /// </summary>
        public Boolean UseOriginalFont { get; set; }

        /// <summary>
        /// Use style tag instead style attribute
        /// </summary>
        public Boolean UseStyleTag { get; set; }

        /// <summary>
        /// Use br tag instead \n
        /// </summary>
        public Boolean UseBr { get; set; }

        /// <summary>
        /// Includes line numbers
        /// </summary>
        public Boolean IncludeLineNumbers { get; set; }

        FastColoredTextBox _tb;

        public ExportToHTML()
        {
            UseNbsp = true;
            UseOriginalFont = true;
            UseStyleTag = true;
            UseBr = true;
        }

        public String GetHtml(FastColoredTextBox tb)
        {
            _tb = tb;
            var sel = new Range(tb);
            sel.SelectAll();
            return GetHtml(sel);
        }
        
        public String GetHtml(Range r)
        {
            _tb = r.tb;
            var styles = new Dictionary<StyleIndex, Object>();
            var sb = new StringBuilder();
            var tempSB = new StringBuilder();
            var currentStyleId = StyleIndex.None;
            r.Normalize();
            var currentLine = r.Start.iLine;
            styles[currentStyleId] = null;

            if (UseOriginalFont)
            {
                sb.AppendFormat("<font style=\"font-family: {0}, monospace; font-size: {1}px; line-height: {2}px;\">",
                                r.tb.Font.Name, r.tb.CharHeight - r.tb.LineInterval, r.tb.CharHeight);
            }

            if (IncludeLineNumbers)
            {
                tempSB.AppendFormat("<span class=lineNumber>{0}</span>  ", currentLine + 1);
            }

            var hasNonSpace = false;

            foreach (var p in r)
            {
                var c = r.tb[p.iLine][p.iChar];

                if (c.style != currentStyleId)
                {
                    Flush(sb, tempSB, currentStyleId);
                    currentStyleId = c.style;
                    styles[currentStyleId] = null;
                }

                if (p.iLine != currentLine)
                {
                    for (var i = currentLine; i < p.iLine; i++)
                    {
                        tempSB.AppendLine(UseBr ? "<br>" : "");

                        if (IncludeLineNumbers)
                        {
                            tempSB.AppendFormat("<span class=lineNumber>{0}</span>  ", i + 2);
                        }
                    }

                    currentLine = p.iLine;
                    hasNonSpace = false;
                }

                switch (c.c)
                {
                    case ' ':
                        if ((hasNonSpace || !UseForwardNbsp) && !UseNbsp)
                            goto default;

                        tempSB.Append("&nbsp;");
                        break;
                    case '<':
                        tempSB.Append("&lt;");
                        break;
                    case '>':
                        tempSB.Append("&gt;");
                        break;
                    case '&':
                        tempSB.Append("&amp;");
                        break;
                    default:
                        hasNonSpace = true;
                        tempSB.Append(c.c);
                        break;
                }
            }

            Flush(sb, tempSB, currentStyleId);

            if (UseOriginalFont)
            {
                sb.AppendLine("</font>");
            }

            //build styles
            if (UseStyleTag)
            {
                tempSB.Length = 0;
                tempSB.AppendLine("<style type=\"text/css\">");

                foreach (var styleId in styles.Keys)
                {
                    tempSB.AppendFormat(".fctb{0}{{ {1} }}\r\n", GetStyleName(styleId), GetCss(styleId));
                }

                tempSB.AppendLine("</style>");
                sb.Insert(0, tempSB.ToString());
            }

            if (IncludeLineNumbers)
            {
                sb.Insert(0, LineNumbersCSS);
            }

            return sb.ToString();
        }

        String GetCss(StyleIndex styleIndex)
        {
            var styles = new List<Style>();
            //find text renderer
            var textStyle = default(TextStyle);
            var mask = 1;
            var hasTextStyle = false;

            for (var i = 0; i < _tb.Styles.Length; i++)
            {
                if (_tb.Styles[i] != null && ((int)styleIndex & mask) != 0 && _tb.Styles[i].IsExportable)
                {
                    var style = _tb.Styles[i];
                    styles.Add(style);

                    var isTextStyle = style is TextStyle;

                    if (isTextStyle && !hasTextStyle || _tb.AllowSeveralTextStyleDrawing)
                    {
                        hasTextStyle = true;
                        textStyle = style as TextStyle;
                    }
                }
                
                mask = mask << 1;
            }

            //add TextStyle css
            var result = "";
            
            if (!hasTextStyle)
            {
                //draw by default renderer
                result = _tb.DefaultStyle.GetCSS();
            }
            else
            {
                result = textStyle.GetCSS();
            }

            //add non TextStyle css
            foreach (var style in styles)
            {
                if (style != textStyle)
                {
                    result += style.GetCSS();
                }
            }

            return result;
        }

        public static String GetColorAsString(Color color)
        {
            if (color == Color.Transparent)
            {
                return "";
            }

            return String.Format("#{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
        }

        String GetStyleName(StyleIndex styleIndex)
        {
            return styleIndex.ToString().Replace(" ", "").Replace(",", "");
        }

        void Flush(StringBuilder sb, StringBuilder tempSB, StyleIndex currentStyle)
        {
            //find textRenderer
            //var textStyle = styles.Where(s => s is TextStyle).FirstOrDefault();
            
            if (tempSB.Length != 0)
            {
                if (UseStyleTag)
                {
                    sb.AppendFormat("<font class=fctb{0}>{1}</font>", GetStyleName(currentStyle), tempSB.ToString());
                }
                else
                {
                    var css = GetCss(currentStyle);

                    if (css != "")
                    {
                        sb.AppendFormat("<font style=\"{0}\">", css);
                    }

                    sb.Append(tempSB.ToString());

                    if (css != "")
                    {
                        sb.Append("</font>");
                    }
                }

                tempSB.Length = 0;
            }
        }
    }
}
