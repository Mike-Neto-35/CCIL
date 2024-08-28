using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockatriceCardImageLoader.Planesculptors
{
    /// <summary>
    /// Describes a card of a Planesculptors set.
    /// </summary>
    public class Card
    {
        public string CardId { get; set; }
        public string SequenceNumber { get; set; }
        public string Shape { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string Cmc { get; set; }
        public string[] Colors { get; set; }
        public string Types { get; set; }
        public string ArtUrl { get; set; }
        public string Url { get; set; }
        public string RulesText { get; set; }
        public string FlavorText { get; set; }
        public string Rarity { get; set; }
        public string RarityName { get; set; }
        public string PtString { get; set; }
        public string Illustrator { get; set; }


        // Optional properties for secondary face of the card (if any)
        public string Name2 { get; set; }
        public string ManaCost2 { get; set; }
        public string Types2 { get; set; }
        public string RulesText2 { get; set; }
        public string FlavorText2 { get; set; }
        public string PtString2 { get; set; }
        public string Illustrator2 { get; set; }

        // Scripting
        public string SetVersionLink { get; set; }
        public string BbCode { get; set; }
    }
}
