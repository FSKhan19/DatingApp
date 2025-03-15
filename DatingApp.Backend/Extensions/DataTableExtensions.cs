using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DatingApp.Backend.Extensions
{

    /// <summary>
    /// Provides extension methods for the <see cref="DataTable"/> class.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Converts a <see cref="DataTable"/> to a CSV-formatted string.
        /// </summary>
        /// <param name="dtDataTable">The <see cref="DataTable"/> to convert.</param>
        /// <param name="isShowHeaders">A boolean indicating whether to include column headers in the CSV output.</param>
        /// <returns>A string containing the CSV representation of the <see cref="DataTable"/>.</returns>
        public static string DataTableToCSV(this DataTable dtDataTable, bool isShowHeaders)
        {
            if (dtDataTable == null)
            {
                throw new ArgumentNullException(nameof(dtDataTable), "DataTable cannot be null.");
            }

            StringBuilder sb = new StringBuilder();

            // Add headers if required
            if (isShowHeaders)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    sb.Append(EscapeCsvValue(dtDataTable.Columns[i].ColumnName));
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendLine();
            }

            // Add rows
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    object value = dr[i];
                    sb.Append(EscapeCsvValue(value?.ToString()));
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Escapes a value for inclusion in a CSV file.
        /// </summary>
        /// <param name="value">The value to escape.</param>
        /// <returns>The escaped value.</returns>
        private static string EscapeCsvValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                // Escape double quotes by doubling them
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }

            return value;
        }

        /// <summary>
        /// Converts a <see cref="DataTable"/> to a list of objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to create from the <see cref="DataTable"/>.</typeparam>
        /// <param name="table">The <see cref="DataTable"/> to convert.</param>
        /// <returns>A list of objects of type <typeparamref name="T"/>.</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : new()
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table), "DataTable cannot be null.");
            }

            List<T> list = new List<T>();

            // Get properties of T and their underlying types (for nullable types)
            var typeProperties = typeof(T).GetProperties()
                .Select(propertyInfo => new
                {
                    PropertyInfo = propertyInfo,
                    Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
                })
                .ToList();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T();

                foreach (var typeProperty in typeProperties)
                {
                    object value = row[typeProperty.PropertyInfo.Name];

                    // Handle DBNull or null values
                    object safeValue = value == null || DBNull.Value.Equals(value)
                        ? null
                        : Convert.ChangeType(value, typeProperty.Type);

                    typeProperty.PropertyInfo.SetValue(obj, safeValue);
                }

                list.Add(obj);
            }

            return list;
        }
    }
}
