using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Google.Analytics.GTGATracker.GTGApplication;
using Google.GData.Analytics;
using Google.Analytics.GTGATracker.GTGAData;
using Google.Analytics.GTGATracker.GTGATrackingForm;
using System.Reflection;
using Google.GData.Client;
using Google.Analytics;
using Microsoft.Practices.Unity;
using Google.Analytics.GTGATracker.GTGADomain;
using System.Net;
using System.Configuration;
using System.Xml;
using System.Runtime.Serialization;


namespace GTGATrackingForm
{
    public partial class TrackingForm : Form
    {

        private GAnalyticsService _AnalyticsService { get; set; }
        private Auth2Register _Auth { get; set; }
        private GtSettingsHandler _SettingsHandler { get; set; } 
        private List<GtQueryDetails> _QueryDetailsList { get; set; }
        private List<GtAccount> _AccountList { get; set; }
        private GtAccount _Account { get; set; }
        private DataFeed _DataFeed { get; set; }
        private GtResult _Result { get; set; }
        private GtSettingsHandler _Section { get; set; }
        private GtAccountAdapter _Adapter { get; set; }

         
       
       
        public TrackingForm()
        {

            InitializeComponent();

            _Auth = new Auth2Register();
            _SettingsHandler = new GtSettingsHandler();
            _Section = _SettingsHandler.GetConfig();
            _Adapter = new GtAccountAdapter();
            _AccountList = _Adapter.ConvertToGtAccountList(_Section.AccountsItems);
            //As DropDownList dataSource it's used a BidingSource with the object list we want displaying.
            cbAccounts.DataSource = new BindingSource(_AccountList, null);
            cbAccounts.DisplayMember = "Key";  
        }


        /// <summary>
        /// Handles the Click event of the btnRetrieveData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRetrieveData_Click(object sender, EventArgs e)
        {
            try 
            {
                string comboIndex = cbAccounts.SelectedIndex.ToString();
                var account = _Section.AccountsItems[Convert.ToInt32(comboIndex)];
                _Account = _Adapter.ConvertToGtAccount(account);
                _QueryDetailsList = _Account.Queries;
                //Create the Analytics Service. This will automatically call the previously registered authenticator. 
                _Auth.Auth();
                _AnalyticsService = new GAnalyticsService(_Auth, _Account);
                List<String> accounts = _AnalyticsService.getProfileIdList();
                string xmlChoice = ConfigurationManager.AppSettings["SingleXml"];
                Int32 chosen = Convert.ToInt32(xmlChoice);

                if (chosen == 1 )
                {
                    _AnalyticsService.getAllSingleResults(_Account);
                }       
                else
                {        
                    _AnalyticsService.getAllResults(_Account);
                }

            }
            catch (GDataRequestException ex)
            {
                  MessageBox.Show(ex.ResponseString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                   
            }
               
        }

        /// <summary>
        /// Handles the RowEnter event of the cbQueries control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void cbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            GtAccount accountDetails = (GtAccount)cbAccounts.SelectedItem;
            GtAccountAdapter _Adapter = new GtAccountAdapter();
            var section = _SettingsHandler.GetConfig();
            _AccountList = _Adapter.ConvertToGtAccountList(section.AccountsItems);
            if (accountDetails == null) 
            {
                throw new Exception("No account selected");
            }
        }

    }
}


        


