using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SPT.ProfileConverter
{
    public static class JsonHelper
    {
        //Write profile with tabs instead of spaces.
        public static string JObjectToString(JObject profile)
        {
            StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);

            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
            using (JsonTextWriter JsonWriter = new JsonTextWriter(stringWriter))
            {
                JsonWriter.Formatting = Formatting.Indented;
                JsonWriter.IndentChar = '\t';
                JsonWriter.Indentation = 1;

                jsonSerializer.Serialize(JsonWriter, profile);
            }

            return stringWriter.ToString();
        }
    }
}
