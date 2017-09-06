using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace GTGaLog
{
    public class LogWriter
    {
        public readonly ILog log = LogManager.GetLogger("GtGALog");
        
            private static LogWriter instance = null;
            protected LogWriter()
            {
                // Exists only to defeat instantiation.
            }

            public static LogWriter getInstance()
            {
                if (instance == null)
                {
                    instance = new LogWriter();
                    log4net.Config.XmlConfigurator.Configure();
                }
                return instance;
            }

            public void Info(string message)
            {
                log.Info(message);
            }
    }
}


