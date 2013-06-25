using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace IntelliScraper
{
    public class CacheManager
    {
        public string storeDir { get; set; }
        public string projectDir { get; set; }
        public CacheManager(string projectName)
        {
            storeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            storeDir += "\\cache";
            if (!System.IO.Directory.Exists(storeDir))
                System.IO.Directory.CreateDirectory(storeDir);

            projectDir += storeDir + "\\" + projectName;
            if (!System.IO.Directory.Exists(projectName))
                System.IO.Directory.CreateDirectory(projectName);
 
        }

        public void addToCache(string key,object value)
        {
            if (value != null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, value);
                byte[]  bytes = ms.ToArray();


                string fname = this.projectDir + "\\" + key;
                System.IO.FileStream _FileStream = new System.IO.FileStream(fname, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(bytes, 0, bytes.Length);

                // close file stream
                _FileStream.Close();
            }

        }

        public object getFromCache(string key, object value)
        {
            string fname = this.projectDir + "\\" + key;
            System.IO.FileStream fsSource = new System.IO.FileStream(fname, System.IO.FileMode.Create, System.IO.FileAccess.Read);
            byte[] bytes = new byte[fsSource.Length];
            int numBytesToRead = (int)fsSource.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
            numBytesToRead = bytes.Length;
            return bytes.Cast<object>();
        }
    }
}
