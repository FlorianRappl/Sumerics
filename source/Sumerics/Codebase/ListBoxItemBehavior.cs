namespace Sumerics
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public static class ListBoxItemBehavior
    {
        public static Boolean GetIsBroughtIntoViewWhenSelected(ListBoxItem listBoxItem)
        {
            return (Boolean)listBoxItem.GetValue(IsBroughtIntoViewWhenSelectedProperty);
        }

        public static void SetIsBroughtIntoViewWhenSelected(ListBoxItem listBoxItem, Boolean value)
        {
            listBoxItem.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
        }

        public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached(
                "IsBroughtIntoViewWhenSelected",
                typeof(Boolean),
                typeof(ListBoxItemBehavior),
                new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));

        static void OnIsBroughtIntoViewWhenSelectedChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var item = depObj as ListBoxItem;

            if (item != null && e.NewValue is Boolean)
            {
                if ((Boolean)e.NewValue)
                {
                    item.Selected += OnListBoxItemSelected;
                }
                else
                {
                    item.Selected -= OnListBoxItemSelected;
                }
            }
        }

        static void OnListBoxItemSelected(Object sender, RoutedEventArgs e)
        {
            // Only react to the Selected event raised by the ListBoxItem 
            // whose IsSelected property was modified.  Ignore all ancestors 
            // who are merely reporting that a descendant's Selected fired. 
            if (Object.ReferenceEquals(sender, e.OriginalSource))
            {
                var item = e.OriginalSource as ListBoxItem;

                if (item != null)
                {
                    item.BringIntoView();
                }
            }
        }
    }
}
