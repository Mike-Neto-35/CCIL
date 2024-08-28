using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CockatriceCardImageLoader.Cockatrice
{
    /// <summary>
    /// Describes a card from a collection.
    /// </summary>
    public class CollectionCard
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("text")]
        public string Text { get; set; }

        [XmlElement("tablerow")]
        public string Tablerow { get; set; }

        [XmlElement("token")]
        public string Token { get; set; }

        [XmlElement("prop")]
        public CollectionCardProperties Properties { get; set; }
        //public Dictionary<string, string> Properties { get; set; }

        [XmlElement("set")]
        public CollectionCardPrint[] Prints { get; set; }

        [XmlElement("reverse-related")]
        public CollectionCardRelation[] ReverseRelated { get; set; }



        public CollectionCard Clone()
        {
            CollectionCard clone = new CollectionCard
            {
                Name = this.Name,
                Text = this.Text,
                Properties = this.Properties,
                Token = this.Token,
                Tablerow = this.Tablerow,
            };

                clone.Prints = new CollectionCardPrint[this.Prints.Length];

            for (int i = 0; i < this.Prints.Length; i++)
                clone.Prints[i] = new CollectionCardPrint
                {
                    Illustrator = this.Prints[i].Illustrator,
                    Muid = this.Prints[i].Muid,
                    Num = this.Prints[i].Num,
                    PicURL = this.Prints[i].PicURL,
                    Rarity = this.Prints[i].Rarity,
                    SetName = this.Prints[i].SetName,
                    Uuid = this.Prints[i].Uuid
                };

            clone.ReverseRelated = new CollectionCardRelation[this.ReverseRelated.Length];

            for (int i = 0; i < this.ReverseRelated.Length; i++)
                clone.ReverseRelated[i] = new CollectionCardRelation
                {
                    Attach = this.ReverseRelated[i].Attach,
                    CardName = this.ReverseRelated[i].CardName,
                    Count = this.ReverseRelated[i].Count,
                    Exclude = this.ReverseRelated[i].Exclude,
                };

            return clone;
        }
    }
}
