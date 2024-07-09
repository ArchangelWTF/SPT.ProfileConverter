using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SPT.ProfileConverter.Converters
{
    public class ThreeEightToThreeNine : ConverterBase
    {
        protected override string OnLoadConversion(string Profile)
        {
            Profile = Profile.Replace("sptUsec", "pmcUSEC");
            Profile = Profile.Replace("sptBear", "pmcBear");

            OnConversionProgressChanged("Ran OnLoadConversion: sptUsec -> pmcUSEC | sptBear -> pmcBear");

            return Profile;
        }

        public override ConversionStatus ConvertProfile(string FullFilePath)
        {
            JObject jsonObj = JObject.Load(LoadProfile(FullFilePath));

            if (jsonObj["info"]["ProfileConvertedFrom38"] != null)
            {
                return new ConversionStatus { Successful = false, Result = "This profile has already been converted!" };
            }

            if (jsonObj["spt"]?["version"] != null)
            {
                string SPTVersion = jsonObj["spt"]["version"].ToString();

                if(SPTVersion.Contains("3.9"))
                {
                    return new ConversionStatus { Successful = false, Result = "This profile was created in 3.9, skipping conversion" };
                }
            }

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

            // Implement moneyTransferLimitData on PMC profile
            var PMCObject = jsonObj["characters"]["pmc"];
            if (PMCObject != null)
            {
                PMCObject["moneyTransferLimitData"] = new JObject
                {
                    ["nextResetTime"] = 1717779074,
                    ["remainingLimit"] = 1000000,
                    ["totalLimit"] = 1000000,
                    ["resetInterval"] = 86400
                };
            }
            else
            {
                return new ConversionStatus { Successful = false, Result = "PMC character object does not exist!" };
            }

            OnConversionProgressChanged("Added moneyTransferLimitData to profile");

            // Modify the "Info" PMC object
            var pmcInfoObject = PMCObject["Info"];
            if (pmcInfoObject != null)
            {
                pmcInfoObject["ProfileConvertedFromPreviousVersion"] = true;
                pmcInfoObject["isMigratedSkills"] = false;

                if (pmcInfoObject["GameVersion"]?.ToString() == "edge_of_darkness")
                {
                    pmcInfoObject["SelectedMemberCategory"] = 2;
                }
                else
                {
                    pmcInfoObject["SelectedMemberCategory"] = 0;
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
