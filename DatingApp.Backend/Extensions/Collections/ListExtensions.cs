using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace DatingApp.Backend.Extensions.Collections
{

    /// <summary>
    /// Provides extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Converts a list of objects of type <typeparamref name="T"/> to a <see cref="DataTable"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="iList">The list of objects to convert.</param>
        /// <returns>A <see cref="DataTable"/> containing the data from the list.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input list is null.</exception>
        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            if (iList == null)
            {
                throw new ArgumentNullException(nameof(iList), "Input list cannot be null.");
            }

            DataTable dataTable = new DataTable();

            // Get property descriptors for the type T
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));

            // Add columns to the DataTable based on the properties of T
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
            {
                Type type = propertyDescriptor.PropertyType;

                // Handle nullable types
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }

            // Add rows to the DataTable
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T item in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(item);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
