namespace Sumerics.Controls
{
    using FastColoredTextBoxNS;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Forms;
    using System.Windows.Forms.Integration;

    /// <summary>
    /// Interaction logic for AutocompletePopup.xaml
    /// </summary>
    public partial class AutocompletePopup : Popup
    {
        #region Fields

		readonly ObservableCollection<AutocompleteItem> _visibleItems;
        readonly ObservableCollection<AutocompleteItem> _availableItems;
        readonly FastColoredTextBox _tb;
        readonly Timer _timer;

        #endregion

        #region ctor

        AutocompletePopup(FastColoredTextBox textBox, WindowsFormsHost host)
        {
            _timer = new Timer();
            _timer.Tick += tick;
            InitializeComponent();
            PlacementTarget = host;
            CustomPopupPlacementCallback = placePopup;

            _tb = textBox;
            _tb.KeyDown += new KeyEventHandler(keyDown);
            _tb.SelectionChanged += new EventHandler(selectionChanged);
            _tb.KeyPressed += new KeyPressEventHandler(keyPressed);
            _tb.LostFocus += tb_LostFocus;

            _visibleItems = new ObservableCollection<AutocompleteItem>();
            AppearInterval = 500;
            SearchPattern = @"[\w\.]";
            MinFragmentLength = 1;
            _timer.Start();

            var binding = new System.Windows.Data.Binding();
            binding.Source = _visibleItems;
            Liste.SetBinding(System.Windows.Controls.ListBox.ItemsSourceProperty, binding);
        }

        public AutocompletePopup(ConsoleControl console) : this(console.Console, console.Host)
        {
            _availableItems = console.AutoCompleteItems;
        }

        public AutocompletePopup(EditorControl editor) : this(editor.Editor, editor.Host)
        {
            _availableItems = editor.AutoCompleteItems;
        }

        #endregion

        #region Properties

        public Range Fragment
        {
            get;
            internal set;
        }

        /// <summary>
        /// Regex pattern for serach fragment around caret
        /// </summary>
        public String SearchPattern
        {
            get;
            set;
        }

        /// <summary>
        /// Minimum fragment length for popup
        /// </summary>
        public Int32 MinFragmentLength
        {
            get;
            set;
        }

        /// <summary>
        /// Interval of menu appear (ms)
        /// </summary>
        public Int32 AppearInterval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        /// <summary>
        /// Gets the number of visible items.
        /// </summary>
        public Int32 Count
        {
            get { return _visibleItems.Count; }
        }

        /// <summary>
        /// Gets or sets the available items.
        /// </summary>
        public ObservableCollection<AutocompleteItem> AvailableItems
        {
            get { return _availableItems; }
        }

        #endregion

        #region Methods

        void keyPressed(Object sender, KeyPressEventArgs e)
        {
            if (IsOpen)
            {
                DoAutocomplete(false);
            }
            else
            {
                ResetTimer(_timer);
            }
        }

        void selectionChanged(Object sender, EventArgs e)
        {
            if (IsOpen)
            {
                var needClose = false;

                if (!_tb.Selection.IsEmpty)
                {
                    needClose = true;
                }
                else if (!Fragment.Contains(_tb.Selection.Start))
                {
                    if (_tb.Selection.Start.iLine == Fragment.End.iLine && _tb.Selection.Start.iChar == Fragment.End.iChar + 1)
                    {
                        //user press key at end of fragment
                        var c = _tb.Selection.CharBeforeStart;

                        if (!Regex.IsMatch(c.ToString(), SearchPattern))
                        {
                            needClose = true;
                        }
                    }
                    else
                    {
                        needClose = true;
                    }
                }

                if (needClose)
                {
                    Close();
                }
            }
        }

        void keyDown(Object sender, KeyEventArgs e)
        {
            if (IsOpen)
            {
                if (ProcessKey(e.KeyData, e.Modifiers))
                {
                    e.Handled = true;
                }
            }
            else if (e.Modifiers == Keys.Control && (e.KeyData & Keys.Space) == Keys.Space)
            {
                DoAutocomplete(true);
                e.Handled = true;
            }
        }

        void tick(Object sender, EventArgs e)
        {
            _timer.Stop();
            DoAutocomplete(false);
        }

        void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

