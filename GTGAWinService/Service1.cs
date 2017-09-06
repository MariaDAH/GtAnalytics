using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Google.Analytics.GTGATracker.GTGApplication;
using Google.Analytics.GTGATracker.GTGAData;
using System.Configuration;
using log4net;
using Google.GData.Client;

namespace GTGAWinService
{
    public partial class GTGAWinService : ServiceBase
    {

        #region Properties

        private Timer _AutoUpdateTimer { get; set; }    // timer to launch process automatically. 
        private GAnalyticsService _AnalyticsService { get; set; }
        private Auth2Register _Auth { get; set; }
        private GtSettingsHandler _SettingsHandler { get; set; }
        private GtAccount _Account { get; set; }
        private List<GtQueryDetails> _QueryDetailsList { get; set; }
        private GtSettingsHandler _Section { get; set; }
        private GtAccountAdapter _Adapter { get; set; }
        private static readonly ILog log = LogManager.GetLogger("GTGALog");   

        #endregion


        public GTGAWinService()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnStart(string[] args)
        {
            
            try
            {
                
                log.Info("this is the first log message");   
                //Configure timer.
                _AutoUpdateTimer = new Timer();
                _AutoUpdateTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["WinServiceTimer"]);
                _AutoUpdateTimer.Enabled = true;
                //Hook up the elapsed event for the Timer.
                _AutoUpdateTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);

                // Read configuration data. 
                log.Info("OnStart: Reading configuration...");
                _SettingsHandler = new GtSettingsHandler();
                _Section = _SettingsHandler.GetConfig();
                log.Info("Configuration Read...");
                //Register on Google Authentication Server
                log.Info("OnStart: Registering on Google Authetication Server...");
                _Auth = new Auth2Register();
                //Create the Analytics Service. This will automatically call the previously registered authenticator. 
                _Auth.RegisterAuthenticator();
                log.Info("OnStart: Registered.");
                
           
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }                   
        }


        protected override void OnStop()
        {
            log.Info("onStop: stopping service");
            _AutoUpdateTimer.Stop();
            log.Info("onStop: stopped service");
        }



        #region EventHandlers

        // Specify what you want to happen when the Elapsed event is raised.
        void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //Set the analytics account 
                log.Info("Setting account...");
                _Adapter = new GtAccountAdapter();
                //Create the first account in the listAccounts. 
                _Account = _Adapter.ConvertToGtAccount(_Section.AccountsItems[0]);
                log.Info("Account set...");
                //Register Analytics Service
                log.Info("OnStart: Set down Analytics Service...");
                _AnalyticsService = new GAnalyticsService(_Auth, _Account);
                log.Info("OnStart: Analytics Service On.");
                string xmlChoice = ConfigurationManager.AppSettings["SingleXml"];
                Int32 chosen = Convert.ToInt32(xmlChoice);

                if (chosen == 1)
                {
                    _AnalyticsService.getAllSingleResults(_Account);
                }
                else
                {
                    _AnalyticsService.getAllResults(_Account);
                }
                //Writes in a xml file all the query responses.
                log.Info("Responses Retrieved..."); 
                log.Info("Displaying responses...");               
            }
            catch (GDataRequestException ex)
            {
                log.Error("Excepción: " + ex.InnerException);
            }
        }

        #endregion
    }
}
