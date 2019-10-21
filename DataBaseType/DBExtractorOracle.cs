using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using System.Data;

namespace ExcelDBConverter
{
    public class DBExtractorOracle : DBExtractor
    {
        protected override void ExtractData(string fullConeectionString, string queryLocation)
        {
            using (OracleConnection connection = new OracleConnection(fullConeectionString))
            {
                try
                {
                    connection.Open();
                    var queryFromFile = File.ReadAllText(queryLocation);

                    using (OracleCommand cmd = new OracleCommand(queryFromFile, connection))
                    {
                        CreateTempTable(cmd);
                        DoQueryFromFile(queryFromFile, cmd);
                        FillListFromTableSelect(DatabaseSchemaList, cmd);
                        DropTempTable(cmd);
                        CreateExcelFile();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void FillListFromTableSelect(List<string> list, OracleCommand cmd)
        {
            cmd.CommandText = "select * from TEMP_TO_FORMAT";

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader[0].ToString());
            }
        }

        private void DropTempTable(OracleCommand cmd)
        {
            cmd.CommandText = "drop table TEMP_TO_FORMAT";
            cmd.ExecuteNonQuery();
        }

        private void DoQueryFromFile(string queryFromFile, OracleCommand cmd)
        {
            cmd.CommandText = queryFromFile;
            cmd.ExecuteNonQuery();
        }

        private void CreateTempTable(OracleCommand cmd)
        {
            cmd.CommandText = "create table TEMP_TO_FORMAT (record varchar2(4000))";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        protected override string GetSQLFile()
        {
            return "OracleSQL.sql";
        }
    }
}
