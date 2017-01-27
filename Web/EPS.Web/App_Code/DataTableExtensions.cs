using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Text;

/// <summary>
/// Summary description for DataTableExtensions
/// </summary>
public static class DataTableExtensions
{
    public static IList<T> ToList<T>(this DataTable table) where T : new()
    {
        IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
        IList<T> result = new List<T>();

        foreach (var row in table.Rows)
        {
            var item = CreateItemFromRow<T>((DataRow)row, properties);
            result.Add(item);
        }

        return result;
    }

    private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
    {
        T item = new T();
        foreach (var property in properties)
        {
            property.SetValue(item, row[property.Name], null);
        }
        return item;
    }
    public static string ToXml(this DataSet ds)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (TextWriter streamWriter = new StreamWriter(memoryStream))
            {
                var xmlSerializer = new XmlSerializer(typeof(DataSet));
                xmlSerializer.Serialize(streamWriter, ds);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
