using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net;
using log4net;

namespace GTGAWinService
{
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger("GTGALog");

        /// <summary>
        /// Main entry application point
        /// </summary>
        static void Main()
        {            
            ServiceBase[] ServicesToRun;

            // To activate EXPECT100CONTINUE option for the proxy.
            ServicePointManager.Expect100Continue = false;

            try
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Info("Creating service...");

                ServicesToRun = new ServiceBase[] { new GTGAWinService() };

                log.Info("Executing on Service Control Manager... ");
          
                ServiceBase.Run(ServicesToRun);

            }
            catch (Exception ex) 
            {
                log.Error(ex.Message);
            }          
        }
    }
}
