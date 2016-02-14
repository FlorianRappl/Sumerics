namespace FastColoredTextBoxNS
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// Line of text
    /// </summary>
    public class Line : IList<Char>
    {
        protected List<Char> chars;

        public String FoldingStartMarker { get; set; }
        public String FoldingEndMarker { get; set; }

        /// <summary>
        /// Text of line was changed
        /// </summary>
        public Boolean IsChanged { get; set; }

        /// <summary>
        /// Time of last visit of caret in this line
        /// </summary>
        /// <remarks>This property can be used for forward/backward navigating</remarks>
        public DateTime LastVisit { get; set; }

        /// <summary>
        /// Background brush.
        /// </summary>
        public Brush BackgroundBrush { get; set;}

        /// <summary>
        /// Unique ID
        /// </summary>
        public Int32 UniqueId { get; private set; }

        /// <summary>
        /// Count of needed start spaces for AutoIndent
        /// </summary>
        public Int32 AutoIndentSpacesNeededCount { get; internal set; }

        internal Line(Int32 uid)
        {
            UniqueId = uid;
            chars = new List<Char>();
        }

        /// <summary>
        /// Clears style of chars, delete folding markers
        /// </summary>
        public void ClearStyle(StyleIndex styleIndex)
        {
            FoldingStartMarker = null;
            FoldingEndMarker = null;

            for (var i = 0; i < Count; i++)
            {
                var c = this[i];
                c.style &= ~styleIndex;
                this[i] = c;
            }
        }

        /// <summary>
        /// Text of the line
        /// </summary>
        public virtual String Text
        {
            get
            {
                var sb = new StringBuilder(Count);

                foreach (var c in this)
                {
                    sb.Append(c.c);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Clears folding markers
        /// </summary>
        public void ClearFoldingMarkers()
        {
            FoldingStartMarker = null;
            FoldingEndMarker = null;
        }

        /// <summary>
        /// Count of start spaces
        /// </summary>
        public Int32 StartSpacesCount
        {
            get
            {
                var spacesCount = 0;

                for (var i = 0; i < Count; i++)
                {
                    if (this[i].c == ' ')
                    {
                        spacesCount++;
                    }
                    else
                    {
                        break;
                    }
                }

                return spacesCount;
            }
        }

        public Int32 IndexOf(Char item)
        {
            return chars.IndexOf(item);
        }

        public void Insert(Int32 index, Char item)
        {
            chars.Insert(index, item);
        }

        public void RemoveAt(Int32 index)
        {
            chars.RemoveAt(index);
        }

        public Char this[Int32 index]
        {
            get { return chars[index]; }
            set { chars[index] = value; }
        }

        public void Add(Char item)
        {
            chars.Add(item);
        }

        public void Clear()
        {
            chars.Clear();
        }

        public Boolean Contains(Char item)
        {
            return chars.Contains(item);
        }

        public void CopyTo(Char[] array, Int32 arrayIndex)
        {
            chars.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Chars count
        /// </summary>
        public Int32 Count
        {
            get { return chars.Count; }
        }

        public Boolean IsReadOnly
        {
            get {  return false; }
        }

        public Boolean Remove(Char item)
        {
            return chars.Remove(item);
        }

        public IEnumerator<Char> GetEnumerator()
        {
            return chars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return chars.GetEnumerator();
        }

        public virtual void RemoveRange(Int32 index, Int32 count)
        {
            if (index < Count)
            {
                chars.RemoveRange(index, Math.Min(Count - index, count));
            }
        }

        public virtual void TrimExcess()
        {
            chars.TrimExcess();
        }

        public virtual void AddRange(IEnumerable<Char> collection)
        {
            chars.AddRange(collection);
        }
    }
}
