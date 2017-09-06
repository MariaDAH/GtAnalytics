using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Google.Analytics.GTGATracker.GTGAData;
using System.Runtime.Serialization;
using System.Xml;
using System.Web;

namespace Google.Analytics.GTGATracker.GTGApplication
{

    /// <summary>
    /// This class implements methods to convert a GtResultObject into a Xml format.
    /// </summary>
    public class GtResultXmlSerializer
    {
        public GtResultXmlSerializer() { }


        /// <summary>
        /// This method converts a GtResult object into a xml format.
        /// </summary>
        /// <param name="result"> Object to serialize.</param>
        /// <returns>An string with the object serialized.</returns>
        public string Serialize(GtResult result)
        {
            #region Preconditions

            if (result == null)
                throw new ArgumentNullException("result");

            #endregion

            string xml = null;

            // Create a new XmlSerializer instance with the type of the result class
            XmlSerializer serializer = new XmlSerializer(result.GetType());

            // Create a new streamWriter to write the serialized object to a file
            StringWriter stringWriter = new StringWriter();

            //Write the object serialized in the stringWriter file
            serializer.Serialize(stringWriter, result);

            xml = stringWriter.ToString();

            stringWriter.Close();

            return xml;
        }



        /// <summary>
        /// This method converts a GtResult object into a xml UTF-8 format.
        /// It was necessary converting to this format because it was 
        /// encountered an issue when trying to display an XML file on 
        /// the ticker.
        /// </summary>
        /// <param name="instance">Object to serialize</param>
        /// <returns>An string with the object serialized.</returns>
        public string SerializeUtf8(Object instance)
        {
            #region Preconditions

            if (instance == null)
                throw new ArgumentNullException("instance");

            #endregion


            XmlSerializer serializer = new XmlSerializer(instance.GetType());

            // Create a MemoryStream here, we are just working
            // exclusively in memory
            Stream stream = new MemoryStream();

            XmlWriter xtWriter = XmlWriter.Create(stream);

            serializer.Serialize(xtWriter, instance);

            xtWriter.Flush();

            // Go back to the beginning of the Stream to read its contents
            stream.Seek(0, SeekOrigin.Begin);

            // Read back the contents of the stream and supply the encoding

            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            string result = reader.ReadToEnd();

            return result;
        }


        /// <summary>
        /// This method converts a xml into an original object.
        /// </summary>
        /// <param name="xmlRequest">XML string to deseralize </param>
        /// <param name="type">Type of the object </param>
        /// <returns>Object with type introduced as a parameter in the method</returns>
        public object Deserialize(string xmlRequest, Type type)
        {
            #region Preconditions

            if (string.IsNullOrEmpty(xmlRequest) || string.IsNullOrWhiteSpace(xmlRequest))
                throw new ArgumentException("xmlRequest");
            if (type == null)
                throw new ArgumentNullException("type");

            #endregion

            object output = null;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlRequest);

            XmlNodeReader reader = new XmlNodeReader(xmlDocument.DocumentElement);
            XmlSerializer serializer = new XmlSerializer(type);

            output = serializer.Deserialize(reader);

            #region Postconditions

            if (output != null && !output.GetType().Equals(type))
                throw new InvalidCastException("output");

            #endregion

            return output;
        }


       
    }
}
