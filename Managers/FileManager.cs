using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace ExcelDBConverter
{
    public class FileManager
    {
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

        public void DeleteFile(string path) => File.Delete(path);

        public void FromDBToCSV(List<string> list, string pathToPlace)
        {
            var pathToFile = pathToPlace + "\\tempCSV.csv";
            using (StreamWriter sw = File.CreateText(pathToFile))
            {
                sw.WriteLine("Table name,Field name,Data type,Is nullable,PK,References,Constraints,Description");
                foreach (var row in list)
                {
                    sw.WriteLine(row);
                }
                sw.Close();
            }

            Application app = new Application();
            Workbook wb = app.Workbooks.Open(pathToFile);
            wb.SaveAs(pathToPlace + "\\temp.xlsx", XlFileFormat.xlOpenXMLWorkbook);
            DeleteFile(pathToFile);
            wb.Close();
            app.Quit();
        }
    }
}
