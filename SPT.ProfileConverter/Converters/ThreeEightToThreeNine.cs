﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SPT.ProfileConverter.Converters
{
    public class ThreeEightToThreeNine : ConverterBase
    {
        public override ConversionStatus ConvertProfile(string FullFilePath)
        {
            // Read the JSON file, immediately convert to correct PMC types.
            var reader = new JsonTextReader(new StringReader(ConvertPMCTypes(File.ReadAllText(FullFilePath))));
            //Handle Float parsing as decimal, leaving this as default brought a whole load of unnecessary changes.
            reader.FloatParseHandling = FloatParseHandling.Decimal;
            var jsonObj = JObject.Load(reader);

            // Add profile converted marker to the info object.
            var info = jsonObj["info"];
            if (info != null)
            {
                info["ProfileConvertedFrom38"] = true;
            }
            else
            {
                return new ConversionStatus { Successful = false, Result = "info object does not exist!" };
            }

            OnConversionProgressChanged("Added profile converter marker");

            // Implement moneyTransferLimitData on profiles
            var profiles = jsonObj["profiles"];
            if (profiles != null)
            {
                foreach (var profile in profiles.Children<JObject>())
                {
                    profile["moneyTransferLimitData"] = new JObject
                    {
                        ["nextResetTime"] = 1717779074,
                        ["remainingLimit"] = 1000000,
                        ["totalLimit"] = 1000000,
                        ["resetInterval"] = 86400
                    };
                }
            }

            OnConversionProgressChanged("Added moneyTransferLimitData to profile");

            // Modify the "Info" PMC object
            var pmcObject = jsonObj["characters"]["pmc"]["Info"];
            if (pmcObject != null)
            {
                pmcObject["ProfileConvertedFromPreviousVersion"] = true;
                pmcObject["isMigratedSkills"] = false;

                if (pmcObject["GameVersion"]?.ToString() == "edge_of_darkness")
                {
                    pmcObject["SelectedMemberCategory"] = 2;
                }
                else
                {
                    pmcObject["SelectedMemberCategory"] = 0;
                }
            }
            else
            {
                return new ConversionStatus { Successful = false, Result = "PMC object does not exist!" };
            }

            OnConversionProgressChanged("Added new Arena data to PMC object");

            // Generated with ChatGPT, modify all the GP coins with the correct data.
            ModifyGPCoinTpl(jsonObj);
            
            return new ConversionStatus
            {
                Successful = true,
                Result = "Successfully converted",
                Object = jsonObj,
                JsonString = JsonHelper.JObjectToString(jsonObj)
            };
        }

        private string ConvertPMCTypes(string JsonFile)
        {
            JsonFile.Replace("sptUsec", "pmcUSEC");
            JsonFile.Replace("sptBear", "pmcBear");

            return JsonFile;
        }

        private void ModifyGPCoinTpl(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties().ToArray())
                {
                    if (property.Name == "_tpl" && property.Value.ToString() == "5d235b4d86f7742e017bc88a")
                    {
                        var parentObj = (JObject)property.Parent;
                        var resource = parentObj["upd"]["Resource"];
                        bool SpawnedInSession = Convert.ToBoolean(parentObj["upd"]["SpawnedInSession"]);
                        if (resource != null)
                        {
                            OnConversionProgressChanged($"Modifying GP coin with _id: {parentObj["_id"]}");

                            parentObj["upd"].Parent.Remove(); // Remove the "Resource" object
                            parentObj["upd"] = new JObject
                            {
                                ["SpawnedInSession"] = SpawnedInSession,
                                ["StackObjectsCount"] = 1
                            };
                        }
                    }
                    ModifyGPCoinTpl(property.Value);
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (var item in token.Children())
                {
                    ModifyGPCoinTpl(item);
                }
            }
        }
    }
}