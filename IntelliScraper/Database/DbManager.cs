using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Database
{
    public class DbManager
    {
        public PetaPoco.Database db { get; set; }
        QueryBuilder qb = new QueryBuilder();
        public DbManager(string connectionString,string providerName)
        {
            this.db = new PetaPoco.Database(connectionString, providerName);
        }

        public ICollection<KeyValuePair<String, Object>> queryFirstOrDefault(string query)
        {
            return (ICollection<KeyValuePair<String, Object>>)db.FirstOrDefault<object>(query);
        }

        public ICollection<KeyValuePair<String, Object>> query(string query)
        {
            return (ICollection<KeyValuePair<String, Object>>)db.Query<object>(query);
        }

        public bool exist(List<string> existByAttributes, string table, List<KeyValuePair<string, object>> record)
        {            
            string query = qb.existQuery(existByAttributes, table, record);
            if (!string.IsNullOrEmpty(query))
            {
                ICollection<KeyValuePair<String, Object>> coll = queryFirstOrDefault(query);
                if (coll != null)
                    return true;
            }
            return false;
        }
        
        public void insert(List<string> existByAttributes, string table, List<KeyValuePair<string, object>> record)
        {
            string query = qb.insertQuery(existByAttributes, table, record);
            executeQuery(query);         
        }

        public void update(List<string> existByAttributes, List<string> columns, string table, List<KeyValuePair<string, object>> record)
        {
            string query = qb.updateQuery(existByAttributes, columns,table, record);
            executeQuery(query);      
        }
        
        public void delete(List<string> existByAttributes,  string table, List<KeyValuePair<string, object>> record)
        {
            string query = qb.deleteQuery(existByAttributes,  table, record);
            executeQuery(query);
        }

        public int executeQuery(string query)
        {
            if (!string.IsNullOrEmpty(query))
                return db.Execute(query);
            else return -1;
        }
        
    }
}
