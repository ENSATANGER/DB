using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    abstract class Model
    {
        public int id = 0;
        private string sql = "";

        Dictionary<string, T> ObjectToDictionary<T>(object obj)
        {
            Dictionary<string,T> keyValues = new Dictionary<string,T>();
            var properties = obj.GetType().GetProperties();
            foreach ( var property in properties )
            {
                keyValues.Add(property.Name,(T)property.GetValue(obj));
            }
            return keyValues;
        }

        private dynamic DictionaryToObject(Dictionary<String, object> dico)
        {
            dynamic obj = new ExpandoObject();
            var dictonary = (IDictionary<string, object>)obj;
            foreach(KeyValuePair<string, object> kvp in dico)
            {
                dictonary.Add(kvp.Key, kvp.Value);
            }
            return obj;
        }

        public int save()
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();
            dico = ObjectToDictionary<string>(this);

            Dictionary<string, string> ChampsTable = Connexion.getChamps_table(this.GetType().Name);

            if(this.id == 0)
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("INSERT INTO ");
                sqlBuilder.Append(this.GetType().Name);
                sqlBuilder.Append("(");

                int c = 0;
                foreach (KeyValuePair<string, string> kvp in ChampsTable)
                {
                    if(kvp.Value != "id"){
                        if (c > 0)
                            sqlBuilder.Append(",");
                        sqlBuilder.Append(kvp.Value);
                        c++;
                    }
                }
                sqlBuilder.Append(")");
                sqlBuilder.Append(" VALUES (");

                c = -1;
                foreach (KeyValuePair<string, string> kvp in dico)
                {
                    if (c != -1) // ignorer valeur de l'id
                    {
                        if (c > 0)
                            sqlBuilder.Append(",");
                        sqlBuilder.Append("'");
                        sqlBuilder.Append(kvp.Key);
                        sqlBuilder.Append("'");
                    }
                    c++;
                }
                sqlBuilder.Append(")");

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
                foreach (var pair in ChampsTable.Zip(dico, (champ, newvalue) => new { Champ = champ, NewValue = newvalue }))
                {
                    if (pair.Champ.Value != "id")
                    {
                        if (c > 0)
                            sqlBuilder.Append(",");
                        sqlBuilder.Append(pair.Champ.Value);
                        sqlBuilder.Append("=");
                        sqlBuilder.Append("'");
                        sqlBuilder.Append(pair.NewValue.Value);
                        sqlBuilder.Append("'");
                        c++;
                    }
                }
                sqlBuilder.Append("WHERE id = ");
                sqlBuilder.Append(id);

                sql = sqlBuilder.ToString();
            }
            
            if (Connexion.IUD(sql) != 0)
                return 0;

            return -1; // cas d'erreur
        }

        public dynamic find()
        {
            Dictionary<string, object> dico = new Dictionary<string, object>();

            sql = "select * from " + this.GetType().Name + " where id=" + id;

            IDataReader data = Connexion.Select(sql);

            for(int i = 0; i < data.FieldCount; i++)

                dico.Add(data.GetName(i),data.GetValue(i));

            return DictionaryToObject(dico);
        }
    }
}
