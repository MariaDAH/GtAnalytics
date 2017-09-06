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
    public class GtQueryDetails
    {

        #region Properties

        public string Name { get; set; }
        public string Tittle { get; set; }
        public string TableId { get; set; }
        public string Metrics { get; set; }
        public string Dimensions { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Period { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public int Max { get; set; }
        //public Filters filters { get; set; }
       
        #endregion

        #region Constructor

        public GtQueryDetails() { }

        public GtQueryDetails(string name, string tableId, string metrics, string dimensions, 
            DateTime from, DateTime to,int offset, string sort, int max)
        {
            this.Name = name;
            this.TableId = tableId;
            this.Metrics = metrics;
            this.Dimensions = dimensions;
            this.From = from;
            this.To = to;
            this.Offset = offset;
            this.Sort = sort;
            this.Max = max;
        }


        #endregion


        //public override bool Equals(object obj)
        //{
        //    GtQueryDetails target = (GtQueryDetails)obj;

        //    return (this.Name == target.Name)
        //          && (this.TableId == target.TableId)
        //          && (this.Metrics == target.Metrics)
        //          &&(this.Dimensions == target.Dimensions)
        //          &&(this.From == target.To)
        //          &&(this.To == target.To)
        //          &&(this.Offset == target.Offset)
        //          &&(this.Sort == target.Sort)
        //          &&(this.Max == target.Max);
                    
        //}



        // The GetHashCode method is used in hashing algorithms and data 
        // structures such as a hash table. In order to ensure that it works 
        // properly, we suppose that the Name does not change.        
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
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
            String strGtQueryDetails = "[ Name = " + Name + " | " +
                "Tittle = " + Tittle + " | " +
                "TableId = " + TableId + " | " +
                "Metrics = " + Metrics + " | " +
                "Dimensions = " + Dimensions +" | " +
                "From = " + From  +" | " +
                "To = " + To  + " | " +
                "Offset = " + Offset + " | " +
                "Sort = " + Sort + " | " +
                "Max = " + Max + "] " ;

            return strGtQueryDetails;
        }

         
    }
}
