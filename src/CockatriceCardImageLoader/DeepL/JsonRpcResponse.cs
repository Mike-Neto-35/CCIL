using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockatriceCardImageLoader.DeepL
{
    public class JsonRpcResponse
    {
        public string Jsonrpc { get; set; }
        public long Id { get; set; }
        public Result Result { get; set; }
    }

    public class Result
    {
        public List<Translation> Translations { get; set; }
        public string TargetLang { get; set; }
        public string SourceLang { get; set; }
        public bool SourceLangIsConfident { get; set; }
        public Dictionary<string, object> DetectedLanguages { get; set; }
    }

    public class Translation
    {
        public List<Beam> Beams { get; set; }
        public string Quality { get; set; }
    }

    public class Beam
    {
        public List<Sentence> Sentences { get; set; }
        public int NumSymbols { get; set; }
    }

}
