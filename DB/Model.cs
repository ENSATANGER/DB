using System;
using System.Collections.Generic;
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
            if(this.id == 0)
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("INSERT INTO ");
                sqlBuilder.Append(this.GetType().Name);
                sqlBuilder.Append("(");

                int c = 0;
                foreach (KeyValuePair<string, string> kvp in dico)
                {
                    if(kvp.Key != "id"){
                        if (c > 0)
                            sqlBuilder.Append(",");
                        sqlBuilder.Append(kvp.Key);
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
                foreach (KeyValuePair<string, string> kvp in dico)
                {
                    if (kvp.Key != "id")
                    {
                        if (c > 0)
                            sqlBuilder.Append(",");
                        sqlBuilder.Append(kvp.Key);
                        sqlBuilder.Append("=");
                        sqlBuilder.Append("'");
                        sqlBuilder.Append(kvp.Value);
                        sqlBuilder.Append("'");
                        c++;
                    }
                }
                sqlBuilder.Append("WHERE id = ");
                sqlBuilder.Append(id);

                sql = sqlBuilder.ToString();
            }
            return -1;
        }

        public dynamic find()
        {
            Dictionary<string, object> dico = new Dictionary<string, object>();

            sql = "select * from " + this.GetType().Name + " where id=" + id;

            return DictionaryToObject(dico);
        }
    }
}
