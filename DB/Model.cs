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

        Dictionary<string, T> ObjectToDictionary<T>(object obj)
        {
            return null;
        }
        private dynamic DictionaryToObject(Dictionary<string, object> dico)
        {
            return null;
        }
        public int save()
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();
            dico = ObjectToDictionary<string>(this);
            return 1;
        }


        public dynamic find(int id)
        {
            Dictionary<string, object> dico = new Dictionary<string, object>();
            sql = "select * from " + this.GetType().Name + " where id=" + id;
            return DictionaryToObject(dico);
        }

       /* public static dynamic find<T>(int id)
        {
            Dictionary<string, object> dico = new Dictionary<string, object>();
            sql = "select * from "+" where id=" + id;
            return DictionaryToObject(dico);
        }

        public int delete()
        {
        }*/
        public List<dynamic> All()
        {
            List<dynamic> L = new List<dynamic>();
            sql = "select * from" + GetType().Name;
            IDataReader reader = Connexion.Select(sql);

            while (reader.Read())
            {
                Dictionary<string, object> dico = new Dictionary<string, object>();

                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                dico.Add(reader.GetInt32(0), new );
                 L.Add(dico);

            }

            reader.Close();
            return new List<dynamic>();
        }
        public static List<dynamic> all<T>()
        {
            return new List<dynamic>();
        }
        public List<dynamic> Select(Dictionary<string, object> dico)
        {
            return new List<dynamic>();
        }
        public static List<dynamic> select<T>(Dictionary<string, object> dico)
        {
                return new List<dynamic>();
        }
    }
}
