using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    internal abstract class Model
    {
        public int id = 0;
        private string sql = "";

        
        public List<dynamic> All()
        {
            List<dynamic> L = new List<dynamic>();
            sql = "select * from" + GetType().Name;
            IDataReader reader = Connexion.Select(sql);

            while (reader.Read())
            {
                Dictionary<string, object> dico = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                    dico.Add(reader.GetName(i),reader.GetValue(i));

                 L.Add(dico);
            }
            reader.Close();
            return L;
        }

        public static List<dynamic> all<T>()
        {
            return new List<dynamic>();
        }

        public List<dynamic> Select(Dictionary<string, object> dico)
        {
            List <dynamic> L = new List<dynamic>();

            sql = "select * from " + GetType().Name + " where ";
            foreach (KeyValuePair<string, object> e in dico)
                sql+= e.Key + "=" + e.Value;

            IDataReader reader = Connexion.Select(sql);

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    dico.Add(reader.GetName(i), reader.GetValue(i));

                L.Add(dico);
            }
            reader.Close();
            return L;
        }
        public static List<dynamic> select<T>(Dictionary<string, object> dico)
        {
                return new List<dynamic>();
        }
    }
}
