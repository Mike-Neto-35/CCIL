using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CockatriceCardImageLoader.Cockatrice
{
    /// <summary>
    /// Describes the properties of a card.
    /// </summary>
    public class CollectionCardProperties
    {
        [XmlElement("format-brawl")]
        public string FormatBrawl { get; set; }

        [XmlElement("maintype")]
        public string MainType { get; set; }

        [XmlElement("format-timeless")]
        public string FormatTimeless { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlElement("manacost")]
        public string ManaCost { get; set; }

        [XmlElement("cmc")]
        public string Cmc { get; set; }

        [XmlElement("format-pioneer")]
        public string FormatPioneer { get; set; }

        [XmlElement("format-duel")]
        public string FormatDuel { get; set; }

        [XmlElement("format-paupercommander")]
        public string FormatPauperCommander { get; set; }

        [XmlElement("format-explorer")]
        public string FormatExplorer { get; set; }

        [XmlElement("format-modern")]
        public string FormatModern { get; set; }

        [XmlElement("coloridentity")]
        public string ColorIdentity { get; set; }

        [XmlElement("colors")]
        public string Colors { get; set; }

        [XmlElement("format-oathbreaker")]
        public string FormatOathbreaker { get; set; }

        [XmlElement("format-historic")]
        public string FormatHistoric { get; set; }

        [XmlElement("format-vintage")]
        public string FormatVintage { get; set; }

        [XmlElement("side")]
        public string Side { get; set; }

        [XmlElement("pt")]
        public string Pt { get; set; }

        [XmlElement("format-legacy")]
        public string FormatLegacy { get; set; }

        [XmlElement("format-gladiator")]
        public string FormatGladiator { get; set; }

        [XmlElement("layout")]
        public string Layout { get; set; }

        [XmlElement("format-commander")]
        public string FormatCommander { get; set; }
    }
}
