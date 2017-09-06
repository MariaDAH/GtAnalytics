using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Google.Analytics.GTGATracker.GTGAData
{

    /// <summary>
    /// Read the information in the respective app.config section.
    /// </summary>
    public class GtSettingsHandler: ConfigurationSection
    {
        public GtSettingsHandler() { }

        public GtSettingsHandler GetConfig() 
        {
            GtSettingsHandler section;
            try
            {
                 section = (GtSettingsHandler)ConfigurationManager.GetSection("GtSettingsHandler");
            }
            catch (Exception e)
            {
                throw new Exception("Problem reading account from app.config" + e);
            }
            return section;
  
        }

        /// <summary>
        /// AccountsCollection
        /// </summary>
        [ConfigurationProperty("Accounts", IsRequired=true)]
        public AccountsCollection AccountsItems
        {
            get { return ((AccountsCollection)(base["Accounts"] ));}
            set { base["Accounts"] = value; }
        }

       

        /// <summary>
        /// Configure accounts collection
        /// </summary>
        [ConfigurationCollection(typeof(AccountElement), AddItemName = "Account")]
        public class AccountsCollection : ConfigurationElementCollection
        {
            public AccountsCollection() { }


            public AccountElement this[object accountKey]
            {
                get { return (AccountElement)BaseGet(accountKey); }
            }

            public void Add(AccountElement account)
            {
                base.BaseAdd(account);
            }

            public override ConfigurationElementCollectionType CollectionType
            {
                get { return ConfigurationElementCollectionType.BasicMap; }
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new AccountElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return((AccountElement)element).Key;
            }

            protected override string ElementName
            {
                get { return "Account"; }
            }

            public AccountElement this[int idx]
            {
                get
                {
                    return (AccountElement)BaseGet(idx);
                }
            }

        }

        /// <summary>
        /// Get the account element
        /// </summary>                
        public class AccountElement : ConfigurationElement
        {
            public AccountElement() { }


            public AccountElement(string key, string email, string pwd, QueryDetailsCollection queries)
            {
                Key = key;
                Email = email;
                Password = pwd;
                QueriesList = queries;
            }


            [ConfigurationProperty("key", IsKey = true, DefaultValue = "", IsRequired = true)]
            public string Key
            {
                get { return (string)base["key"]; }
                set { base["key"] = value; }
            }


            [ConfigurationProperty("email", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string Email
            {
                get { return (string)base["email"]; }
                set { base["email"] = value; }
            }


            [ConfigurationProperty("password", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string Password
            {
                get { return (string)base["password"]; }
                set { base["password"] = value; }
            }

            [ConfigurationProperty("queries", IsKey = false)]
            public QueryDetailsCollection QueriesList
            {
                get { return (QueryDetailsCollection)base["queries"]; }
                set { base["queries"] = value; }
            }

        }



        /// <summary>
        /// Configure the queries details element
        /// </summary>
        [ConfigurationCollection(typeof(QueryDetailsCollection),AddItemName="query")] 
        public class QueryDetailsCollection : ConfigurationElementCollection
        {
            public QueryDetailsCollection() { }


            public QueryDetailsElement this[object accountKey]
            {
                get { return (QueryDetailsElement)BaseGet(accountKey); }
            }

            public void Add(QueryDetailsElement queryDetails)
            {
                base.BaseAdd(queryDetails);
            }

            public override ConfigurationElementCollectionType CollectionType
            {
                get { return ConfigurationElementCollectionType.BasicMap; }
            }


            protected override ConfigurationElement CreateNewElement()
            {
                return new QueryDetailsElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((QueryDetailsElement)element).Name;
            }


            protected override string ElementName
            {
                get { return "query"; }
            }

            public QueryDetailsElement this[int idx]
            {
                get
                {
                    return (QueryDetailsElement)BaseGet(idx);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class QueryDetailsElement :ConfigurationElement
        {

            //Falta el offset.

            public QueryDetailsElement() {}


            public QueryDetailsElement(string name, string tittle, string tableId, string from, string to, string offset, 
                  string sort, string max, string metrics, string dimensions) 
            {
                Name = name;
                Tittle = tittle;
                TableId = tableId;
                From = from;
                To = to;
                Offset = offset;
                Sort = sort; 
                Max = max;
                Metrics = metrics;
                Dimensions = dimensions;
            }

            [ConfigurationProperty("name", IsKey = true, DefaultValue = "", IsRequired = true)]
            public string Name
            {
                get { return (string)base["name"]; }
                set { base["name"] = value; }
            }


            [ConfigurationProperty("tittle", IsKey = true, DefaultValue = "", IsRequired = true)]
            public string Tittle
            {
                get { return (string)base["tittle"]; }
                set { base["tittle"] = value; }
            }

            [ConfigurationProperty("tableId", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string TableId
            {
                get { return (string)base["tableId"]; }
                set { base["tableId"] = value; }
            }


            [ConfigurationProperty("startDate", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string From
            {
                get { return (string)base["startDate"]; }
                set { base["startDate"] = value; }
            }


            [ConfigurationProperty("endDate", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string To
            {
                get { return (string)base["endDate"]; }
                set { base["endDate"] = value; }
            }



            [ConfigurationProperty("offset", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string Offset
            {
                get { return (string)base["offset"]; }
                set { base["offset"] = value; }
            }


            [ConfigurationProperty("sort", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string Sort
            {
                get { return (string)base["sort"]; }
                set { base["sort"] = value; }
            }

            [ConfigurationProperty("max", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string Max
            {
                get { return (string)base["max"]; }
                set { base["max"] = value; }
            }


            [ConfigurationProperty("metrics", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string Metrics
            {
                get { return (string)base["metrics"]; }
                set { base["metrics"] = value; }
            }


            [ConfigurationProperty("dimensions", IsKey = false, DefaultValue = "", IsRequired = true)]
            public string Dimensions
            {
                get { return (string)base["dimensions"]; }
                set { base["dimensions"] = value; }
            }
        }

    }
}
