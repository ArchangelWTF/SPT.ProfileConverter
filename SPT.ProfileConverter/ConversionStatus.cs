using Newtonsoft.Json.Linq;

namespace SPT.ProfileConverter
{
    public class ConversionStatus
    {
        public bool Successful { get; set; } = false;
        public string Result { get; set; } = string.Empty;
        public JObject? Object { get; set; } = null;
        public string JsonString { get; set; } = "";
    }
}
