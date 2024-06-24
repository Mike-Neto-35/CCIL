using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CockatriceCardImageLoader.Cockatrice
{
    public class CollectionCard
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("text")]
        public string Text { get; set; }

        [XmlElement("prop")]
        public CollectionCardProperties Properties { get; set; }

        [XmlElement("set")]
        public CollectionCardPrint[] Prints { get; set; }
    }
}
