using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Google.Analytics.GTGATracker.GTGAData
{
    [Serializable()]
    public class GtAccountDetails
    {

        #region Properties

        public string AccountName;

        public string WebPropertyName;

        public string IdProfile;

        #endregion

        #region Constructor

        public GtAccountDetails(string accountName, string webPropertyName, string idProfile) 
        {
            this.AccountName = accountName;
            this.WebPropertyName = webPropertyName;
            this.IdProfile = idProfile;
        }


        //public override bool Equals(object obj)
        //{
        //    GtAccountDetails target = (GtAccountDetails)obj;

        //    return (this.AccountName == target.AccountName)
        //          && (this.WebPropertyName == target.WebPropertyName)
        //          && (this.IdProfile == target.IdProfile);
        //}



        // The GetHashCode method is used in hashing algorithms and data 
        // structures such as a hash table. In order to ensure that it works 
        // properly, we suppose that the idProfile does not change.        
        public override int GetHashCode()
        {
            return this.IdProfile.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the 
        /// current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current 
        /// <see cref="T:System.Object"></see>.
        /// </returns>
        public override String ToString()
        {
            String strGtAccountDetails = "[ AccountName = " + AccountName + " | " +
                "WebPropertyName = " + WebPropertyName + " | " +
                "IdProfile = " + IdProfile + " ] ";

            return strGtAccountDetails;
        }



        #endregion

        




    }
}
