using Newtonsoft.Json;

namespace WitAI.NET
{
    public class Locale
    {
        [JsonProperty("detected_locales")]
        public LocaleObject[] Locales { get; set; }
    }

    public class LocaleObject
    {
        public string Locale { get; set; }
        public double Confidence { get; set; }
    }
}
