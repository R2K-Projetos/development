using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ghb.Psicossoma.Library.Extensions
{
    public static partial class DataExtensionMethods
    {

        #region Rotinas de Conversão

        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                PropertyInfo? p = item?.GetType().GetProperty(c.ColumnName);

                if (p is not null && row[c] != DBNull.Value)
                    p.SetValue(item, row[c], null);
            }
        }

        // function that creates an object from the given data row
        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new();
            SetItemFromRow(item, row);

            return item;
        }

        // function that creates a list of an object from the given data table
        public static List<T> CreateListFromTable<T>(this DataTable tbl) where T : new()
        {
            List<T> lst = new();

            foreach (DataRow r in tbl.Rows)
                lst.Add(CreateItemFromRow<T>(r));

            return lst;
        }

        #endregion
    }
}
