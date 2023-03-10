﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
            Dictionary<string, T> keyValues = new Dictionary<string, T>();
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                keyValues.Add(property.Name, (T)property.GetValue(obj));
            }
            return keyValues;
        }

        private dynamic DictionaryToObject(Dictionary<String, object> dico)
        {
            dynamic obj = new ExpandoObject();
            var dictonary = (IDictionary<string, object>)obj;
            foreach (KeyValuePair<string, object> kvp in dico)
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

            if (this.id == 0)
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("INSERT INTO ");
                sqlBuilder.Append(this.GetType().Name);
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

            for (int i = 0; i < data.FieldCount; i++)

                dico.Add(data.GetName(i), data.GetValue(i));

            return DictionaryToObject(dico);
        }

        /*public List<dynamic> All()
        {
            List<dynamic> L = new List<dynamic>();
            sql = "select * from" + GetType().Name;
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
        }*/

        public static List<dynamic> all<T>()
        {
            return new List<dynamic>();
        }

        public List<dynamic> Select(Dictionary<string, object> dico)
        {

            List<dynamic> L = new List<dynamic>();

            sql = "select * from " + GetType().Name + " where ";
            foreach (KeyValuePair<string, object> e in dico)
                sql += e.Key + "=" + e.Value;

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

        //louay's contribution

        public static dynamic find<T>(int id)
        {
            Dictionary<string, object> dico = new Dictionary<string, object>();

            
            String sql = "select * from " + typeof(T).Name + " where id = @id";

            
            //open connexion using Connexion class
            Connexion.Connect();
            
            //create command object using IDbCommand interface
            //and the opened connection object
            IDbCommand cmd = Connexion.con.CreateCommand();
            cmd.CommandText = sql;
            
            //add id parameter to command
            IDbDataParameter param = cmd.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;

            //add param to cmd object
            cmd.Parameters.Add(param);
            
            //execute query and read data with IDataReader
            IDataReader reader = cmd.ExecuteReader();

            // loop through columns and rows to add the name and value to disco
            if (reader.Read())
            {
                for(int i=0; i< reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    Object columnValue = reader.GetValue(i);
                    disco[columnName] = columnValue;
                }
            }
            reader.Close();
            Connexion.con.Close();
            return DictionaryToObject<T>(dico);
        }

        
        ////////////////////////////////////////////
     
        public int delete()
        {
            int rowsAffected = 0;
            String sql = "DELETE from " + this.GetType().Name + " where id = @id";


            //open connexion using Connexion class
            Connexion.Connect();

            //create command object using IDbCommand interface
            //and the opened connection object
            IDbCommand cmd = Connexion.con.CreateCommand();
            cmd.CommandText = sql;

            //add id parameter to command
            IDbDataParameter param = cmd.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;

            //add param to cmd object
            cmd.Parameters.Add(param);

            //execute the delete query and get the number of deleted rows
            rowsAffected = cmd.ExecuteNonQuery();

            Connexion.con.Close();
            return rowsAffected;
        }

        ////////////////////////////////////

        public List<dynamic> All()
        {
            List<dynamic> records = new List<dynamic>();

            // Execute a query to retrieve all records for the table
            string query = $"SELECT * FROM {GetTableName()}";
            
            // it seems like using(...){...} is a better way to use reader
            // to ensure that there is no resource leaks
            using (IDataReader reader = Connexion.Select(query))
            {
                while (reader.Read())
                {
                    Dictionary<string, object> record = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object value = reader.GetValue(i);
                        record[columnName] = value;
                    }
                    records.Add(record);
                }
            }
            return records;
        }
    }
}