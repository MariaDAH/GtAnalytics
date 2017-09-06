using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Google.Analytics.GTGATracker.GTGAData
{
    [Serializable]
    [XmlRootAttribute("ListResults", Namespace = "Google.Analytics.GTGATracker.GTGAData", IsNullable = false)]
    public class GtListResults
    {
       
        private List<GtResult> _ResultList = new List<GtResult>();

        [XmlArray("ResultList"), XmlArrayItem("Result", typeof(GtResult))]
        public List<GtResult> ResultList
        {
            get { return _ResultList; }
            set { _ResultList = value; }
        }

    }
}
