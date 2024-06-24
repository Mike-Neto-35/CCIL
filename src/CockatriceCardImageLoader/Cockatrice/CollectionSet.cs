﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CockatriceCardImageLoader.Cockatrice
{
    public class CollectionSet
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("longname")]
        public string LongName { get; set; }

        [XmlElement("settype")]
        public string SetType { get; set; }

        [XmlElement("releasedate")]
        public string ReleaseDate { get; set; }
    }
}
