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
    public class CollectionCardPrint
    {
        [XmlText]
        public string SetCode { get; set; }

        [XmlAttribute("picurl")]
        public string PicURL { get; set; }

        [XmlAttribute("uuid")]
        public string Uuid { get; set; }

        [XmlAttribute("rarity")]
        public string Rarity { get; set; }

        [XmlAttribute("num")]
        public string Num { get; set; }

        [XmlAttribute("muid")]
        public string Muid { get; set; }

        [XmlAttribute("illustrator")]
        public string Illustrator { get; set; }



        public string PrintImageUrl(string baseUrl)
        {
            if (this.PicURL != null)
            {
                return this.PicURL;
            }
            else
            {
                return Path.Combine(baseUrl, this.Uuid[0].ToString(), this.Uuid[1].ToString(), this.Uuid + ".jpg?");
            }
        }

        public bool HasPrintImage
        {
            get
            {
                return this.PicURL != null || this.Uuid != null;
            }
        }
    }
}
