using SPT.ProfileConverter.Converters;

namespace SPT.ProfileConverter.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("----------");
            System.Console.WriteLine("SPT.ProfileConverter for converting profiles from SPT 3.8 to SPT 3.9");
            System.Console.WriteLine("https://github.com/ArchangelWTF/SPT.ProfileConverter");
            System.Console.WriteLine("Using this voids all support you will have for your profile in the SPT Discord server.");
            System.Console.WriteLine("----------");

            if (args.Length == 0)
            {
                System.Console.WriteLine("No profile specified!");
                return;
            }

            string file = args[0];

            ConverterBase.ConversionProgress += ConverterBase_ConversionProgress;
            ConversionStatus Converter = new ThreeEightToThreeNine().ConvertProfile(file);

            if(Converter.Successful)
            {
                System.Console.WriteLine($"Profile {file} has been successfully converted.");

                //Write file back with LF file endings just like the original profile.
                File.WriteAllText(file, Converter.JsonString.Replace("\r\n", "\n"));
            }
            else
            {
                System.Console.WriteLine(Converter.Result);
            }

            System.Console.ReadKey();
        }

        private static void ConverterBase_ConversionProgress(string Status) => System.Console.WriteLine(Status);
    }
}
