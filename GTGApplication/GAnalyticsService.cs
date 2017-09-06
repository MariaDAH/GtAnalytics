using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Analytics.GTGATracker.GTGAData;
using Google.GData.Analytics;
using Google.GData.Client;
using Google.Analytics.GTGATracker.GTGADomain;
using Microsoft.Practices.Unity;
using System.Configuration;
using System.Net;
using System.Data;
using System.Xml;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Google.GData.Extensions;
using System.Collections;
using log4net;





namespace Google.Analytics.GTGATracker.GTGApplication
{
    /// <summary>
    /// Class wich handles the login, report requests and parsing.
    /// To read Google Analytics you have to create an instance of this class 
    /// GAnalyticsService, extended from the Google Analytics API class AnalyticsService 
    /// and set the username (email) and password.This object will carry out all the necesary 
    /// steps to get the data from Google.It also implements the interface IGAnalyticsService 
    /// to maintain the minimum coupling between layers.
    /// </summary>
    [Serializable()]
    public class GAnalyticsService
        : AnalyticsService, IGAnalyticsService
    {
        #region Properties

        [Dependency]
        public IAnalyticsProxy AnalyticsProxy { private get; set; }


        public  GtAccount _Account { get; set; }
        private Auth2Register Auth { get; set; }
        private DataFeed RetrievedData { get; set; }
        private GtResult Result { get; set; }

        public string errores = string.Empty;
        private static readonly ILog log = LogManager.GetLogger("GTGALog");   
        
      
        #endregion

        #region Constructor


        /// <summary>
        /// Initialize and authenticate. This constructor create an object of GAnalyticsService,
        /// extended from AnalyticsService class and it is registered in Google Auth2 Service.
        /// Attemps to authorize using the client login authorization mechanism to get the possibity of
        /// requesting data into a Google Analytics.
        /// </summary> 
        /// <param name="auth"><code>Auth2Register</code>This object autheticates the application in the Google Auth2 Service</param>
        /// <exception cref="ArgumentNullException&lt;Auth&gt;"/>
        /// <exception cref="InvalidCredentials"/>     
        public GAnalyticsService(Auth2Register auth)
            : base("GTGATracker")
        {
            this.Auth = auth;
            log4net.Config.XmlConfigurator.Configure();
        }


        /// <summary>
        /// Initialize and authenticate. This constructor create an object of GAnalyticsService,
        /// extended from AnalyticsService class and it tries to authentificate an user with 
        /// the given credentials for a specific account.
        /// Attemps to authorize using the client login authorization mechanism to get the possibity of
        /// requesting data into a Google Analytics account and sets the account wich we like tracking.
        /// </summary>
        /// <param name="account", GtAccount>Pass the object GtAcccount created by reading the configuration section 
        /// handler (GtSettingsHandler object), we need to get the user login, password and some query parameters from this object
        /// we will use later to do the specific requests.</param>
        /// <param name="auth"><code>Auth2Register</code>This object autheticates the application in the Google Auth2 Service</param>
        /// <exception cref="ArgumentNullException&lt;Auth&gt;"/>
        /// <exception cref="InvalidCredentials"/>  
        public GAnalyticsService(Auth2Register auth, GtAccount account) 
            :base("GTGATracker")
        {
            if (account == null) 
            {
                throw new ArgumentNullException("account", "Account not set");
            }
            if (string.IsNullOrEmpty(account.Email)) 
            {
                throw new ArgumentNullException("email", "Email not set");
            }
            if (string.IsNullOrEmpty(account.Password)) 
            {
                throw new ArgumentNullException("password","Password not set");
            }
            this.Auth = auth;
            this._Account = account;
        }

        #endregion

        #region  IGAnalyticsService Methods


        
        public string GetEmail()
        {
            return this._Account.Email;
            
        }

       

        public string GetPassword()
        {
            return this._Account.Password;
        }



        public List<String> getProfileIdList() 
        {
            try
            {
                List<String> profilesIdList = new List<String>();
                GDataGAuthRequestFactory requestFactory = new GDataGAuthRequestFactory("analytics", "GTGATracker");
                requestFactory.AccountType = "GOOGLE";
                AnalyticsProxy = new AnalyticsProxy(requestFactory);
                this.RequestFactory = requestFactory;
                this.setUserCredentials(GetEmail(), GetPassword());
                string queri = ConfigurationManager.AppSettings["FeedAccountsUri"] + ConfigurationManager.AppSettings["API_Key"];
                string webProfileId = "";
                string webPropertie = "";
                string profileIdUrl = "";
                List<String> webProfilesList = new List<String>();
                List<String> idList = new List<String>();
                DataQuery query = new DataQuery(queri);
                DataFeed feedWebProperty = this.Query(query);
                foreach (AtomEntry entry in feedWebProperty.Entries)
                {
                    webProfileId = entry.Links[1].HRef.Content;
                    DataQuery propertyQuery = new DataQuery(webProfileId);
                    DataFeed feedProfile = this.Query(propertyQuery);
                    foreach (DataEntry entri in feedProfile.Entries)
                    {
                        webPropertie = entri.Links[2].HRef.Content;
                        DataQuery profileIdQuery = new DataQuery(webPropertie);
                        DataFeed feedProfileId = this.Query(profileIdQuery);
                        foreach (DataEntry entrie in feedProfileId.Entries)
                        {
                            idList.Add(entrie.Links[0].HRef.Content);
                            profileIdUrl = entrie.Links[0].HRef.Content;

                        }

                    }
                }

                return idList;
            }
            catch(GDataRequestException e)
            {
                throw e;
            }

        }

        

        public List<String> getWebPropertiesList()
        {
            List<String> webPropertieList = new List<String>();
            GDataGAuthRequestFactory requestFactory = new GDataGAuthRequestFactory("analytics", "GTGATracker");
            requestFactory.AccountType = "GOOGLE";
            IWebProxy myProxy = new WebProxy(ConfigurationManager.AppSettings["ProxyUrl"]);
            NetworkCredential credentials = new NetworkCredential(ConfigurationManager.AppSettings["AnalyticsProxyUsername"],
                ConfigurationManager.AppSettings["AnalyticsProxyPassword"]);
            myProxy.Credentials = credentials;
            requestFactory.Proxy = myProxy;
            this.setUserCredentials(GetEmail(), GetPassword());
            String queri = ConfigurationManager.AppSettings["FeedAccountsUri"]
                + ConfigurationManager.AppSettings["API_Key"];
            DataQuery query = new DataQuery(queri);
            DataFeed feedWebProperty = this.Query(query);
            foreach (AtomEntry entry in feedWebProperty.Entries)
            {
                webPropertieList.Add(entry.Links[1].HRef.Content);
            }
            return webPropertieList;
        }



        public List<string> getMetrics(string metrics)
        {

            List<string> metricList = new List<string>();

            // Input string contain separators.
            char[] delimiter1 = new char[] { ',' };

            // ... Use StringSplitOptions.None.
            string[] array1 = metrics.Split(delimiter1,
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string entry in array1)
            {
                metricList.Add(entry);
            }
            return metricList;
        }



        public List<string> getDimensions(string dimensions)
        {
            List<string> dimensionList = new List<string>();

            // Input string contain separators.
            char[] delimiter1 = new char[] { ',' };

            // ... Use StringSplitOptions.None.
            string[] array1 = dimensions.Split(delimiter1,
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string entry in array1)
            {
                dimensionList.Add(entry);
            }
            return dimensionList;
        }



        public DataFeed GetStatistics(String profile, GtQueryDetails details)
        {
            try
            {
                RequestSettings settings = new RequestSettings("GTGATracker");
                AnalyticsRequest request = new AnalyticsRequest(settings);
                request.Service = this;
                AnalyticsProxy = new AnalyticsProxy(request);
                setUserCredentials(GetEmail(), GetPassword());
                DataQuery query = new DataQuery(ConfigurationManager.AppSettings["FeedDataUri"]);
                query.Ids = "ga:"+ profile;
                string dimensions = details.Dimensions;
                List<string> dimensionsList = new List<string>();
                dimensionsList = getDimensions(dimensions);
                if (dimensions != "")
                {
                    dimensions = "ga:" + dimensionsList.First();
                }
                else
                {
                    dimensions = "";
                }

                for (int i = 1; i < dimensionsList.Count; i++)
                {
                    dimensions += ",ga:" + dimensionsList[i];
                }
                query.Dimensions = dimensions;
                string metrics = details.Metrics;
                List<string> metricList = new List<string>();
                metricList = getMetrics(metrics);
                if (metrics != "")
                {
                    metrics = "ga:" + metricList.First();
                }
                else
                {
                    metrics = "";
                }
                metrics = "ga:" + metricList.First();
                for (int i = 1; i < metricList.Count; i++)
                {
                    metrics += ",ga:" + metricList[i];
                }
                query.Metrics = metrics;
                query.GAStartDate = details.From.ToString("yyyy-MM-dd");
                log.Info("StartDate: " + details.From);
                query.GAEndDate = details.To.ToString("yyyy-MM-dd");
                log.Info("EndDate: " + details.To);
                query.Sort = details.Sort;
                query.NumberToRetrieve = details.Max;
            
                
                this.RetrievedData = Query(query);
                return RetrievedData;

            }
            catch (GDataRequestException ex)
            {
                throw ex;         
            }

        }

   
        public List<Metric> getFeedMetrics(DataFeed feed) 
        {
            List<Metric> feedMetrics = feed.Aggregates.Metrics;
            return feedMetrics;      

        }

      
        public List<Dimension> getFeedDimensions(DataEntry entry) 
        {
            List<Dimension> feedDimensions = new List<Dimension>();
            if (entry.Dimensions != null)
            {
                feedDimensions = entry.Dimensions;
            }
            else
            {
                feedDimensions = null;
            }

            return feedDimensions;
            
        }


        public GtResult getResult(DataFeed feed, GtQueryDetails details)
        {
            string dimensionPrefix;
            string dimensionSufix;

            try
            {
                IList<AtomEntry> feedCollection = feed.Entries.ToList<AtomEntry>();
                GtResult result = new GtResult();
                //Uncomment this line to display also the name of the profileId requested.
                //result.AccountName = feed.DataSource.TableName;
                result.AccountTittle = details.Tittle;
                result.Metrics = new List<MetricsDTO>();
               

                foreach (DataEntry entri in feedCollection)
                {
                    if (entri.Dimensions.Count != 0)
                    {

                        foreach (Metric metric in entri.Metrics)
                        {
                            List<String> dimensionValues = new List<String>();
                            for (int i = 0; i < entri.Dimensions.Count; i++)
                            {
                                dimensionPrefix = "";
                                dimensionSufix = "";
                               

                                if (i == 0) 
                                {
                                    dimensionPrefix = ConfigurationManager.AppSettings["dimensionPrefix"];
                                }
                                if (i == entri.Dimensions.Count - 1)
                                {
                                    dimensionSufix = ConfigurationManager.AppSettings["dimensionSufix"];
                                }
                               
                                    dimensionValues.Add(dimensionPrefix + entri.Dimensions[i].Value + dimensionSufix);
                                
                                
                            }
                            MetricsDTO metricDTO = new MetricsDTO();
                            metricDTO.DimensionsValues = dimensionValues;
                            metricDTO.MetricValue = Math.Round(metric.FloatValue, 2);
                            metricDTO.MetricName = metric.Name.Replace("ga:", "") + ConfigurationManager.AppSettings["metricSufix"];
                            result.Metrics.Add(metricDTO);
                        }
                    }
                    else
                    {
                        foreach (Metric metric in entri.Metrics)
                        {
                            MetricsDTO metricDTO = new MetricsDTO();
                            metricDTO.MetricValue = Math.Round(metric.FloatValue, 2);
                            metricDTO.MetricName = metric.Name.Replace("ga:", "");        
                            result.Metrics.Add(metricDTO);
                        }

                    }

                }
                return result;
            }
            catch(GDataRequestException ex)
            {
                throw ex;
            }
        }


        public void getAllResults(GtAccount account) 
        {
            
                
                List<GtQueryDetails> detailsList = new List<GtQueryDetails>();
                detailsList = account.Queries;
                GtResultXmlSerializer serialize = new GtResultXmlSerializer();
                String serializado = "";
                List<GtResult> listResults = new List<GtResult>();
                foreach (GtQueryDetails details in detailsList)
                {
                    String id = details.TableId.ToString();
                    log.Info("Requesting:"  + details.Tittle);
                    RetrievedData = GetStatistics(id, details);
                    Result = getResult(RetrievedData,details);
                    listResults.Add(Result);
                }

                if (!Directory.Exists(@ConfigurationManager.AppSettings["ResultPath"]))
                    Directory.CreateDirectory(@ConfigurationManager.AppSettings["ResultPath"]);

                serializado = serialize.SerializeUtf8(listResults);
                StreamWriter file = new StreamWriter(@ConfigurationManager.AppSettings["ResultPath"] + "result.xml");
                file.Write(serializado);
                file.Close();
        }


        public void getAllSingleResults(GtAccount account)
        {
            List<GtQueryDetails> detailsList = new List<GtQueryDetails>();
            detailsList = account.Queries;
            GtResultXmlSerializer serialize = new GtResultXmlSerializer();
            String serializado = "";
            string path =@ConfigurationManager.AppSettings["ResultPath"];
            string filePath;

            if (!Directory.Exists(@ConfigurationManager.AppSettings["ResultPath"]))
                Directory.CreateDirectory(@ConfigurationManager.AppSettings["ResultPath"]);

            for (int i = 0; i < detailsList.Count(); i++)
            {
                String id = detailsList[i].TableId.ToString();
                log.Info("Requesting:" + detailsList[i].Tittle);
                RetrievedData = GetStatistics(id, detailsList[i]);
                Result = getResult(RetrievedData,detailsList[i]);
                serializado = serialize.SerializeUtf8(Result);
                filePath = "result_" + detailsList[i].Name +  ".xml";
                StreamWriter file = new StreamWriter(path + filePath);
                file.Write(serializado);
                file.Close();
            }

        }


        #endregion

    }
}
