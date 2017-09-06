using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Analytics.GTGATracker.GTGAData;
using Google.Analytics.GTGATracker.GTGADomain;
using Google.GData.Client;
using Google.GData.Analytics;
using System.Net;
using System.Xml;
using System.Data;

namespace Google.Analytics.GTGATracker.GTGApplication
{
    public interface IGAnalyticsService : IService

    {

        IAnalyticsProxy AnalyticsProxy { set; }



        /// <summary>
        /// Get the email credential of the account read from the app.config wich 
        /// we will use to authenticate our analytics service account where we will report data from.
        /// </summary>
        /// <returns>A string with the mail of the analytics service account chosen to track.</returns>
        string GetEmail();


        /// <summary>
        /// Get the password credential of the account read from the app.config wich we will 
        /// use to authenticate our analytics service account where we will report data from.
        /// </summary>
        /// <returns>A string with the password of the analytics service account chosen to track.</returns>
        string GetPassword();


        /// <summary>
        /// Takes the metrics read form the app.config
        /// </summary>
        /// <param name="metrics"></param>
        /// <returns>The metrics list used to build the queries.</returns>
        List<string> getMetrics(string metrics);


        /// <summary>
        /// Takles the dimensions read from the app.config.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns>The dimensions list used to build the query.</returns>
        List<string> getDimensions(string dimensions);



        /// <summary>
        /// Get all the idProfiles joined to an account. 
        /// </summary>
        /// <returns>A list of strings with the idProfiles </returns>
        List<String> getProfileIdList();




        /// <summary>
        /// Get all the webProperties joined to an account.
        /// </summary>
        /// <returns>A list of strings with the webProperties. </returns>
        List<String> getWebPropertiesList();




        /// <summary>
        /// This method builds queries configured into the app.config and it sends them to Google Analytics.
        /// </summary>
        /// <param name="profile">Pass the object GtProfile created by reading the app.config section throught
        ///  the class section handler, we need to get the user login, password and some query parameters 
        ///  from this object.
        /// </param>
        /// <param name="details",GtQueryDetails>Pass the object GtQueryDetails created by reading the
        /// app.config section throught the class section handler, we needs to get the tableId, 
        /// and the tittle profile to do the query.
        /// </param>
        /// <result>Object DataFeed wich contains all the info returned by Google Analytics after querying.</result>
        /// <exception cref="ArgumentNullException&lt;Auth2gt;"/>
        /// <exception cref="InvalidCredentials"/>
        DataFeed GetStatistics(String profile, GtQueryDetails details);




        /// <summary>
        /// Get the metrics and values of these metrics returned by google analytics after querying.
        /// </summary>
        /// <param name="feed">Object DataFeed wich contains all the info returned by google analytics after querying.</param>
        /// <returns>List of metrics with their response values.</returns>
        List<Metric> getFeedMetrics(DataFeed feed);




        /// <summary>
        /// Get the dimensions and values of these dimensions returned by google analytics after querying.
        /// </summary>
        /// <param name="feed">Object DataFeed wich contains all the info returned by google analytics after querying.</param>
        /// <returns>List of dimesions with their response values.</returns>
        List<Dimension> getFeedDimensions(DataEntry entry);



        /// <summary>
        /// This class builds objects GtResult to be parsed as an xml file
        /// </summary>
        /// <param name="feed">Object DataFeed returned after queryig google.</param>
        /// <returns>An object GtResult wich contains the query results.</returns>
        GtResult getResult(DataFeed feed, GtQueryDetails details);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        void getAllResults(GtAccount account);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        void getAllSingleResults(GtAccount account);

    }
}
