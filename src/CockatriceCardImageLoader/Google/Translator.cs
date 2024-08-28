using CockatriceCardImageLoader.DeepL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CockatriceCardImageLoader.Google
{
    public class Translator
    {
        public static string Translate(string entry, string sourceLanguage, string targetLanguage)
        {
            entry = Uri.EscapeDataString(entry);

            string requestLink = "https://www.google.com/async/translate?vet=12ahUKEwiJ1PeH7ZeIAxVxU0EAHa7CBbIQqDh6BAgHEC4..i&ei=Fi7PZonxGvGmhbIProWXkAs&opi=89978449&rlz=1C1RXQR_pt-PTPT1095PT1095&yv=3&_fmt=pc&cs=0";
            string requestBody = $"async=translate,sl:{sourceLanguage},tl:{targetLanguage},st:{entry},id:1724855052667,qc:true,ac:false,_id:tw-async-translate,_pms:s,_fmt:pc";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestLink);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(requestBody);
            }

            string result = null;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            if (result != null)
            {
                string openingTag = "id=\"tw-answ-target-text\">";
                string closingTag = "</span>";

                int i = result.IndexOf(openingTag);

                if (i >= 0)
                {
                    int f = result.IndexOf(closingTag, i);

                    if (f >= 0)
                    {
                        result = result.Substring(i + openingTag.Length, f - i - openingTag.Length);
                    }
                }
            }

            return result;
        }
    }
}
