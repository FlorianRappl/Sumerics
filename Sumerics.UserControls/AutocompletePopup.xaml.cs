using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms.Integration;

namespace Sumerics.Controls
{
    /// <summary>
    /// Interaction logic for AutocompletePopup.xaml
    /// </summary>
    public partial class AutocompletePopup : Popup
    {
        #region Members

		ObservableCollection<AutocompleteItem> visibleItems;
        ObservableCollection<AutocompleteItem> availableItems;
        FastColoredTextBox tb;
        Timer timer;

        #endregion

        #region ctor

        AutocompletePopup(FastColoredTextBox textBox, WindowsFormsHost host)
        {
            timer = new Timer();
            timer.Tick += tick;
            InitializeComponent();
            PlacementTarget = host;
            CustomPopupPlacementCallback = placePopup;

            tb = textBox;
            tb.KeyDown += new KeyEventHandler(keyDown);
            tb.SelectionChanged += new EventHandler(selectionChanged);
            tb.KeyPressed += new KeyPressEventHandler(keyPressed);
            tb.LostFocus += tb_LostFocus;

            visibleItems = new ObservableCollection<AutocompleteItem>();
            AppearInterval = 500;
            SearchPattern = @"[\w\.]";
            MinFragmentLength = 1;
            timer.Start();

            var binding = new System.Windows.Data.Binding();
            binding.Source = visibleItems;
            Liste.SetBinding(System.Windows.Controls.ListBox.ItemsSourceProperty, binding);
        }

        public AutocompletePopup(ConsoleControl console) : this(console.Console, console.Host)
        {
            availableItems = console.AutoCompleteItems;
        }

        public AutocompletePopup(EditorControl editor) : this(editor.Editor, editor.Host)
        {
            availableItems = editor.Model.Items;
        }

        #endregion

        #region Properties

        public Range Fragment { get; internal set; }

        /// <summary>
        /// Regex pattern for serach fragment around caret
        /// </summary>
        public string SearchPattern { get; set; }

        /// <summary>
        /// Minimum fragment length for popup
        /// </summary>
        public int MinFragmentLength { get; set; }

        /// <summary>
        /// Interval of menu appear (ms)
        /// </summary>
        public int AppearInterval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }

        /// <summary>
        /// Gets the number of visible items.
        /// </summary>
        public int Count
        {
            get { return visibleItems.Count; }
        }

        /// <summary>
        /// Gets or sets the available items.
        /// </summary>
        public ObservableCollection<AutocompleteItem> AvailableItems
        {
            get { return availableItems; }
            set { availableItems = value; }
        }

        #endregion

        #region Methods

        void keyPressed(object sender, KeyPressEventArgs e)
        {
            if (IsOpen)
                DoAutocomplete(false);
            else
                ResetTimer(timer);
        }

        void selectionChanged(object sender, EventArgs e)
        {
            if (IsOpen)
            {
                var needClose = false;

                if (!tb.Selection.IsEmpty)
                    needClose = true;
                else if (!Fragment.Contains(tb.Selection.Start))
                {
                    if (tb.Selection.Start.iLine == Fragment.End.iLine && tb.Selection.Start.iChar == Fragment.End.iChar + 1)
                    {
                        //user press key at end of fragment
                        var c = tb.Selection.CharBeforeStart;

                        if (!Regex.IsMatch(c.ToString(), SearchPattern))
                            needClose = true;
                    }
                    else
                        needClose = true;
                }

                if (needClose)
                    Close();
            }
        }

        void keyDown(object sender, KeyEventArgs e)
        {
            if (IsOpen)
            {
                if (ProcessKey(e.KeyData, e.Modifiers))
                    e.Handled = true;
            }
            else if (e.Modifiers == Keys.Control && (e.KeyData & Keys.Space) == Keys.Space)
            {
                DoAutocomplete(true);
                e.Handled = true;
            }
        }

        void tick(object sender, EventArgs e)
        {
            timer.Stop();
            DoAutocomplete(false);
        }

        void ResetTimer(Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

        bool ProcessKey(Keys keyData, Keys keyModifiers)
        {
            if (!IsOpen)
                return false;

            if (keyModifiers == Keys.None)
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

        void DoAutocomplete(bool forced = false)
		{
            var selectedItem = Liste.SelectedItem;
            visibleItems.Clear();

            //get fragment around caret
            var fragment = tb.Selection.GetFragment(SearchPattern);
            var text = fragment.Text;

            //calc screen point for popup menu
            var point = tb.PlaceToPoint(fragment.End);
            point.Offset(2, tb.CharHeight);
            
            if (forced || (text.Length >= MinFragmentLength && tb.Selection.IsEmpty))
            {
                Fragment = fragment;

                //build popup menu
                var query = availableItems
                    .Where(m => m.Compare(text) != CompareResult.Hidden)
                    .OrderBy(m => m.Text);

                foreach (var item in query)
                    visibleItems.Add(item);

                if (visibleItems.Contains(selectedItem))
                    Liste.SelectedItem = selectedItem;
                else if (visibleItems.Count != 0 && (text.Length > 3 || visibleItems.Count == 1))
                    Liste.SelectedItem = visibleItems[0];
			}

            //show popup menu
			if (Count > 0)
				Show(point);
			else
				Close();
        }

        void DoAutocomplete(AutocompleteItem item, Range fragment)
        {
            var newText = item.GetTextForReplace();

            //replace text of fragment
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

        void tb_LostFocus(object sender, EventArgs e)
        {
            Close();
        }

        public void SelectNext(int shift)
        {
            if(visibleItems.Count == 0)
                return;

			Liste.SelectedIndex = (Liste.SelectedIndex + shift + visibleItems.Count) % visibleItems.Count;
            Liste.ScrollIntoView(Liste.SelectedItem);
        }

        /// <summary>
        /// Shows popup menu immediately
        /// </summary>
        /// <param name="forced">If True - MinFragmentLength will be ignored</param>
        public void Show(bool forced)
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

        bool OnSelectingWithEnter()
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
            if (Liste.SelectedItem == null)
                return;

            try
            {
                var item = (AutocompleteItem)Liste.SelectedItem;
                DoAutocomplete(item, Fragment);
                Close();
                
                var args2 = new SelectedEventArgs
                {
                    Item = item,
                    Tb = Fragment.tb
                };
                
                item.OnSelected(this, args2);
            }
            finally
            { }
		}

		void OnItemTouched(object sender, System.Windows.Input.TouchEventArgs e)
		{
            Liste.SelectionChanged += Liste_SelectionChanged;
		}

        void Liste_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Liste.SelectionChanged -= Liste_SelectionChanged;
            OnSelecting();
            PlacementTarget.Focus();
            tb.Focus();
        }

		void OnItemClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			OnSelecting();
            PlacementTarget.Focus();
			tb.Focus();
		}

        #endregion
    }
}
