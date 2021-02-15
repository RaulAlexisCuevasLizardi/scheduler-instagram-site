

using System.Data.SqlClient;

namespace BillysWebsite.Helpers
{
    public class DatabaseHelper
    {
        private SqlConnection conn;

        public void OpenConection()
        {
            conn = new SqlConnection("Data Source=DESKTOP-FCNCI3B;Initial Catalog=BillysWebsiteDB;Integrated Security=True");
            conn.Open();
        }

        public void CloseConnection()
        {
            conn.Close();
        }

        public int ExecuteQueries(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            return cmd.ExecuteNonQuery();
        }

        public SqlDataReader DataReader(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                return cmd.ExecuteReader();
            }
            catch (System.Exception)
            {

                return null;
            }
        }
    }
}