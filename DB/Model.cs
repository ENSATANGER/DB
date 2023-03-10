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
            List<GetType()> L = new List<GetType>();
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
            List <GetType()> L = new List<GetType>();

            sql = "select * from " + GetType().Name + " where ";
            foreach (KeyValuePair<string, Etudiant> e in dico)
                sql+= e.Key + "=" + e.Value);

            IDataReader reader = Connexion.Select(sql);

            while (reader.Read())
            {
                Dictionary<string, object> dico = new Dictionary<string, object>();

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
