using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Analytics.GTGATracker.GTGAData;
using Microsoft.Practices.Unity;
using System.Net;
using System.IO;
using Google.Analytics.GTGATracker.GTGADomain.Properties;
using Google.Apis.Analytics.v3;
using Google.GData.Client;
using Google.GData.Analytics;



namespace Google.Analytics.GTGATracker.GTGADomain
{
    [Serializable()]
    public class AnalyticsProxy: IAnalyticsProxy
    {
        #region Properties
        private string ProxyAddress;
        private string UserName;
        private string Password;
        private IWebProxy MyProxy;
        private Uri NewUri;
        #endregion
        
        

        #region Constructor


        public AnalyticsProxy(AnalyticsRequest request)
        {
            this.ProxyAddress = Settings.Default.ProxyUrl;

            if (ProxyAddress.Length == 0)
            {
                request.Proxy = MyProxy;
            }
            else
            {
                this.UserName = Settings.Default.AnalyticsProxyUsername;
                this.Password = Settings.Default.AnalyticsProxyPassword;
                this.NewUri = new Uri(ProxyAddress);
                MyProxy = new WebProxy();
                MyProxy.GetProxy(NewUri);
                MyProxy.Credentials = new NetworkCredential(UserName, Password);
                request.Proxy = MyProxy;

            }
        }



        public AnalyticsProxy(GDataGAuthRequestFactory request)
        {
            this.ProxyAddress = Settings.Default.ProxyUrl;

            if (ProxyAddress.Length == 0)
            {
                request.Proxy = MyProxy;
            }
            else
            {
                this.UserName = Settings.Default.AnalyticsProxyUsername;
                this.Password = Settings.Default.AnalyticsProxyPassword;
                this.NewUri = new Uri(ProxyAddress);
                MyProxy = new WebProxy();
                MyProxy.GetProxy(NewUri);
                MyProxy.Credentials = new NetworkCredential(UserName, Password);
                request.Proxy = MyProxy;

            }
        }

        #endregion



        #region IAnalyticsProxy Methods

        public string HttpRequest(string url, string query)
        {
            throw new NotImplementedException();
        }

        public ICredentials Credentials
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Uri GetProxy(Uri destination)
        {
            throw new NotImplementedException();
        }

        public bool IsBypassed(Uri host)
        {
            throw new NotImplementedException();
        }

        #endregion

        

    }
}
