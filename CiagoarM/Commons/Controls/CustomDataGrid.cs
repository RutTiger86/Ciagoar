using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel.DataAnnotations.Schema;
using Ciagoar.Core.Attributes;

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

                foreach(Attribute AT in propertyDescriptor.Attributes)
                {
                    if(AT is CustomColumnAttributes CCA)
                    {
                        if (CCA.StarWidth != 0)
                        {
                            e.Column.Width = new DataGridLength(CCA.StarWidth, DataGridLengthUnitType.Star);
                        }

                        if (CCA.MinWidth != 0)
                        {
                            e.Column.MinWidth = CCA.MinWidth;
                        }
                    }

                }
                

                //e.Column.MinWidth = propertyDescriptor.
            }
            catch
            {

            }
        }
    }
}
