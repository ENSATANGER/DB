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
            if (con == null)
            {
                try
                {
                    con = new SqlConnection("Data Source=localhost;Initial Catalog=ENSAT_TANGER;Integrated Security=True");
                    
                }
                catch (Exception ex)
                {
                    //MySql CNX!!!!!!!!!
                }
            }
            if (con.State.ToString() == "Closed")
                con.Open();
        }
        public static int IUD(string req)
        {
            Connect();
            try{
                cmd = con.CreateCommand();
                cmd.CommandText = req;
                return cmd.ExecuteNonQuery();
            }
            catch(SqlException ex){
                return -1;
            }
        }

        public static IDataReader Select(string req)
        {
            Connect();
            cmd = con.CreateCommand();
            cmd.CommandText = req;
            IDataReader rd = cmd.ExecuteReader();
            return rd;
        }
         public static Dictionary<string, string> getChamps_table(string table)
        {
            Connect();
            Dictionary<string, string> champs = new Dictionary<string, string>();
            string req = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table;";
            cmd = new SqlCommand(req);
            cmd.Connection = con;
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = "@table";
            parameter.Value = table;
            cmd.Parameters.Add(parameter);
            IDataReader dr = cmd.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                champs.Add("Champs " + (i + 1), dr.GetString(0));
                i++;
            }
            dr.Close();
            return champs;

        }
    }
}