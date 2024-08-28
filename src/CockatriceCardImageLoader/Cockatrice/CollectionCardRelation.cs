using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CockatriceCardImageLoader.Cockatrice
{
    /// <summary>
    /// Describes a print of a card in a specific set.
    /// </summary>
    public class CollectionCardRelation
    {
        [XmlText]
        public string CardName { get; set; }

        [XmlAttribute("exclude")]
        public string Exclude { get; set; }

        [XmlAttribute("count")]
        public string Count { get; set; }

        [XmlAttribute("attach")]
        public string Attach { get; set; }

    }
}
