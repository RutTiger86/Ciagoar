using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace CiagoarM.Commons.Controls
{
    class CustomDataGrid : DataGrid
    {
        protected override void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
        {
            try
            {
                base.OnAutoGeneratingColumn(e);
                var propertyDescriptor = e.PropertyDescriptor as PropertyDescriptor;
                e.Column.Header = propertyDescriptor.DisplayName;
                e.Column.IsReadOnly = propertyDescriptor.IsReadOnly;
                e.Column.Visibility = propertyDescriptor.IsBrowsable
                                      ? Visibility.Visible
                                      : Visibility.Collapsed;
            }
            catch
            {

            }
        }
    }
}
