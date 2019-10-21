using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelDBConverter
{
    public class ExcelManager
    {
        private string[] fileStructure;
        private string[] header;
        private IEnumerable<IGrouping<string, Table>> grouped;
        private Application app;
        private ProgressUIComponent progressUIComponent;

        public ExcelManager(Application _app, ProgressUIComponent _progressUIComponent)
        {
            app = _app;
            progressUIComponent = _progressUIComponent;
        }

        public void ConvertFormatToCSV(string pathFileToFormat, string pathFileFormated)
        {
            var wb = app.Workbooks.Open(pathFileToFormat);
            wb.SaveAs(pathFileFormated, XlFileFormat.xlCSVWindows);
            wb.Close();
        }

        public void GetFileStructure(string path) => fileStructure = File.ReadAllLines(path);

        public void CreateFormatedStructure()
        {
            var query = from line in fileStructure.Skip(1)
                        let data = line.Split(',')
                        select new Table
                        {
                            TableName = data[0],
                            FieldName = data[1],
                            DataType = data[2],
                            IsNullable = data[3],
                            PK = data[4],
                            References = data[5],
                            Constraints = data[6],
                            Description = data[7],
                        };
            grouped = query.OrderBy(n => n.TableName).ThenBy(m=> m.FieldName).GroupBy(tab => tab.TableName);
            header = fileStructure[0].Split(',').Skip(1).ToArray();
            header[2] = "Null";
        }

        public void CreateFileWithStyles(PathManager pathManager)
        {
            var workbook = app.Workbooks.Add("");
            PrepareWorkBook(workbook);
            workbook.SaveAs(pathManager.PathWithResultFolder
                            + @"\"
                            + pathManager.FileName
                            + ".xlsx", XlFileFormat.xlOpenXMLWorkbook);
            workbook.Close();
        }

        private void PrepareWorkBook(Workbook workbook)
        {
            var sheet = (Worksheet)workbook.ActiveSheet;
            int row = 2;
            int currentProgress = 0;
            int maxProgress = grouped.Count();
            foreach (var table in grouped)
            {
                UpdateUIProgressBar(ref currentProgress, maxProgress);
                List<string[]> tableToFill = new List<string[]>();
                CreateTableName(sheet, ref row, table);
                CreateTableHeaders(sheet, ref row);
                CreateTableContentToFill(table, tableToFill);
                FillTableContent(sheet, ref row, tableToFill);

                sheet.Columns.AutoFit();
            }
        }

        private void UpdateUIProgressBar(ref int _currentProgress, int maxProgress)
        {
            _currentProgress++;
            progressUIComponent.ProgressValue = (double)_currentProgress / maxProgress * 100;
        }

        private static void CreateTableName(Worksheet sheet, ref int row, IGrouping<string, Table> table)
        {
            sheet.Cells[row, 1] = table.Key;
            sheet.Range["A" + row.ToString(), "A" + row.ToString()].Font.Bold = true;
            row++;
        }

        private void CreateTableHeaders(Worksheet sheet, ref int row)
        {
            var headerRange = ReturnRange(sheet, row);
            headerRange.Value = header;
            AddHeaderStyle(headerRange);
            row++;
        }

        private static void CreateTableContentToFill(IGrouping<string, Table> table, List<string[]> tableToFill)
        {
            foreach (var line in table)
            {
                string[] toFill = line.ReturnTableRow();
                tableToFill.Add(toFill);
            }
        }

        private void FillTableContent(Worksheet sheet, ref  int row, List<string[]> tableToFill)
        {
            var tableRange = ReturnRange(sheet, row, (row + tableToFill.Count() - 1));
            tableRange.Value = tableToFill.List2DArray();
            AddBorderStyle(tableRange);
            row += tableToFill.Count() + 1;
        }

        private Range ReturnRange(Worksheet sheet, int start, int end)
        {
            return sheet.Range["A" + start.ToString(), "G" + end.ToString()];
        }

        private Range ReturnRange(Worksheet sheet, int row)
        {
            return sheet.Range["A" + row.ToString(), "G" + row.ToString()];
        }

        private void AddHeaderStyle(Range range)
        {
            range.Borders.LineStyle = XlLineStyle.xlContinuous;
            range.Interior.Color = XlRgbColor.rgbLightGrey;
        }

        private void AddBorderStyle(Range range) => range.Borders.LineStyle = XlLineStyle.xlContinuous;

        private class Table
        {
            public string TableName { get; set; }
            public string FieldName { get; set; }
            public string DataType { get; set; }
            public string IsNullable { get; set; }
            public string PK { get; set; }
            public string References { get; set; }
            public string Constraints { get; set; }
            public string Description { get; set; }

            public string[] ReturnTableRow()
            {
                string[] toFill = { FieldName,
                                    DataType,
                                    IsNullable,
                                    PK,
                                    References != "" ? References : "NULL",
                                    Constraints != "" ? Constraints : "NULL",
                                    Description != "" ? Description : "NULL"
                };
                return toFill;
            }
        }
    }
}

