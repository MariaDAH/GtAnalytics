using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Google.Analytics.GTGATracker.GTGAData
{


    /// <summary>
    /// This is simple class to maintain the query parameters.
    /// </summary>
    [Serializable()]
    public class GtAccount
    {

        #region Properties Region

        public string key { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<GtQueryDetails> Queries { get; set; }
       
        #endregion

        
        public GtAccount() { }


        public GtAccount(string email, string password) 
        {
            this.Email = email;
            this.Password = password;
        }


        public GtAccount(string email, string password, List<GtQueryDetails> queries) {
            this.Email = email;
            this.Password = password;
            this.Queries = queries;
        }


        public GtAccount(string key, string email, string password, List<GtQueryDetails> queries) 
        {
            this.key = key;
            this.Email = email;
            this.Password = password;
            this.Queries = queries;
        }


        //public override bool Equals(object obj)
        //{
        //    GtAccount target = (GtAccount)obj;

        //    return (this.Email == target.Email)
        //          && (this.Password == target.Password)
        //          && (this.Queries == target.Queries)
        //          && (this.key == target.key);
        //}



        // The GetHashCode method is used in hashing algorithms and data 
        // structures such as a hash table. In order to ensure that it works 
        // properly, we suppose that the Key does not change.        
        public override int GetHashCode()
        {
            return this.key.GetHashCode();
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
            String strGtAccount = "[ Email = " + Email + " | " +
                "Password = " + Password + " | " +
                "Queries = " + Queries + " |" +
                "Key =" + key + " ] ";

            return strGtAccount;
        }


    }
}
