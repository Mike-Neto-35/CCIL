using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace CockatriceCardImageLoader.Cockatrice
{
    /// <summary>
    /// Collection of cards and sets.
    /// </summary>
    public class CollectionFile
    {
        [XmlAttribute("version")]
        public string FormatVersion { get; set; }

        [XmlElement("info")]
        public CollectionInfo Info { get; set; }

        [XmlArray("sets")]
        [XmlArrayItem("set")]
        public CollectionSet[] Sets { get; set; }

        [XmlArray("cards")]
        [XmlArrayItem("card")]
        public CollectionCard[] Cards { get; set; }



        public static CollectionFile ImportFromXmlFile(string xmlFilename)
        {
            CollectionFile cardFile = null;

            XmlDocument xmlDoc = new XmlDocument();

            string xml = System.IO.File.ReadAllText(xmlFilename);
            xml = lowerCaseAllNames(xml);
            xmlDoc.LoadXml(xml);

            XmlRootAttribute rootAttribute = new XmlRootAttribute("cockatrice_carddatabase");
            XmlSerializer serializer = new XmlSerializer(typeof(CollectionFile), rootAttribute);

            using (XmlReader reader = new XmlNodeReader(xmlDoc.DocumentElement))
            {
                cardFile = (CollectionFile)serializer.Deserialize(reader);
            }

            return cardFile;
        }



        public void ExportToXmlFile(string xmlFilename)
        {
            string resultXml = string.Empty;

            this.FormatVersion = "4";

            XmlRootAttribute rootAttribute = new XmlRootAttribute("cockatrice_carddatabase");
            XmlSerializer ser = new XmlSerializer(this.GetType(), rootAttribute);

            using (MemoryStream memStm = new MemoryStream())
            {
                ser.Serialize(memStm, this);

                memStm.Position = 0;
                resultXml = new StreamReader(memStm).ReadToEnd();
            }

            System.IO.File.WriteAllText(xmlFilename, resultXml);
        }

        private static string LOWER_CASE_XSL_TRANSFORMER = "<xsl:stylesheet version=\"1.0\"\r\n xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\">\r\n <xsl:output omit-xml-declaration=\"yes\" indent=\"yes\"/>\r\n <xsl:strip-space elements=\"*\"/>\r\n\r\n <xsl:variable name=\"vUpper\" select=\r\n \"'ABCDEFGHIJKLMNOPQRSTUVWXYZ'\"/>\r\n\r\n <xsl:variable name=\"vLower\" select=\r\n \"'abcdefghijklmnopqrstuvwxyz'\"/>\r\n\r\n <xsl:template match=\"node()|@*\">\r\n     <xsl:copy>\r\n       <xsl:apply-templates select=\"node()|@*\"/>\r\n     </xsl:copy>\r\n </xsl:template>\r\n\r\n <xsl:template match=\"*[name()=local-name()]\" priority=\"2\">\r\n  <xsl:element name=\"{translate(name(), $vUpper, $vLower)}\"\r\n   namespace=\"{namespace-uri()}\">\r\n       <xsl:apply-templates select=\"node()|@*\"/>\r\n  </xsl:element>\r\n </xsl:template>\r\n\r\n <xsl:template match=\"*\" priority=\"1\">\r\n  <xsl:element name=\r\n   \"{substring-before(name(), ':')}:{translate(local-name(), $vUpper, $vLower)}\"\r\n   namespace=\"{namespace-uri()}\">\r\n       <xsl:apply-templates select=\"node()|@*\"/>\r\n  </xsl:element>\r\n </xsl:template>\r\n\r\n <xsl:template match=\"@*[name()=local-name()]\" priority=\"2\">\r\n  <xsl:attribute name=\"{translate(name(), $vUpper, $vLower)}\"\r\n   namespace=\"{namespace-uri()}\">\r\n       <xsl:value-of select=\".\"/>\r\n  </xsl:attribute>\r\n </xsl:template>\r\n\r\n <xsl:template match=\"@*\" priority=\"1\">\r\n  <xsl:attribute name=\r\n   \"{substring-before(name(), ':')}:{translate(local-name(), $vUpper, $vLower)}\"\r\n   namespace=\"{namespace-uri()}\">\r\n     <xsl:value-of select=\".\"/>\r\n  </xsl:attribute>\r\n </xsl:template>\r\n</xsl:stylesheet>";

        private static string lowerCaseAllNames(string xml)
        {
            XslTransform xslt = new XslTransform();

            XmlDocument xslDoc = new XmlDocument();
            xslDoc.LoadXml(LOWER_CASE_XSL_TRANSFORMER);

            xslt.Load(xslDoc.DocumentElement);

            using (Stream xmlReader = streamFromString(xml))
            {
                XPathDocument mydata = new XPathDocument(xmlReader);

                //XmlWriter writer = new XmlTextWriter(Console.Out);
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        ////Transform the data and send the output to the console.
                        xslt.Transform(mydata, null, xmlWriter, null);
                    }
                    return stringWriter.ToString();
                }


            }
        }

        private int printCount = -1;

        public int PrintCount
        {
            get
            {
                if (printCount == -1)
                    printCount = computePrintCount();

                return printCount;
            }
        }

        private int computePrintCount()
        {
            int n = 0;

            foreach (CollectionCard card in this.Cards)
                n += card.Prints.Length;

            return n;
        }

        private static Stream streamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

}
