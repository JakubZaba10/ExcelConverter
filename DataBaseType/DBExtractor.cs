using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelDBConverter
{
    public abstract class DBExtractor
    {
        protected List<string> DatabaseSchemaList = new List<string>();

        protected FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        protected virtual string FormatConnectionString(string connectionString, string user, string password)
        {
            string fullConeectionString = "Data Source="
                + connectionString
                + ";User ID="
                + user
                + ";Password="
                + password + ";";
            return fullConeectionString;
        }

        public virtual void Connect(string connectionString, string user, string password)
        {
            folderBrowserDialog.ShowDialog();
            Task task = new Task(() =>
            {
                string fullConeectionString = FormatConnectionString(connectionString, user, password);
                var sqlFile = GetSQLFile();
                var queryLocation = SetQueryLocation(sqlFile);
                ExtractData(fullConeectionString, queryLocation);
            });
            task.Start();
        }

        protected string SetQueryLocation(string sqlFile)
        {
            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            var queryLocation = path + sqlFile;
            return queryLocation;
        }

        protected abstract void ExtractData(string fullConeectionString, string queryLocation);

        protected abstract string GetSQLFile();

        protected void CreateExcelFile()
        {
            var excel = new FileManager();
            excel.FromDBToCSV(DatabaseSchemaList, folderBrowserDialog.SelectedPath);
        }

    }
}
