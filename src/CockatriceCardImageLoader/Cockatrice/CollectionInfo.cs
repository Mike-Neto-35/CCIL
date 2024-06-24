using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CockatriceCardImageLoader.Cockatrice
{
    public class CollectionInfo
    {
        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("createdat")]
        public string CreatedAt { get; set; }

        [XmlElement("sourceurl")]
        public string SourceUrl { get; set; }

        [XmlElement("sourceversion")]
        public string SourceVersion { get; set; }
    }
}
