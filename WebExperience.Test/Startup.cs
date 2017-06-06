using Microsoft.Owin;
using Owin;
using GeneralKnowledge.Test.App.Tests;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

[assembly: OwinStartupAttribute(typeof(WebExperience.Test.Startup))]
namespace WebExperience.Test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlStr = "select top 10 * from assetTable";
            var csvProcessing = new CsvProcessingTest();
            DataTable csvDataTable = csvProcessing.accessList();

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    using (var command = new SqlCommand(sqlStr, connection))
                    {
                        connection.Open();
                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            if (!dr.HasRows)
                            {
                                insertDataTable(csvDataTable);
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch
            {

            }
            ConfigureAuth(app);
        }

        public static void insertDataTable(DataTable csvDataTable)
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);

                    bulkCopy.DestinationTableName = "assetTable";

                    connection.Open();
                    bulkCopy.WriteToServer(csvDataTable);
                    connection.Close();
                }

                catch
                {

                }

            }
        }
    }
}
