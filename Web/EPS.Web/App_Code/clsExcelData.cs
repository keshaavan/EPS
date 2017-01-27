using System;
using System.Data;
using System.IO;

using OfficeOpenXml;
using EPS.Resources;

/// <summary>
/// Summary description for clsExcelData
/// </summary>
public class clsExcelData
{
    public static DataSet ReadExcel(string fileName, string sheet)
    {
        try
        {
            ////// Create a new Adapter
            ////OleDbDataAdapter objDataAdapter = new OleDbDataAdapter();
            ////// retrieve the Select command for the Spreadsheet
            ////objDataAdapter.SelectCommand = ExcelCommand(fileName, sheet);
            // Create a DataSet
            DataSet objDataSet = new DataSet();
            FileInfo f = new FileInfo(fileName);
            using (var pck = new OfficeOpenXml.ExcelPackage(f))
            {
                var ws = pck.Workbook.Worksheets["Sheet1"];//.First();

                DataTable tbl = new DataTable();
                tbl.Columns.Add(DBResources.col_ClientMarket, typeof(string));
                tbl.Columns.Add(DBResources.col_FileName, typeof(string));
                tbl.Columns.Add(DBResources.col_ClientReference, typeof(string));
                tbl.Columns.Add(DBResources.col_ReceivedDate, typeof(DateTime));

                var startRow = 2;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var row = tbl.NewRow();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                    tbl.Rows.Add(row);
                }

                objDataSet.Tables.Add(tbl);

                // Populate the DataSet with the spreadsheet worksheet data
                return objDataSet;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }
}
