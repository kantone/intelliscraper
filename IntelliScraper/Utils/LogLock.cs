using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IntelliScraper
{
    public class LogLock : log4net.Appender.FileAppender.MinimalLock
    {
    
        public override void ReleaseLock()
        {
            base.ReleaseLock();

            var logFile = new FileInfo(CurrentAppender.File);
            if (logFile.Exists && logFile.Length <= 0)
            {
                logFile.Delete();
            }
        }
    }
}
