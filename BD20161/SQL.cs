using MySql.Data.MySqlClient;
using System.Data;

namespace BD20161
{
    public static class SQL
    {
        public static MySqlConnection con = new MySqlConnection();

        public static DataTable GetComments(string queryString)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                dt.Load(new MySqlCommand(queryString, con).ExecuteReader());
            }
            catch { }
            con.Close();
            return dt;
        }

        public static string GetComment(string queryString)
        {
            try
            {
                string s = GetComments(queryString).Rows[0].ItemArray[0].ToString();
                return s;
            }
            catch
            {
                return "";
            }
        }
    }
}