using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Xml.Serialization;

namespace Google.Analytics.GTGATracker.GTGAData
{

    // Set this 'GtResult' class as the root node
    // of any XML file its serialized to.
    [Serializable]
    [XmlRootAttribute("GtResult", Namespace = "Google.Analytics.GTGATracker.GTGAData", IsNullable = false)]
	public class GtResult
	{
        #region Properties


        private string _AccountName;

        [XmlElement(DataType = "string")]
        public string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

        private string _AccountTittle;

        [XmlElement(DataType = "string")]
        public string AccountTittle
        {
            get { return _AccountTittle; }
            set { _AccountTittle = value; }
        }


       
       private List<MetricsDTO> _Metrics = new List<MetricsDTO>();


       // Set this 'List<MetricsDTO>' field
       // to be an attribute of the root node.
       // Serializes an ArrayList as a "Metrics" array
       // of XML elements of type string named "MetricsDTO".
       [XmlArray("Metrics"), XmlArrayItem("Metric", typeof(MetricsDTO))] 
       public List<MetricsDTO> Metrics
        {
            get { return _Metrics; }
            set { _Metrics = value; }
        }

        #endregion


        /// <summary>
        /// Default constructor for this class 
        /// required for serialization.
        /// </summary>
        #region Constructor

        public GtResult() {}

       
        #endregion

        #region Methods



        #endregion



    }
}
