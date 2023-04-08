using MySql.Data.MySqlClient;
using Oracle.DataAccess.Client;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DB
{
    public class Connexion
    {
        static IDbConnection con = null;
        static IDbCommand cmd = null;
        
        public static IDbConnection Con
        {
            get { return con; }
            set { con = value; }
        }

        public static IDbCommand Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
        
        public static void Connect()
        {
            if (con == null)
            {
                Dictionary<string, string> D = DB("SqlServer");
                try
                {
<<<<<<< Updated upstream
                    con = new MySqlConnection("Data source=" + D["Server"] + "; uid=" + D["uid"] +
                        "; pwd=" + D["pwd"] + "; Database=" + D["Database"]);
=======
                    con = new SqlConnection("Data source=" + D["Server"] + ";Initial Catalog=" + D["InitialCatalog"] + ";Integrated Security=" + D["IntegratedSecurity"]);
>>>>>>> Stashed changes
                }
                catch (Exception SqlServer)
                {
                    D = DB("MySql");
                    try
                    {
<<<<<<< Updated upstream
                        con = new SqlConnection("Data source=" + D["Server"] +
                            ";Initial Catalog=" + D["InitialCatalog"] + ";Integrated Security=" +
                            D["IntegratedSecurity"]);
=======
                        con = new MySqlConnection("Data source=" + D["Server"] + "; uid=" + D["uid"] + "; pwd=" + D["pwd"] + "; Database=" + D["Database"]);
>>>>>>> Stashed changes
                    }
                    catch (Exception MySql)
                    {
                        Console.WriteLine("aucune base de données disponible");
                    }
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
            cmd = con.CreateCommand();
            cmd.CommandText = req;
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
        public static Dictionary<string, string> DB(string db)
        {

            XDocument XDatabases = XDocument.Load("./../../../../DB/DB/env.xml");

            XElement XRoot = XDatabases.Root;
            var XDBS = XRoot.Element(db);

            if (XDBS != null)
            {
                Dictionary<string, string> D = new Dictionary<string, string>();

                foreach (XElement el in XDBS.Elements())
                   D.Add((el.Name).ToString(),el.Value);
                return D;
            }
            return null;
        }
    }
}