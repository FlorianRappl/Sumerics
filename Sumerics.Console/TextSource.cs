namespace FastColoredTextBoxNS
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;

    /// <summary>
    /// This class contains the source text (chars and styles).
    /// It stores a text lines, the manager of commands, undo/redo stack, styles.
    /// </summary>
    public class TextSource: IList<Line>, IDisposable
    {
        readonly protected List<Line> lines = new List<Line>();

        LinesAccessor _linesAccessor;
        Int32 _lastLineUniqueId;
        FastColoredTextBox _currentTB;

        internal CommandManager Manager { get; private set; }

        /// <summary>
        /// Styles
        /// Maximum style count is 16
        /// </summary>
        public readonly Style[] Styles = new Style[sizeof(ushort) * 8];

        /// <summary>
        /// Occurs when line was inserted/added
        /// </summary>
        public event EventHandler<LineInsertedEventArgs> LineInserted;

        /// <summary>
        /// Occurs when line was removed
        /// </summary>
        public event EventHandler<LineRemovedEventArgs> LineRemoved;

        /// <summary>
        /// Occurs when text was changed
        /// </summary>
        public event EventHandler<TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Occurs when recalc is needed
        /// </summary>
        public event EventHandler<TextChangedEventArgs> RecalcNeeded;

        /// <summary>
        /// Occurs before text changing
        /// </summary>
        public event EventHandler<TextChangingEventArgs> TextChanging;

        /// <summary>
        /// Occurs after CurrentTB was changed
        /// </summary>
        public event EventHandler CurrentTBChanged;

        /// <summary>
        /// Current focused FastColoredTextBox
        /// </summary>
        public FastColoredTextBox CurrentTB {
            get { return _currentTB; }
            set
            {
                _currentTB = value;
                OnCurrentTBChanged(); 
            }
        }

        public virtual void ClearIsChanged()
        {
            foreach (var line in lines)
            {
                line.IsChanged = false;
            }
        }

        public virtual Line CreateLine()
        {
            return new Line(GenerateUniqueLineId());
        }

        private void OnCurrentTBChanged()
        {
            if (CurrentTBChanged != null)
            {
                CurrentTBChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Default text style
        /// This style is using when no one other TextStyle is not defined in Char.style
        /// </summary>
        public TextStyle DefaultStyle { get; set; }

        public TextSource(FastColoredTextBox currentTB)
        {
            CurrentTB = currentTB;
            _linesAccessor = new LinesAccessor(this);
            Manager = new CommandManager(this);
            InitDefaultStyle();
        }

        public void InitDefaultStyle()
        {
            DefaultStyle = new TextStyle(null, null, FontStyle.Regular);
        }

        public virtual Line this[int i]
        {
            get
            {
                return lines[i];
            }
            set
            {
                //Nothing to do here.
            }
        }

        public virtual Boolean IsLineLoaded(Int32 iLine)
        {
            return lines[iLine] != null;
        }

        /// <summary>
        /// Text lines
        /// </summary>
        public IList<String> Lines
        {
            get { return _linesAccessor; }
        }

        public IEnumerator<Line> GetEnumerator()
        {
            return lines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return lines as IEnumerator;
        }

        public Int32 BinarySearch(Line item, IComparer<Line> comparer)
        {
            return lines.BinarySearch(item, comparer);
        }

        public Int32 GenerateUniqueLineId()
        {
            return _lastLineUniqueId++;
        }

        public virtual void InsertLine(Int32 index, Line line)
        {
            lines.Insert(index, line);
            OnLineInserted(index);
        }

        public void OnLineInserted(Int32 index)
        {
            OnLineInserted(index, 1);
        }

        public void OnLineInserted(Int32 index, Int32 count)
        {
            if (LineInserted != null)
            {
                LineInserted(this, new LineInsertedEventArgs(index, count));
            }
        }

        public virtual void RemoveLine(Int32 index)
        {
            RemoveLine(index, 1);
        }

        public Boolean IsNeedBuildRemovedLineIds
        {
            get { return LineRemoved != null; }
        }

        public virtual void RemoveLine(Int32 index, Int32 count)
        {
            var removedLineIds = new List<Int32>();

            if (count > 0 && IsNeedBuildRemovedLineIds)
            {
                for (var i = 0; i < count; i++)
                {
                    removedLineIds.Add(this[index + i].UniqueId);
                }
            }
            
            lines.RemoveRange(index, count);
            OnLineRemoved(index, count, removedLineIds);
        }

        public void OnLineRemoved(Int32 index, Int32 count, List<Int32> removedLineIds)
        {
            if (count > 0 && LineRemoved != null)
            {
                LineRemoved(this, new LineRemovedEventArgs(index, count, removedLineIds));
            }
        }

        public void OnTextChanged(Int32 fromLine, Int32 toLine)
        {
            if (TextChanged != null)
            {
                TextChanged(this, new TextChangedEventArgs(Math.Min(fromLine, toLine), Math.Max(fromLine, toLine)));
            }
        }

        public class TextChangedEventArgs : EventArgs
        {
            public Int32 iFromLine;
            public Int32 iToLine;

            public TextChangedEventArgs(Int32 iFromLine, Int32 iToLine)
            {
                this.iFromLine = iFromLine;
                this.iToLine = iToLine;
            }
        }

        public virtual Int32 IndexOf(Line item)
        {
            return lines.IndexOf(item);
        }

        public virtual void Insert(Int32 index, Line item)
        {
            InsertLine(index, item);
        }

        public virtual void RemoveAt(Int32 index)
        {
            RemoveLine(index);
        }

        public virtual void Add(Line item)
        {
            InsertLine(Count, item);
        }

        public virtual void Clear()
        {
            RemoveLine(0, Count);
        }

        public virtual Boolean Contains(Line item)
        {
            return lines.Contains(item);
        }

        public virtual void CopyTo(Line[] array, Int32 arrayIndex)
        {
            lines.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Lines count
        /// </summary>
        public virtual Int32 Count
        {
            get { return lines.Count; }
        }

        public virtual Boolean IsReadOnly
        {
            get { return false; }
        }

        public virtual Boolean Remove(Line item)
        {
            var i = IndexOf(item);

            if (i >= 0)
            {
                RemoveLine(i);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void NeedRecalc(TextChangedEventArgs args)
        {
            if (RecalcNeeded != null)
            {
                RecalcNeeded(this, args);
            }
        }

        internal void OnTextChanging()
        {
            var temp = default(String);
            OnTextChanging(ref temp);
        }

        internal void OnTextChanging(ref String text)
        {
            if (TextChanging != null)
            {
                var args = new TextChangingEventArgs() { InsertingText = text };
                TextChanging(this, args);
                text = args.InsertingText;

                if (args.Cancel)
                {
                    text = String.Empty;
                }
            };
        }

        public virtual Int32 GetLineLength(Int32 i)
        {
            return lines[i].Count;
        }

        public virtual Boolean LineHasFoldingStartMarker(Int32 iLine)
        {
            return !String.IsNullOrEmpty(lines[iLine].FoldingStartMarker);
        }

        public virtual Boolean LineHasFoldingEndMarker(Int32 iLine)
        {
            return !String.IsNullOrEmpty(lines[iLine].FoldingEndMarker);
        }

        public virtual void Dispose()
        {
        }

        public virtual void SaveToFile(String fileName, Encoding enc)
        {
            using (var sw = new StreamWriter(fileName, false, enc))
            {
                for (var i = 0; i < Count - 1; i++)
                {
                    sw.WriteLine(lines[i].Text);
                }

                sw.Write(lines[Count-1].Text);
            }
        }
    }
}
