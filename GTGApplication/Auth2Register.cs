using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Util;
using DotNetOpenAuth.OAuth2;
using System.Net;
using System.IO;
using System.Configuration;
using Google.GData.Analytics;
using Google.Analytics.GTGATracker.GTGADomain;
using Microsoft.Practices.Unity;
using DotNetOpenAuth.Messaging;
using Google.GData.Client;
using System.Web;
using System.Diagnostics;


namespace Google.Analytics.GTGATracker.GTGApplication
{
    /// <summary>
    /// This class implements the authentication and authorization for Google API Access with google-gdata(.NET library for the Google Data API)
    /// It's strongly recommended use this kind of authorization to access third party API's since a consumer application
    /// as GTGATRacker. 
    /// Before creating service is necesary to add a delegate for the authentication so  this class register the authenticator.
    /// </summary>
    public class Auth2Register
    {

        #region Properties
        
        //Provider The Oauth2 client used by native applications.
        private NativeApplicationClient _Provider { get; set; }

        private Uri _AuthUri { get; set; }

        //Client_Id obtained during application registration on google console.
        private string ClientId = ConfigurationManager.AppSettings["ClientId"];
        
        //Client_Secret obtained during application registration on google console.
        private string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
       
        //Indicates the Google API access your application is requesting.
        //The values passed in this parameter inform the consent page shown to the user. 
        //Values are set of permissions the application request 
        public enum Scopes
        {
            [Google.Apis.Util.StringValueAttribute("https://www.googleapis.com/auth/analytics.readonly")]
            AnalyticsReadonly,
            [Google.Apis.Util.StringValueAttribute("https://www.googleapis.com/o/oauth2/auth/analytics")]
            Analytics,
        }

        //Determines the authorization code returned by Google Authorization Server.
        private static string ResponseString { get; set; }

        #endregion

        #region Constructor
        
        public Auth2Register() {}   
        
        #endregion


        /// <summary>
        /// This method registers this application in Google Authorization Server according Open Authorization 
        /// Protocol to get the API Access of Google Analytics.
        /// </summary>
        public void RegisterAuthenticator()
        {
        }


        public void Auth()
        {

            //Instances service provider class, for intance, Google Authorization Server.
            _Provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            //Set the consumer Key and consumer Secret given by Google Auth Server to allow consumer registering in.
            _Provider.ClientIdentifier = ClientId;
            _Provider.ClientSecret = ClientSecret;
            //IAuthorizationState state = new AuthorizationState(new[] { Auth2Register.Scopes.AnalyticsReadonly.GetStringValue() });
            //state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            //Uri authUri = _Provider.RequestUserAuthorization(state);
            //Request authorization from the user and get the code
            //string authCode = MakeRequest(authUri);
            //Retrieve the access token by using the authorization code:
            //IAuthorizationState auth = _Provider.ProcessUserAuthorization(authCode, state);  
            ////Creates a new authenticator as a function of a totenProvider and the response_type 
            //var authProvider = new OAuth2Authenticator<NativeApplicationClient>(_Provider, _Provider.ProcessUserAuthorization);
            var auth = new OAuth2Authenticator<NativeApplicationClient>(_Provider, GetAuthorization);

        }
        private static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            // Get the auth URL:
            IAuthorizationState state = new AuthorizationState(new[] { Auth2Register.Scopes.AnalyticsReadonly.GetStringValue() });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            Uri authUri = arg.RequestUserAuthorization(state);

            // Request authorization from the user (by opening a browser window):
            Process.Start(authUri.ToString());
            Console.Write("  Authorization Code: ");
            string authCode = Console.ReadLine();
            Console.WriteLine();

            // Retrieve the access token by using the authorization code:
            return arg.ProcessUserAuthorization(authCode, state);
        }

        ///// <summary>
        ///// Authorization Method.
        ///// </summary>
        ///// <param name="arg">Object provider<code>NativeApplicationSet</code> sets the application type 
        ///// to get adequate permisions.</param>
        ///// <returns>The authentication code</returns>
        //private  IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        //{
        //    // Get the auth URL:
        //    IAuthorizationState state = new AuthorizationState(new[] {Auth2Register.Scopes.AnalyticsReadonly.GetStringValue()});
        //    state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
        //    Uri authUri = arg.RequestUserAuthorization(state);
                     
        //    //Request authorization from the user and get the code
        //    string authCode = MakeRequest(authUri);

        //    //Retrieve the access token by using the authorization code:
        //    return arg.ProcessUserAuthorization(authCode, state);  
        //}

        ///// <summary>
        ///// Exchange code for token.
        ///// </summary>
        ///// <param name="arg">AuthUri<code>Uri</code> Sends the request to the Google Api endpoint.</param>
        ///// <returns>Token response</returns>
        //private string MakeRequest(Uri authUri)
        //{
        //    var normalizedEndpoint = authUri;
        //    var request = HttpWebRequest.Create(normalizedEndpoint);
        //    request.ContentLength = 0;
        //    request.Method = "POST";
        //    IWebProxy myProxy = new WebProxy();
        //    Uri uri = new Uri(ConfigurationManager.AppSettings["ProxyUrl"]);
        //    myProxy.GetProxy(uri);
        //    myProxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["AnalyticsProxyUsername"], ConfigurationManager.AppSettings["AnalyticsProxyPassword"]);
        //    request.Proxy = myProxy;

        //    try
        //    {
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        {
        //            // Get the response stream  
        //            StreamReader responseReader = new StreamReader(response.GetResponseStream());
        //            return ResponseString = responseReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException e)
        //    {
        //        throw new Exception("An error occurred while verifying the IDP response", e);
        //    }
        //}
   }
}
