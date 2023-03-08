using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Connexion
    {
        static IDbConnection con = null;
        static IDbCommand cmd = null;
        public static void Connect()
        {
            try
            {
                con = new SqlConnection("Data Source=DESKTOP-9UQDINE;Initial Catalog=ENSA_TANGER;Integrated Security=True");

            }
            catch (Exception ex)
            {
                //MySql CNX!!!!!!!!!
            }
        }
        public static int IUD(string req)
        {
            try
            {
                cmd = con.CreateCommand();
                cmd.CommandText = req;
                return cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static IDataReader Select(string req)
        {
            try
            {
                cmd = con.CreateCommand();
                cmd.CommandText = req;
                return cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                return null;//return (IDataReader)ex;?????????
            }
        }
        public static Dictionary<string, string> getChamps_table(string table)
        {
            Dictionary<string, string> champs = new Dictionary<string, string>();
            try
            {
                string req = "SELECT COLUMN_NAME\r\nFROM INFORMATION_SCHEMA.COLUMNS\r\nWHERE TABLE_NAME = '@table';";
                cmd = new SqlCommand(req);
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@table", table));
                IDataReader dr = cmd.ExecuteReader();
                int i = 0;
                while (dr != null)
                {
                    champs.Add("Champs " + (i + 1), dr.GetString(i));
                    i++;
                }
                return champs;
            }
            catch (Exception ex)
            {
                return null;//return (IDataReader)ex;?????????
            }

        }
    }
}
