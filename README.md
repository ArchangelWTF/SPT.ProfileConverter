# SPT Profile Converter
## Do not go and ask for support for this in the SPT or FIKA Discord server! You will not get any support for profiles converted with this tool. (And even has properties to make it easier for SPT support staff to check if a profile has been converted.)

A tool for converting profiles from SPT 3.8 to SPT 3.9, mainly because I got sick of doing this manually with each profile. <br>
If you have issues feel free to open an issue on this repository but dont expect me to hold your hand.

---
## For converting manually see below: <br>
Changes across the entire file required (Search and replace): 
sptUsec -> pmcUSEC
sptBear -> pmcBear

---
moneyTransferLimitData needs to be implemented on profiles, not really necessary as it's a arena property, but should probably implement none the less.
"moneyTransferLimitData": {
                "nextResetTime": 1717779074,
                "remainingLimit": 1000000,
                "totalLimit": 1000000,
                "resetInterval": 86400
            },


---
The "Info" PMC object has to have the following changes applied below "MemberCategory":
"isMigratedSkills": false,
"SelectedMemberCategory": 0,

For standard accounts as well as left behind accounts the value in SelectedMemberCategory should stay 0, if your account is an EOD account set this to 2
--
Every occurance with "_tpl": "5d235b4d86f7742e017bc88a" needs to have "upd" modified, replace: 
"Resource": {
     "Value": 0
}
with "StackObjectsCount": 1