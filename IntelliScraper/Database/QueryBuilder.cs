using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Database
{
    public class QueryBuilder
    {
        public string existQuery(List<string> existByAttributes, string table, List<KeyValuePair<string, object>> record)
        {
            string query = string.Format("select * from {0} ", table);
            string condition = " where ";
            int i = 0;
            foreach (string attr in existByAttributes)
            {
                string val = sqlClear((string)findValueFromRecordByColName(record, attr));              
                if(i == (existByAttributes.Count -1))
                    condition += string.Format(" {0}='{1}' ", attr, val);
                else condition += string.Format(" {0}='{1}' and", attr, val);
                i++;
            }
            query += condition;
            return query;
        }
        
        public string deleteQuery(List<string> existByAttributes, string table, List<KeyValuePair<string, object>> record)
        {         
            string query = string.Format("DELETE FROM {0} ", table);
            string condition = " where ";
            int i = 0;
            foreach (string attr in existByAttributes)
            {
                string val = sqlClear((string)findValueFromRecordByColName(record, attr));
                if (i == (existByAttributes.Count - 1))
                    condition += string.Format(" {0}='{1}' ", attr, val);
                else condition += string.Format(" {0}='{1}' and", attr, val);
                i++;
            }
            query += condition;
            return query;
        }

        public string insertQuery(List<string> existByAttributes, string table, List<KeyValuePair<string, object>> record)
        {
            string query = string.Format("insert into {0} ", table);
            string vals = "  VALUES ( ";
            string cols = " ( ";

            foreach (string attr in existByAttributes)
                cols += string.Format("{0},", attr);
            
            if (cols.EndsWith(","))
                cols = cols.Substring(0, cols.Length - 1);
            cols += ") ";

            foreach (string attr in existByAttributes)
            {
                string val = sqlClear((string)findValueFromRecordByColName(record, attr));
                vals += string.Format("'{0}',", val);
            }
            if (vals.EndsWith(","))
                vals = vals.Substring(0,vals.Length - 1);
            query += cols + vals + ")";
            return query;
        }

        public string updateQuery(List<string> existByAttributes, List<string> columns, string table, List<KeyValuePair<string, object>> record)
        {
            string query = string.Format("update {0} SET ", table);
            string vals = " ";

            int i = 0;
            foreach (string attr in columns)
            {
                string val = (string)findValueFromRecordByColName(record, attr);
                if (i == (columns.Count - 1))
                    vals += string.Format("{0}='{1}' ", attr, val);
                else vals += string.Format("{0}='{1}', ", attr, val);
                i++;
            }

            string condition = " where ";
            i = 0;
            foreach (string attr in existByAttributes)
            {
                string val = sqlClear((string)findValueFromRecordByColName(record, attr));
                if (i == (existByAttributes.Count - 1))
                    condition += string.Format(" {0}='{1}' ", attr, val);
                else condition += string.Format(" {0}='{1}' and", attr, val);
                i++;
            }

            query += vals + condition;

  
            return query;
        }

        public string sqlClear(string val){
            val = val.Replace("'", "''");
            return val;
        }

        public KeyValuePair<string, object> findValuePairFromRecordByColName(List<KeyValuePair<string, object>> record, string name)
        {
            var data = from x in record where x.Key == name select x;
            foreach (var d in data)
                return d;
            return new KeyValuePair<string, object>(); ;
        }

       
        public object findValueFromRecordByColName(List<KeyValuePair<string, object>> record, string name)
        {
            var data = from x in record where x.Key == name select x;
            foreach (var d in data)
                return d.Value;
            return null;
        }
    }
}
