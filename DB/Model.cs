using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;



namespace DB
{
    public abstract class Model
    {
        private int Id = 0;
        private string sql = "";

        public int id
        {
            get { return Id; }
            set { Id = value; }
        }
        public Dictionary<string, T> ObjectToDictionary<T>(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var dico = JsonConvert.DeserializeObject<Dictionary<string, T>>(json);
            return dico;
        }

        public dynamic DictionaryToObject(Dictionary<string, object> dico)
        {
            var model = Activator.CreateInstance(GetType());
            PropertyInfo[] properties = GetType().GetProperties();

            foreach (var property in properties)
            {
               
                if (dico.Keys.Contains(property.Name))
                    property.SetValue(model, dico[property.Name]);
            }

            return model;
        }

        public int save()
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();
            dico = ObjectToDictionary<string>(this);

            Dictionary<string, string> ChampsTable = Connexion.getChamps_table(GetType().Name);

            if (Id == 0)
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("INSERT INTO ");
                sqlBuilder.Append(GetType().Name);
                sqlBuilder.Append("(");

                int c = 0;
                foreach (KeyValuePair<string, string> kvp in ChampsTable)
                {
                    if (kvp.Value != "id")
                    {
                        if (c > 0)
                            sqlBuilder.Append(",");
                        sqlBuilder.Append(kvp.Value);
                        c++;
                    }
                }
                sqlBuilder.Append(")");
                sqlBuilder.Append(" VALUES (");

                c = 0;
                foreach (KeyValuePair<string, string> kvp in dico)
                {
                    if (kvp.Key != "id")
                    {
                        if (c > 0)
                            sqlBuilder.Append(",");
                        sqlBuilder.Append("'");
                        sqlBuilder.Append(kvp.Value);
                        sqlBuilder.Append("'");
                    }
                    c++;
                }
                sqlBuilder.Append(")");
                sqlBuilder.Append(";");

                sql = sqlBuilder.ToString();
            
            }
            else
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("UPDATE ");
                sqlBuilder.Append(this.GetType().Name);
                sqlBuilder.Append(" SET ");

                int c = 0;
                // work with two dictionaries at the same time
                foreach (var Champ in ChampsTable)
                {
                    if (Champ.Value != "id")
                    {
                        if (c > 0)
                            sqlBuilder.Append(",");
                        foreach(var val in dico)
                        {
                            if(Champ.Value == val.Key)
                            {
                                sqlBuilder.Append(Champ.Value);
                                sqlBuilder.Append("=");
                                sqlBuilder.Append("'");
                                sqlBuilder.Append(val.Value);
                                sqlBuilder.Append("' ");

                            }
                        }
                        c++;
                    }
                }
                sqlBuilder.Append("WHERE id = ");
                sqlBuilder.Append(Id);
                sqlBuilder.Append(";");

                sql = sqlBuilder.ToString();

            }

            int v = Connexion.IUD(sql);
            if (v != 0 && v != -1)
                return 0;
            if (v == -1)
                return -2;// Exception from UID
            return -1; // cas d'erreur
        }

        public dynamic find()
        {
            Dictionary<string, object> dico = new Dictionary<string, object>();

            sql = "select * from " + this.GetType().Name + " where id=" + Id;

            IDataReader data = Connexion.Select(sql);

            while (data.Read())
            {
                for (int i = 0; i < data.FieldCount; i++)
                {
                    dico.Add(data.GetName(i), data.GetValue(i));
                }
            }
            data.Close();
            return DictionaryToObject(dico);
        }

        //louay's contribution

        public static dynamic find<T>(int id) where T : Model
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            obj.id = id;
            return obj.find();
        }


        public int delete()
        {
            string req = "DELETE from " + this.GetType().Name + " where id = " + Id;

            //execute query and read data with IDataReader
            return Connexion.IUD(req);
        }


        public List<dynamic> All()
        {
            List<dynamic> L = new List<dynamic>();
            sql = "select * from " + GetType().Name;
            IDataReader reader = Connexion.Select(sql);

            while (reader.Read())
            {
                Dictionary<string, object> dico = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                    dico.Add(reader.GetName(i), reader.GetValue(i));

                L.Add(DictionaryToObject(dico));
            }
            reader.Close();
            return L;
        }

        public static List<dynamic> all<T>() where T : Model
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            return obj.All();
        }

        public List<dynamic> Select(Dictionary<string, object> dico)
        {

            List<dynamic> L = new List<dynamic>();

            sql = "select * from " + GetType().Name + " where ";
            int c = 0;
            foreach (KeyValuePair<string, object> e in dico)
            {
                if (e.Key == "id")
                {
                    string s=""; s += e.Value;
                    if (s == "0") continue;
                }

                if (e.Value != null)
                {
                    if (c > 0)
                        sql += " and ";
                    sql += e.Key + "='" + e.Value + "'";
                    c++;
                }
            }
            
            IDataReader reader = Connexion.Select(sql);
            
            while (reader.Read())
            {
                dico.Clear();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dico.Add(reader.GetName(i), reader.GetValue(i));
                }

                L.Add(DictionaryToObject(dico));
            }
            reader.Close();
            return L;
        }
        public static List<dynamic> select<T>(Dictionary<string, object> dico) where T : Model
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            return obj.Select(dico);
        }

        public override string ToString()
        {
            return "ID : " + Id;
        }
    }
}