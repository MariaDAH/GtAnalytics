using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace Google.Analytics.GTGATracker.GTGAData
{
    [Serializable]
    public class MetricsDTO
    {
        
        public MetricsDTO() {}


        [XmlArray("DimensionValues"), XmlArrayItem("DimensionValue", typeof(List<String>))]
        private List<String> _DimensionsValues;

        public List<String> DimensionsValues
        {
            get { return _DimensionsValues; }
            set { _DimensionsValues = value; }
        }



        [XmlAttributeAttribute(DataType = "double")]
        private double _MetricValue;

        public double MetricValue
        {
            get { return _MetricValue; }
            set { _MetricValue = value; }
        }



        [XmlAttributeAttribute(DataType="string")]
        private string _MetricName;

        public string MetricName
        {
            get { return _MetricName; }
            set { _MetricName = value; }
        }

        


    }
}
