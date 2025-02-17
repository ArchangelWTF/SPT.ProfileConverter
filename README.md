# SPT Profile Converter
## Do not go and ask for support for this in the SPT or FIKA Discord server! You will not get any support for profiles converted with this tool. (And even has markers to make it easier for SPT support staff to check if a profile has been converted.)

A tool for converting profiles from SPT 3.8 to SPT 3.9, mainly because I got sick of doing this manually with each profile.

If you have issues feel free to open an issue on this repository or contact me on Discord (archangel.sh) but dont expect me to hold your hand.

This tool is only for converting profiles with 'vanilla' items, if you convert a profile that has modded items added there will probably still be issues, you will have to either have to wait until the mod has 3.9 compatability or remove those items out of your profile manually.

## Requires:
[.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Usage:

Open command line in the directory you have extracted the executable, and target the profile as an argument.

OR

Drag and drop the profile on the executable.

## For converting manually see below:

Changes across the entire file required (Search and replace): 
```
sptUsec -> pmcUSEC
sptBear -> pmcBear
```
moneyTransferLimitData needs to be implemented in the ["character"]["pmc"] object, not really necessary as it's a arena property, but should probably implement none the less.
```
"moneyTransferLimitData": {
                "nextResetTime": 1717779074,
                "remainingLimit": 1000000,
                "totalLimit": 1000000,
                "resetInterval": 86400
            },
```

---
The "Info" PMC object has to have the following changes applied below "MemberCategory":

```
"isMigratedSkills": false,
"SelectedMemberCategory": 0,
```
For standard accounts as well as left behind accounts the value in SelectedMemberCategory should stay 0, if your account is an EOD account set this to 2

---
Every occurance with "_tpl": "5d235b4d86f7742e017bc88a" needs to have "upd" modified, replace: 
```
"Resource": {
     "Value": 0
}
```
with "StackObjectsCount": 1
