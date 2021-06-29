using System;
using System.Windows;
using System.Windows.Controls;

namespace Paraject.Core.Utilities
{
    public static class AutoRowsGrid
    {
        /// <summary> Returns the value of the ChildrenCount attached property for <paramref name = "grid" />. </summary>
        /// <param name = "grid"> <see cref = "Grid" /> whose property value will be returned. </param>
        /// <returns> <see cref = "int" /> property value. </returns>
        public static int? GetChildrenCount(Grid grid)
        {
            return (int?)grid.GetValue(ChildrenCountProperty);
        }

        /// <summary> Sets the ChildrenCount attached property for <paramref name = "grid" />. </summary>
        /// <param name = "grid"> <see cref = "Grid" /> whose property value will be returned. </param>
        /// <param name = "value"> <see cref = "int" /> value for the property. </param>
        public static void SetChildrenCount(Grid grid, int value)
        {
            grid.SetValue(ChildrenCountProperty, value);
        }

        /// <summary><see cref="DependencyProperty"/> for methods <see cref="GetChildrenCount(Grid)"/> и <see cref="SetChildrenCount(Grid, int)"/>.</summary>
        public static readonly DependencyProperty ChildrenCountProperty =
            DependencyProperty.RegisterAttached(nameof(GetChildrenCount).Substring(3), typeof(int?), typeof(AutoRowsGrid), new PropertyMetadata(null, CountChanged));

        private static void CountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Grid grid = (Grid)d;
            double count = (int)e.NewValue;

            int columns = grid.ColumnDefinitions.Count;
            if (columns < 1)
                columns = 1;

            int newRows = (int)Math.Ceiling(count / columns);

            int rows = grid.RowDefinitions.Count;

            if (newRows != rows)
            {
                if (newRows > rows)
                {
                    for (; newRows > rows; rows++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition());
                    }
                }
                else
                {
                    for (rows--; newRows <= rows; rows--)
                    {
                        grid.RowDefinitions.RemoveAt(rows);
                    }
                }
            }
        }
    }
}
