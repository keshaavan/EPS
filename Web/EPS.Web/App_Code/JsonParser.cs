using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Data;
using System.Runtime.Serialization.Json;

/// <summary>
/// Summary description for JsonParser
/// </summary>
public class JsonParser
{
    public JsonParser()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region From Object To JSON String.
    /// <summary>
    /// Static function to convert given DataTable object as json string.
    /// </summary>
    /// <param name="dTable"></param>
    /// <returns></returns>
    public static string getJSONString(System.Data.DataTable dTable)
    {
        StringBuilder responseJSON = new StringBuilder();
        int rowCount = 0;

        responseJSON.Append("[");

        foreach (System.Data.DataRow r in dTable.Rows)
        {
            rowCount++;
            responseJSON.Append("{");
            int columnCount = 0;
            foreach (System.Data.DataColumn c in dTable.Columns)
            {
                //Return With N/A if the column has DBNULL
                if (r[columnCount].Equals(DBNull.Value) || r[columnCount].ToString().Equals(string.Empty) || r[columnCount].ToString().Equals(Common.g_sNotAvailable))
                {
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, Common.g_sNotAvailable));
                    columnCount++;
                    continue;
                }

                if (dTable.Columns[columnCount].DataType.Equals(typeof(string)))
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, r[columnCount].ToString()));
                else if (dTable.Columns[columnCount].DataType.Equals(typeof(DateTime)))
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, Common.formatToDate(r[columnCount], Common.enumDateFormatType.MM_dd_yyyy)));
                else
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":{1}," : "\"{0}\":{1}", dTable.Columns[columnCount].ColumnName, r[columnCount]));

                columnCount++;
            }
            responseJSON.Append((rowCount < dTable.Rows.Count) ? "}," : "}");
        }
        responseJSON.Append("]");

        return responseJSON.ToString(); ;
    }


    public static string getJSONString(List<List<object>> lst)
    {
        StringBuilder sbJsonStr = new StringBuilder();
        MemoryStream memStrmJSON = new MemoryStream();
        StreamReader strmRdr = new StreamReader(memStrmJSON);
        DataContractJsonSerializer jsonSeril = new DataContractJsonSerializer(typeof(List<List<object>>));

        try
        {
            jsonSeril.WriteObject(memStrmJSON, lst);
            memStrmJSON.Position = 0;
            sbJsonStr.Append(strmRdr.ReadToEnd());
        }
        catch (Exception objExp)
        {
            throw objExp;
        }
        finally
        {
            memStrmJSON.Close();
            strmRdr.Close();
            jsonSeril = null;
            memStrmJSON = null;
            strmRdr = null;
        }

        return sbJsonStr.ToString();
    }

    public static string getJSONStringWithoutDateFormatting(System.Data.DataTable dTable)
    {
        StringBuilder responseJSON = new StringBuilder();
        int rowCount = 0;

        responseJSON.Append("[");

        foreach (System.Data.DataRow r in dTable.Rows)
        {
            rowCount++;
            responseJSON.Append("{");
            int columnCount = 0;
            foreach (System.Data.DataColumn c in dTable.Columns)
            {

                //Return With N/A if the column has DBNULL
                if (r[columnCount].Equals(DBNull.Value) || r[columnCount].ToString().Equals(string.Empty) || r[columnCount].ToString().Equals(Common.g_sNotAvailable))
                {
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, Common.g_sNotAvailable));
                    columnCount++;
                    continue;
                }


                if (dTable.Columns[columnCount].DataType.Equals(typeof(string)) || dTable.Columns[columnCount].DataType.Equals(typeof(bool)))
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, r[columnCount].ToString()));
                else if (dTable.Columns[columnCount].DataType.Equals(typeof(DateTime)))
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, r[columnCount]));
                else
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":{1}," : "\"{0}\":{1}", dTable.Columns[columnCount].ColumnName, r[columnCount]));

                columnCount++;
            }
            responseJSON.Append((rowCount < dTable.Rows.Count) ? "}," : "}");
        }
        responseJSON.Append("]");

        return responseJSON.ToString(); ;
    }

    public static string getJSONStringWithoutDateFormatting(System.Data.DataTable dTable, bool hasChatterDate)
    {
        StringBuilder responseJSON = new StringBuilder();
        int rowCount = 0;

        responseJSON.Append("[");

        foreach (System.Data.DataRow r in dTable.Rows)
        {
            rowCount++;
            responseJSON.Append("{");
            int columnCount = 0;
            foreach (System.Data.DataColumn c in dTable.Columns)
            {
                //Return With N/A if the column has DBNULL
                if (r[columnCount].Equals(DBNull.Value) || r[columnCount].ToString().Equals(string.Empty) || r[columnCount].ToString().Equals(Common.g_sNotAvailable))
                {
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, Common.g_sNotAvailable));
                    columnCount++;
                    continue;
                }


                if (dTable.Columns[columnCount].DataType.Equals(typeof(string)) || dTable.Columns[columnCount].DataType.Equals(typeof(bool)))
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, r[columnCount].ToString()));
                else if (dTable.Columns[columnCount].DataType.Equals(typeof(DateTime)))
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", dTable.Columns[columnCount].ColumnName, Common.formatToDate(r[columnCount], Common.enumDateFormatType.MMM_dd_yyyy) + " at " + Common.formatToDate(r[columnCount], Common.enumDateFormatType.HH_mm_tt)));
                else
                    responseJSON.Append(string.Format((columnCount < dTable.Columns.Count - 1) ? "\"{0}\":{1}," : "\"{0}\":{1}", dTable.Columns[columnCount].ColumnName, r[columnCount]));

                columnCount++;
            }
            responseJSON.Append((rowCount < dTable.Rows.Count) ? "}," : "}");
        }
        responseJSON.Append("]");

        return responseJSON.ToString();
    }

    public static string getJSONStringWithoutDateFormatting(System.Data.DataRow row)
    {
        StringBuilder responseJSON = new StringBuilder();
        int rowCount = 0;

        //responseJSON.Append("[");

        //foreach (System.Data.DataRow r in rows)
        //{
        rowCount++;
        responseJSON.Append("{");
        int columnCount = 0;
        foreach (System.Data.DataColumn c in row.Table.Columns)
        {

            //Return With N/A if the column has DBNULL
            if (row[columnCount].Equals(DBNull.Value) || row[columnCount].ToString().Equals(string.Empty) || row[columnCount].ToString().Equals(Common.g_sNotAvailable))
            {
                responseJSON.Append(string.Format((columnCount < row.Table.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", c.ColumnName, Common.g_sNotAvailable));
                columnCount++;
                continue;
            }


            if (c.DataType.Equals(typeof(string)) || c.DataType.Equals(typeof(String)))
                responseJSON.Append(string.Format((columnCount < row.Table.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", c.ColumnName, row[columnCount].ToString()));
            else if (c.DataType.Equals(typeof(DateTime)))
                responseJSON.Append(string.Format((columnCount < row.Table.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", c.ColumnName, row[columnCount]));
            else if (c.DataType.Equals(typeof(Boolean)) || c.DataType.Equals(typeof(bool)))
                responseJSON.Append(string.Format((columnCount < row.Table.Columns.Count - 1) ? "\"{0}\":\"{1}\"," : "\"{0}\":\"{1}\"", c.ColumnName, row[columnCount]));
            else
                responseJSON.Append(string.Format((columnCount < row.Table.Columns.Count - 1) ? "\"{0}\":{1}," : "\"{0}\":{1}", c.ColumnName, row[columnCount]));

            columnCount++;
        }
        responseJSON.Append("}");
        //}
        //responseJSON.Append("]");

        return responseJSON.ToString();
    }

    /// <summary>
    /// Function to convert List objet to JSON string
    /// </summary>
    /// <param name="lst">List Object Parameter</param>
    /// <returns>JSON String</returns>
    public static string getJSONString(string json)
    {
       // StringBuilder responseJSON = new StringBuilder();
        //MemoryStream memStrmJSON = new MemoryStream();
        //StreamReader strmRdr = new StreamReader(memStrmJSON);
        //DataContractJsonSerializer jsonSeril = new DataContractJsonSerializer(typeof(List<List<object>>));

        //responseJSON.Append("[");
        //int counter = 0;
        //foreach (List<object> l in lst)
        //{
        //    responseJSON.Append("[");
        //    counter = 0;
        //    foreach (object o in l)
        //    {
                
        //        responseJSON.Append("\""+o+"\"");
        //        if (counter < l.Count)
        //        {
        //            responseJSON.Append(",");
        //        }
        //        counter++;
        //    }
        //    responseJSON.Append("[");
        //}
        //responseJSON.Append("]");
        //responseJSON.Append("[[\"a\",\"b\"],[\"a\",\"b\"],[\"a\",\"b\"]]");

        return json.ToString();
    }

    /// <summary>
    /// Function to convert List objet to JSON string
    /// </summary>
    /// <param name="lst">List Object Parameter</param>
    /// <returns>JSON String</returns>
    public static string getJSONString(List<object> lst)
    {
        StringBuilder sbJsonStr = new StringBuilder();
        MemoryStream memStrmJSON = new MemoryStream();
        StreamReader strmRdr = new StreamReader(memStrmJSON);
        DataContractJsonSerializer jsonSeril = new DataContractJsonSerializer(typeof(List<object>));

        try
        {
            jsonSeril.WriteObject(memStrmJSON, lst);
            memStrmJSON.Position = 0;
            sbJsonStr.Append(strmRdr.ReadToEnd());
        }
        catch (Exception objExp)
        {
            throw objExp;
        }
        finally
        {
            memStrmJSON.Close();
            strmRdr.Close();
            jsonSeril = null;
            memStrmJSON = null;
            strmRdr = null;
        }

        return sbJsonStr.ToString();
    }


    /// <summary>
    /// Function to convert Dictionary objet to JSON string
    /// </summary>
    /// <param name="dicnryObj">Input Dictionary value</param>
    /// <returns>JSON String</returns>

    public static string getJSONString(KeyValuePair<string, string> dicnryObj)
    {
        StringBuilder sbJsonStr = new StringBuilder();
        MemoryStream memStrmJSON = new MemoryStream();
        StreamReader strmRdr = new StreamReader(memStrmJSON);
        DataContractJsonSerializer jsonSeril = new DataContractJsonSerializer(typeof(KeyValuePair<string, string>));

        try
        {
            jsonSeril.WriteObject(memStrmJSON, dicnryObj);
            memStrmJSON.Position = 0;
            sbJsonStr.Append(strmRdr.ReadToEnd());
        }
        catch (Exception objExp)
        {
            throw objExp;
        }
        finally
        {
            memStrmJSON.Close();
            strmRdr.Close();
            jsonSeril = null;
            memStrmJSON = null;
            strmRdr = null;
        }

        return sbJsonStr.ToString();
    }

    /// <summary>
    /// Function to convert Dictionary objet to JSON string
    /// </summary>
    /// <param name="dicnryObj">Input Dictionary value</param>
    /// <returns>JSON String</returns>
    public static string getJSONString(Dictionary<string, string> dictryObj)
    {
        StringBuilder sbJsonStr = new StringBuilder();

        try
        {
            sbJsonStr.Append("[{");

            foreach (KeyValuePair<string, string> keyValObj in dictryObj)
            {
                sbJsonStr.AppendFormat("\"{0}\":\"{1}\"", keyValObj.Key, HttpUtility.HtmlEncode(keyValObj.Value));
                sbJsonStr.AppendFormat(",");
            }
            sbJsonStr.Remove(sbJsonStr.ToString().LastIndexOf(','), 1);
            sbJsonStr.Append("}]");

        }
        catch (Exception objExp)
        {
            throw objExp;
        }

        return sbJsonStr.ToString();
    }


    /// <summary>
    /// Function to convert Name/Value pair to JSON string
    /// </summary>
    /// <param name="sName">Key</param>
    /// <param name="sValue">Value</param>
    /// <returns>JSON String</returns>
    public static string getJSONString(string sName, string sValue)
    {
        return string.Format("[{{\"{0}\":\"{1}\"}}]", sName, sValue);
    }

    /// <summary>
    /// Function to convert Name/Value pair to JSON string
    /// </summary>
    /// <param name="sName">Key</param>
    /// <param name="sValue">Value</param>
    /// <returns>JSON String</returns>
    public static string getJSONString(string sName, string sValue, bool bWithCollection = false)
    {
        return bWithCollection ? string.Format("[{{\"{0}\":\"{1}\"}}]", sName, sValue) : string.Format("{{\"{0}\":\"{1}\"}}", sName, sValue);
    }
    #endregion
}