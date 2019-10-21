using System;
using System.Data.SqlClient;
using System.IO;

namespace ExcelDBConverter
{
    public class DBExtractorMS : DBExtractor
    {
        protected override void ExtractData(string fullConeectionString, string queryLocation)
        {
            using (SqlConnection connection = new SqlConnection(fullConeectionString))
            {
                try
                {
                    connection.Open();
                    var queryFromFile = File.ReadAllText(queryLocation);

                    using (SqlCommand command = new SqlCommand(queryFromFile, connection))
                    {
                        command.CommandText = queryFromFile;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string newRow = "";
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                newRow += reader[i] + ",";
                            }
                            DatabaseSchemaList.Add(newRow);
                        }
                        CreateExcelFile();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        protected override string GetSQLFile()
        {
            return "MSSQL.sql";
        }
    }
}
