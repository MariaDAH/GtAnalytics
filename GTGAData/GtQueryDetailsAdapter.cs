using System;
using System.Collections.Generic;
using log4net;



namespace Google.Analytics.GTGATracker.GTGAData
{
  
   
    /// <summary>
    /// This class is used as an adapter to parse the objects built to read the app.config info in the GtSettingsHandler object
    /// as the objects used to work directly to make request to google analytics.
    /// </summary>
    public class GtQueryDetailsAdapter : GtQueryDetails
    {

        

        public GtQueryDetailsAdapter() {}

        /// <summary>
        /// Return the queryDetails used to build the request.
        /// </summary>
        /// <param name="queryDetails">The query details adapted from the class used to read the app.config</param>
        /// <returns>The queryDetails adapted from app.config.</returns>
        public GtQueryDetails ConvertToGtQueryDetails(GtSettingsHandler.QueryDetailsElement queryDetails) 
        {
            GtQueryDetails details = new GtQueryDetails();
            List<GtQueryDetails> detailsList = new List<GtQueryDetails>();

            details.Name = queryDetails.Name;
            details.Tittle = queryDetails.Tittle;
            details.TableId = queryDetails.TableId;
            details.From = Convert.ToDateTime(queryDetails.From);
            details.To = Convert.ToDateTime(queryDetails.To);
            details.Max = Convert.ToInt32(queryDetails.Max);
            details.Sort = (queryDetails.Sort);
            details.Offset = Convert.ToInt32(queryDetails.Offset);
            details.Metrics = queryDetails.Metrics;
            details.Dimensions = queryDetails.Dimensions;
            return details;
        }

        /// <summary>
        /// Returns the list of queries configured in the app.config.
        /// </summary>
        /// <param name="queryDetailsList">String list of queries</param>
        /// <returns>The list of queryDetails adapted from app.config.</returns>
        public List<GtQueryDetails> ConvertToGtQueryDetailsList(GtSettingsHandler.QueryDetailsCollection queryDetailsList)
        {
            
            GtQueryDetails details;
            List<GtQueryDetails> detailsList = new List<GtQueryDetails>();
            
            foreach(GtSettingsHandler.QueryDetailsElement queryDetails in queryDetailsList)
            {
                details = new GtQueryDetails();
                details.Name = queryDetails.Name;
                details.Tittle = queryDetails.Tittle;
                details.TableId = queryDetails.TableId;
                details.Offset = Convert.ToInt32(queryDetails.Offset);
                if (queryDetails.From == "")
                {
                    if (queryDetails.To == "")
                    {
                        details.To = DateTime.Now;
                    }
                    else
                    {
                        details.To = Convert.ToDateTime(queryDetails.To);
                    }
                    int hours = DateTime.Now.Hour;
                    int minutes = DateTime.Now.Minute;
                    int seconds = DateTime.Now.Second;
                    TimeSpan substract = new TimeSpan(details.Offset, 0, 0, 0);
                    DateTime dateTo = new DateTime();
                    DateTime date = new DateTime();
                    dateTo = Convert.ToDateTime(details.To);
                    date = dateTo.Subtract(substract);
                    details.From = date;
                    details.Max = Convert.ToInt32(queryDetails.Max);
                    details.Sort = (queryDetails.Sort);
                    details.Metrics = queryDetails.Metrics;
                    if (queryDetails.Dimensions != "")
                    {
                        details.Dimensions = queryDetails.Dimensions;
                    }
                    else
                    {
                        details.Dimensions = "";
                    }

                    detailsList.Add(details);
                }
                else
                {
                    details.From = Convert.ToDateTime(queryDetails.From);
                    try
                    {
                        if (queryDetails.To == "")
                        {
                            details.To = DateTime.Now;
                        }
                        else
                        {
                            details.To = Convert.ToDateTime(queryDetails.To);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    details.Max = Convert.ToInt32(queryDetails.Max);
                    details.Sort = (queryDetails.Sort);
                    details.Metrics = queryDetails.Metrics;
                    if (queryDetails.Dimensions != "")
                    {
                        details.Dimensions = queryDetails.Dimensions;
                    }
                    else
                    {
                        details.Dimensions = "";
                    }
                    detailsList.Add(details);

                }
            }

            return detailsList;
        }

    }
}
