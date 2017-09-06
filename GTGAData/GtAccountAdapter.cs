using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Google.Analytics.GTGATracker.GTGAData
{
    public class GtAccountAdapter : GtAccount
    {
        public GtAccountAdapter() {}
        
        #region Properties
        private GtQueryDetailsAdapter QueryAdapter;
        private GtAccount AccountAdapter;

        #endregion



        /// <summary>
        /// Convert the accountElement from the app.config to a GtAccount object.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>GtAccount adapted</returns>
        public GtAccount ConvertToGtAccount(GtSettingsHandler.AccountElement account)
        {
            QueryAdapter = new GtQueryDetailsAdapter();
            AccountAdapter = new GtAccount();
            AccountAdapter.Email = account.Email;
            AccountAdapter.Password = account.Password;
            AccountAdapter.Queries = QueryAdapter.ConvertToGtQueryDetailsList(account.QueriesList);
            return AccountAdapter;
        }


        /// <summary>
        /// Convert the accountElementList from the app.config to a GtAccountList object.
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns>GtAccountList adapted</returns>
        public List<GtAccount> ConvertToGtAccountList(GtSettingsHandler.AccountsCollection accounts)
        {
            GtAccount account;
            List<GtAccount> accountList = new List<GtAccount>();
            foreach(GtSettingsHandler.AccountElement accountElement in accounts)
            {
                account = new GtAccount();
                account.key = accountElement.Key;
                account.Email = accountElement.Email;
                account.Password = account.Password;
                accountList.Add(account);
            }
            return accountList;
        }

    }
}
