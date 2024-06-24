using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockatriceCardImageLoader.Planesculptors
{
    public class SiteInterface
    {
        public static string GetSetJson(string setUrl)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] raw = wc.DownloadData(setUrl);
            string html = System.Text.Encoding.UTF8.GetString(raw);

            string json = null;

            int i = html.IndexOf("var cardData = [");

            if (i != -1)
            {
                int f = findClosingBracketIndex(html, i);

                if (f != -1)
                    json = html.Substring(i + 15, f - i - 14);
            }

            return json;
        }

        private static int findClosingBracketIndex(string input, int startIndex)
        {
            int openBrackets = 0;

            for (int i = startIndex; i < input.Length; i++)
            {
                if (input[i] == '[')
                {
                    openBrackets++;
                }
                else if (input[i] == ']')
                {
                    openBrackets--;
                    if (openBrackets == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
