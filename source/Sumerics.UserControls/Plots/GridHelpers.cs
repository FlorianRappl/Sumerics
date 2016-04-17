namespace Sumerics.Controls.Plots
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public static class GridHelpers
    {
        public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached(
            "Rows", typeof(Int32), typeof(GridHelpers), new PropertyMetadata(0, RowsChanged));

        public static Int32 GetRows(DependencyObject obj)
        {
            return (Int32)obj.GetValue(RowsProperty);
        }

        public static void SetRows(DependencyObject obj, Int32 value)
        {
            obj.SetValue(RowsProperty, value);
        }

        public static void RowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var rows = (Int32)e.NewValue;
            var grid = obj as Grid;

            if (grid != null && rows >= 0)
            {
                grid.RowDefinitions.Clear();

                for (var i = 0; i < rows; i++)
                {
                    var height = new GridLength(1, GridUnitType.Star);
                    var definition = new RowDefinition { Height = height };
                    grid.RowDefinitions.Add(definition);
                }
            }
        }

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached(
            "Columns", typeof(Int32), typeof(GridHelpers), new PropertyMetadata(0, ColumnsChanged));

        public static Int32 GetColumns(DependencyObject obj)
        {
            return (Int32)obj.GetValue(ColumnsProperty);
        }

        public static void SetColumns(DependencyObject obj, Int32 value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        public static void ColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var columns = (Int32)e.NewValue;
            var grid = obj as Grid;

            if (grid != null && columns >= 0)
            {
                grid.ColumnDefinitions.Clear();

                for (var i = 0; i < columns; i++)
                {
                    var width = new GridLength(1, GridUnitType.Star);
                    var definition = new ColumnDefinition { Width = width };
                    grid.ColumnDefinitions.Add(definition);
                }
            }
        }
    }
}
