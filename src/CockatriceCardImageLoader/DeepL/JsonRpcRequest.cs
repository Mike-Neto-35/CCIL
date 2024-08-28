using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockatriceCardImageLoader.DeepL
{
    public class JsonRpcRequest
    {
        public string Jsonrpc { get; set; }
        public string Method { get; set; }
        public Params Params { get; set; }
        public long Id { get; set; }
    }

    public class Params
    {
        public List<Job> Jobs { get; set; }
        public Lang Lang { get; set; }
        public int Priority { get; set; }
        public CommonJobParams CommonJobParams { get; set; }
        public long Timestamp { get; set; }
    }

    public class Job
    {
        public string Kind { get; set; }
        public List<Sentence> Sentences { get; set; }
        public List<object> RawEnContextBefore { get; set; }
        public List<object> RawEnContextAfter { get; set; }
        public int PreferredNumBeams { get; set; }
    }

    public class Sentence
    {
        public string Text { get; set; }
        public int Id { get; set; }
        public string Prefix { get; set; }
        public List<int> Ids { get; set; }
    }

    public class Lang
    {
        public string TargetLang { get; set; }
        public Preference Preference { get; set; }
        public string SourceLangComputed { get; set; }
    }

    public class Preference
    {
        public List<object> Weight { get; set; }
        public string Default { get; set; }
    }

    public class CommonJobParams
    {
        public string Quality { get; set; }
        public string RegionalVariant { get; set; }
        public string Mode { get; set; }
        public int BrowserType { get; set; }
        public string TextType { get; set; }
    }

}