        Boolean ProcessKey(Keys keyData, Keys keyModifiers)
        {
            if (IsOpen && keyModifiers == Keys.None)
            {
                switch (keyData)
                {
                    case Keys.Down:
                        SelectNext(+1);
                        return true;

                    case Keys.PageDown:
                        SelectNext(+10);
                        return true;

                    case Keys.Up:
                        SelectNext(-1);
                        return true;

                    case Keys.PageUp:
                        SelectNext(-10);
                        return true;

					case Keys.Tab:
						OnSelecting();
						return true;

                    case Keys.Enter:
						return OnSelectingWithEnter();

                    case Keys.Escape:
                        Close();
                        return true;
                }
            }

            return false;
        }

        void DoAutocomplete(Boolean forced = false)
		{
            var selectedItem = Liste.SelectedItem;
            _visibleItems.Clear();

            //get fragment around caret
            var fragment = _tb.Selection.GetFragment(SearchPattern);
            var text = fragment.Text;

            //calc screen point for popup menu
            var point = _tb.PlaceToPoint(fragment.End);
            point.Offset(2, _tb.CharHeight);
            
            if (forced || (text.Length >= MinFragmentLength && _tb.Selection.IsEmpty))
            {
                Fragment = fragment;

                //build popup menu
                var query = _availableItems
                    .Where(m => m.Compare(text) != CompareResult.Hidden)
                    .OrderBy(m => m.Text);

                foreach (var item in query)
                {
                    _visibleItems.Add(item);
                }

                if (_visibleItems.Contains(selectedItem))
                {
                    Liste.SelectedItem = selectedItem;
                }
                else if (_visibleItems.Count != 0 && (text.Length > 3 || _visibleItems.Count == 1))
                {
                    Liste.SelectedItem = _visibleItems[0];
                }
			}

            //show popup menu
            if (Count > 0)
            {
                Show(point);
            }
            else
            {
                Close();
            }
        }

        void DoAutocomplete(AutocompleteItem item, Range fragment)
        {
            var newText = item.GetTextForReplace();
            var tb = fragment.tb;

            if (tb.Selection.ColumnSelectionMode)
            {
                var start = tb.Selection.Start;
                var end = tb.Selection.End;
                start.iChar = fragment.Start.iChar;
                end.iChar = fragment.End.iChar;
                tb.Selection.Start = start;
                tb.Selection.End = end;
            }
            else
            {
                tb.Selection.Start = fragment.Start;
                tb.Selection.End = fragment.End;
            }

            tb.InsertText(newText);
            tb.Focus();
        }

        public void Close()
        {
            IsOpen = false;
        }

        void tb_LostFocus(Object sender, EventArgs e)
        {
            Close();
        }

        public void SelectNext(Int32 shift)
        {
            if (_visibleItems.Count != 0)
            {
                Liste.SelectedIndex = (Liste.SelectedIndex + shift + _visibleItems.Count) % _visibleItems.Count;
                Liste.ScrollIntoView(Liste.SelectedItem);
            }
        }

        /// <summary>
        /// Shows popup menu immediately
        /// </summary>
        /// <param name="forced">If True - MinFragmentLength will be ignored</param>
        public void Show(Boolean forced)
        {
			DoAutocomplete(forced);
        }

		void Show(System.Drawing.Point p)
        {
			IsOpen = true;
			VerticalOffset = p.Y;
			HorizontalOffset = p.X;
		}

        public CustomPopupPlacement[] placePopup(Size popupSize, Size targetSize, Point offset)
        {
            var placement = new CustomPopupPlacement(new Point(), PopupPrimaryAxis.None);
            return new CustomPopupPlacement[] { placement };
        }

        Boolean OnSelectingWithEnter()
		{
			if (Liste.SelectedItem == null)
			{
				Close();
				return false;
			}
			else
			{
				OnSelecting();
				return true;
			}
		}

        public virtual void OnSelecting()
        {
            if (Liste.SelectedItem != null)
            {
                var item = (AutocompleteItem)Liste.SelectedItem;
                DoAutocomplete(item, Fragment);
                Close();

                var args = new SelectedEventArgs(item, Fragment.tb);
                item.OnSelected(this, args);
            }
		}

        void OnItemTouched(Object sender, System.Windows.Input.TouchEventArgs e)
		{
            Liste.SelectionChanged += Liste_SelectionChanged;
		}

        void Liste_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            Liste.SelectionChanged -= Liste_SelectionChanged;
            OnSelecting();
            PlacementTarget.Focus();
            _tb.Focus();
        }

        void OnItemClicked(Object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			OnSelecting();
            PlacementTarget.Focus();
			_tb.Focus();
		}

        #endregion
    }
}
