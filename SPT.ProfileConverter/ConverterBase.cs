using Newtonsoft.Json;

namespace SPT.ProfileConverter
{
    public class ConverterBase
    {
        public static event Action<string> ConversionProgress = delegate { };

        protected virtual string OnLoadConversion(string Profile) => Profile;

        protected JsonReader LoadProfile(string Profile)
        {
            // Read the JSON file, also add OnLoadConversion in between so things read into the reader can be manipulated prior to loading.
            JsonTextReader reader = new JsonTextReader(new StringReader(OnLoadConversion(File.ReadAllText(Profile))));
            // Handle Float parsing as decimal, leaving this as default brought a whole load of unnecessary changes.
            reader.FloatParseHandling = FloatParseHandling.Decimal;

            return reader;
        }
       
        public virtual ConversionStatus ConvertProfile(string FullFilePath) => new ConversionStatus();

        protected void OnConversionProgressChanged(string Progress) => ConversionProgress.Invoke(Progress);
    }
}
