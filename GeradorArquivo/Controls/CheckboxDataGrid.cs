using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeradorArquivo.Controls
{
    public class CheckboxDataGrid : DataGrid
    {
        public CheckboxDataGrid()
        {
            EventManager.RegisterClassHandler(typeof(DataGridCell),
                DataGridCell.PreviewMouseLeftButtonDownEvent,
                new RoutedEventHandler(this.OnPreviewMouseLeftButtonDown));
        }


        private void OnPreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {

                var parentRow = cell.Parent as DataGridRow;
                if (parentRow != null)
                {
                    SelectedIndex = parentRow.GetIndex();
                }
                CurrentCell = new DataGridCellInfo(cell);
                BeginEdit(e);
                DependencyObject obj = FindVisualChild<CheckBox>(cell);
                if (obj != null)
                {
                    System.Windows.Controls.CheckBox cb = (System.Windows.Controls.CheckBox)obj;
                    cb.Focus();
                    cb.IsChecked = !cb.IsChecked;
                }
            }
        }
        public static TChild FindVisualChild<TChild>(DependencyObject obj) where TChild : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChild)
                {
                    return (TChild)child;
                }
                else
                {
                    TChild childOfChild = FindVisualChild<TChild>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
