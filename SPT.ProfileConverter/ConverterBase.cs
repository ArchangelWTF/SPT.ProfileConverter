namespace SPT.ProfileConverter
{
    public class ConverterBase
    {
        public static event Action<string> ConversionProgress = delegate { };

        public virtual ConversionStatus ConvertProfile(string FullFilePath)
        {
            return new ConversionStatus();
        }

        protected void OnConversionProgressChanged(string Progress) => ConversionProgress.Invoke(Progress);
    }
}
