using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockatriceCardImageLoader.Planesculptors
{
    /// <summary>
    /// Describes a list of cards from a Planesculptors set.
    /// </summary>
    public class CardList : List<Card>
    {
        public static CardList ImportFromFile(string filename)
        {
            string json = System.IO.File.ReadAllText(filename);

            CardList setFile = JsonConvert.DeserializeObject<CardList>(json);

            return setFile;
        }
    }
}
