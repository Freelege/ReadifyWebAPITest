using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Util
{
    class TestUtil
    {
        public static void DetachDB(string dbPhysicalFileFullName)
        {
            try
            {
                // string connectString = @"Server = (localdb)\MSSQLLocalDB; Integrated Security = SSPI;";  //No database is specified here
                string connectString = ConfigurationManager.ConnectionStrings["localDBMaster"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    string sql = "Select d.name from " +
                        "(sys.databases d inner join sys.master_files m on d.database_id = m.database_id) " +
                        $"where m.physical_name = '{dbPhysicalFileFullName}'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        string dbLogicalName = "";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dbLogicalName = reader.GetValue(0).ToString();  //Only need to get one value
                            }
                        }
                        //before detaching, closes all the connections opened by the calling process
                        SqlConnection.ClearAllPools();

                        cmd.CommandText = $"exec master.dbo.sp_detach_db @dbname = '{dbLogicalName}'";
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            catch
            {
            }
        }
    }
}
