using CockatriceCardImageLoader.Cockatrice;
using CockatriceCardImageLoader.Planesculptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockatriceCardImageLoader.Convertors
{
    /// <summary>
    /// Converts Planesculptors card lists to Cockatrice collection files.
    /// </summary>
    public class PlanesculptorsToCockatriceConverter
    {
        public static CollectionFile Convert(CardList planesculptorsCardList, string setCode)
        {
            CollectionFile collectionFile = new CollectionFile();

            collectionFile.Info = new CollectionInfo();

            collectionFile.Sets = [new CollectionSet()];

            collectionFile.Cards = new CollectionCard[planesculptorsCardList.Count];

            int i = 0;

            foreach (Card card in planesculptorsCardList)
            {
                CollectionCard newItem = new CollectionCard();

                newItem.Name = card.Name;
                newItem.Text = ReplaceCostTags(card.RulesText);

                newItem.Properties = new CollectionCardProperties()
                {
                    Cmc = card.Cmc,
                    ManaCost = GetManaCost(card.ManaCost),
                    Colors = StringArrayToString(card.Colors),
                    Type = card.Types,
                    MainType = GetMainType(card.Types.ToLower()),
                    ColorIdentity = StringArrayToString(card.Colors),
                    Pt = card.PtString
                };

                newItem.Prints = [new CollectionCardPrint()
                {
                    Num = card.SequenceNumber.ToString(),
                    Muid = card.CardId.ToString(),
                    PicURL = card.ArtUrl,
                    Rarity = card.RarityName,
                    Illustrator = card.Illustrator,
                    SetName = setCode
                }];

                collectionFile.Cards[i] = newItem;
                i++;
            }

            return collectionFile;
        }

        private static string GetMainType(string text)
        {
            if (text.Contains("land"))
                return "Land";

            if (text.Contains("creature"))
                return "Creature";

            if (text.Contains("artifact"))
                return "Artifact";

            if (text.Contains("enchantment"))
                return "Enchantment";

            if (text.Contains("instant"))
                return "Instant";

            if (text.Contains("sorcery"))
                return "Sorcery";

            if (text.Contains("dungeon"))
                return "Dungeon";

            if (text.Contains("battles"))
                return "Battles";

            if (text.Contains("planes"))
                return "Planes";

            if (text.Contains("planeswalkers"))
                return "Planeswalkers";

            if (text.Contains("tribal"))
                return "Tribal";

            if (text.Contains("phenomenon"))
                return "Phenomenon";

            if (text.Contains("vanguard"))
                return "Vanguard";

            if (text.Contains("scheme"))
                return "Scheme";

            if (text.Contains("conspiracy"))
                return "Conspiracy";

            if (text.Contains("emblem"))
                return "Emblem";

            return "";
        }

        private static Dictionary<string, string> CostKeywords = new Dictionary<string, string>()
        {
            {"mana-x", "X"},
            {"mana-y", "Y"},
            {"mana-z", "Z"},
            {"tap", "T"},
            {"untap", "O"},
            {"energy", "E"},
            {"planeswalker", "PW"},
            {"chaos", "CHAOS"},
            {"colorless", "C"},
            {"snow", "S"},
            {"infinite", "∞"},

            {"mana-0", "0"},
            {"mana-1", "1"},
            {"mana-2", "2"},
            {"mana-3", "3"},
            {"mana-4", "4"},
            {"mana-5", "5"},
            {"mana-6", "6"},
            {"mana-7", "7"},
            {"mana-8", "8"},
            {"mana-9", "9"},
            {"mana-10", "10"},
            {"mana-11", "11"},
            {"mana-12", "12"},
            {"mana-13", "13"},
            {"mana-14", "14"},
            {"mana-15", "15"},
            {"mana-16", "16"},
            {"mana-17", "17"},
            {"mana-18", "18"},
            {"mana-19", "19"},
            {"mana-20", "20"},
            {"mana-21", "21"},
            {"mana-22", "22"},
            {"mana-23", "23"},
            {"mana-24", "24"},
            {"mana-25", "25"},
            {"mana-26", "26"},
            {"mana-27", "27"},
            {"mana-28", "28"},
            {"mana-29", "29"},
            {"mana-100", "100"},
            {"mana-1000000", "1000000"},

            {"white", "W"},
            {"red", "R"},
            {"green", "G"},
            {"blue", "U"},
            {"black", "B"},

            {"phyrexian-w", "W/P"},
            {"phyrexian-r", "R/P"},
            {"phyrexian-g", "G/P"},
            {"phyrexian-u", "U/P"},
            {"phyrexian-b", "B/P"},
            {"phyrexian-colorless", "C/P"},

            {"hybrid-wr", "W/R"},
            {"hybrid-wg", "W/G"},
            {"hybrid-wu", "W/U"},
            {"hybrid-wb", "W/B"},

            {"hybrid-rw", "R/W"},
            {"hybrid-rg", "R/G"},
            {"hybrid-ru", "R/U"},
            {"hybrid-rb", "R/B"},

            {"hybrid-gw", "G/W"},
            {"hybrid-gr", "G/R"},
            {"hybrid-gu", "G/U"},
            {"hybrid-gb", "G/B"},

            {"hybrid-uw", "U/W"},
            {"hybrid-ur", "U/R"},
            {"hybrid-ug", "U/G"},
            {"hybrid-ub", "U/B"},

            {"hybrid-bw", "B/W"},
            {"hybrid-br", "B/R"},
            {"hybrid-bg", "B/G"},
            {"hybrid-bu", "B/U"},

            {"hybrid-2w", "2/W"},
            {"hybrid-2r", "2/R"},
            {"hybrid-2g", "2/G"},
            {"hybrid-2u", "2/U"},
            {"hybrid-2b", "2/B"},
        };

        private static string ReplaceCostTags(string htmlRuleText)
        {
            foreach (string key in CostKeywords.Keys.ToArray())
            {
                htmlRuleText = htmlRuleText.Replace($"<span class=\"icon-wrapper\"><i class=\"mtg {key}\"></i></span>", "{" + CostKeywords[key] + "}");
            }

            return htmlRuleText;
        }

        private static string GetManaCost(string htmlManaCost)
        {
            foreach (string key in CostKeywords.Keys.ToArray())
            {
                htmlManaCost = htmlManaCost.Replace($"<span class=\"icon-wrapper\"><i class=\"mtg {key}\"></i></span>", CostKeywords[key]);
            }

            return htmlManaCost;
        }

        private static string GetManaCost(string htmlManaCost, string key, string alias, string prefix = "", string sufix = "")
        {
            StringBuilder buffer = new StringBuilder();

            int i = htmlManaCost.IndexOf(key);

            while (i >= 0)
            {
                i++;

                buffer.Append(prefix + alias + sufix);

                i = htmlManaCost.IndexOf(key, i + 1);
            }

            return buffer.ToString(); ;
        }

        private static string StringArrayToString(string[] stringArray)
        {
            StringBuilder buffer = new StringBuilder();

            if (stringArray != null && stringArray.Length > 0)
            {
                foreach (string str in stringArray)
                    buffer.Append(str);
            }

            return buffer.ToString();
        }
    }
}
