using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using FifthBot.Core.Data;
using FifthBot.Core.Utils;
using FifthBot.Resources.Database;

namespace FifthBot.Core.Utils
{
    class MenuCreateMethods
    {
        public async Task GroupMenuCreator(SocketCommandContext Context)
        {
            if (Context.Channel.Id != Vars.menuBuilder.ChannelID)
            {
                return;
            }

            if (!Vars.menuBuilder.IsActive)
            {
                return;
            }

            var emojiMenuMsg = (RestUserMessage)await Context.Channel.GetMessageAsync(Vars.menuBuilder.EmojiMenuID);

            var editMenuMsg = (RestUserMessage)await Context.Channel.GetMessageAsync(Vars.menuBuilder.EditMenuID);


            if (Vars.menuBuilder.CommandStep == 0)
            {
                string groupName = Context.Message.Content;

                KinkGroup groupToMenufy = DataMethods.GetGroup(groupName);

                if(groupToMenufy == null)
                {
                    string quitting = "Invalid group name, quitting!";
                    await emojiMenuMsg.ModifyAsync(x => x.Content = quitting);
                    await editMenuMsg.ModifyAsync(x => x.Content = quitting);

                    WipeMenuBuilder();
                    return;

                }

                Vars.menuBuilder.CommandStep = 1;
                Vars.menuBuilder.KinkGroupName = groupToMenufy.KinkGroupName;
                Vars.menuBuilder.KinkGroupID = groupToMenufy.KinkGroupID;
                Vars.menuBuilder.KinksToUpdate = DataMethods.GetKinksInGroupWithEmojis(groupToMenufy.KinkGroupID, Vars.menuBuilder.ServerID);



                if (Vars.menuBuilder.KinksToUpdate == null)
                {
                    string quitting = "No kinks in group, quitting!";
                    await emojiMenuMsg.ModifyAsync(x => x.Content = quitting);
                    await editMenuMsg.ModifyAsync(x => x.Content = quitting);

                    WipeMenuBuilder();
                    return;

                }

                string fartz = "";
                foreach (KinkWithEmoji shitz in Vars.menuBuilder.KinksToUpdate)
                {
                    fartz += "Kink Data - KinkID: " + shitz.KinkID + ", ServerID: " + shitz.ServerID + ", Emoji Name: " + shitz.EmojiName + "\n";
                }
                Console.WriteLine(fartz);

                string emojiMenuText = "​\n" + "Building Menu" + "\n​" + "\n​"
                    + "Group - " + Vars.menuBuilder.KinkGroupName + "\n​";
                await emojiMenuMsg.ModifyAsync(x => x.Content = emojiMenuText);

                string editMenuText = "​\n" + "Edit Menu" + "\n​" + "\n​"
                    + "Valid group name!" + "\n​";
                
                await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);

                await Task.Delay(2000);

                editMenuText += "​\n" + "Is this a kink menu or a limit menu? Enter Kink, Limit, or anything else to quit" + "\n​";
                await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);

            }
            else if (Vars.menuBuilder.CommandStep == 1)
            {
                string limitOrKink = Context.Message.Content;
                limitOrKink.ToLower();
                limitOrKink = char.ToUpper(limitOrKink[0]) + limitOrKink.Substring(1);


                if (limitOrKink != "Limit" && limitOrKink != "Kink")
                {
                    string quitting = "Invalid response, quitting!";
                    await emojiMenuMsg.ModifyAsync(x => x.Content = quitting);
                    await editMenuMsg.ModifyAsync(x => x.Content = quitting);

                    WipeMenuBuilder();
                    return;
                }

                if (limitOrKink == "limit")
                {
                    Vars.menuBuilder.IsLimitMenu = true;
                }

                Vars.menuBuilder.CommandStep++;

                string emojiMenuText = "​\n" + "Building Menu" + "\n​" + "\n​" 
                    + limitOrKink + " Group - " + Vars.menuBuilder.KinkGroupName + "\n​";
                await emojiMenuMsg.ModifyAsync(x => x.Content = emojiMenuText);


                string editMenuText = "​\n" + "Edit Menu" + "\n​" + "\n​" 
                    + "Valid Entry!" + "\n​";

                await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);

                await Task.Delay(2000);

                editMenuText += "​\n" + "Reuse emoji data or enter everything from scratch? Reuse, Scratch, or anything else to quit" + "\n​";
                await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);




            }
            else if (Vars.menuBuilder.CommandStep == 2)
            {
                string limitOrKink = Vars.menuBuilder.IsLimitMenu ? "Limit" : "Kink";
                string reuseOrScratch = Context.Message.Content;
                reuseOrScratch.ToLower();
                reuseOrScratch = char.ToUpper(reuseOrScratch[0]) + reuseOrScratch.Substring(1);


                if (reuseOrScratch != "Reuse" && reuseOrScratch != "Scratch")
                {
                    string quitting = "Invalid response, quitting!";
                    await emojiMenuMsg.ModifyAsync(x => x.Content = quitting);
                    await editMenuMsg.ModifyAsync(x => x.Content = quitting);

                    WipeMenuBuilder();
                    return;
                }

                if (reuseOrScratch == "scratch")
                {
                    /*
                    foreach(Kink kink in Vars.menuBuilder.KinksToUpdate)
                    {
                        kink.EmojiID = 0;
                        kink.EmojiName = "";
                    }
                    */
                }

                Vars.menuBuilder.CommandStep++;

                string emojiMenuText = "​\n" + "Building Menu" + "\n​" + "\n​"
                    + limitOrKink + " Group - " + Vars.menuBuilder.KinkGroupName + "\n​";
                await emojiMenuMsg.ModifyAsync(x => x.Content = emojiMenuText);


                string editMenuText = "​\n" + "Edit Menu" + "\n​" + "\n​"
                    + "Valid Entry!" + "\n​";

                await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);

                /*
                string firstKinkName = Vars.menuBuilder.KinksToUpdate.Where(x => x.EmojiID == 0).FirstOrDefault().KinkName;

                await Task.Delay(2000);

                editMenuText += "​\n" + "Please react to THIS post with the emoji for: " + firstKinkName + "\n​";
                await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);
                */


                WipeMenuBuilder();
            }








        }

        public async Task GroupMenuEmojiAdder(SocketReaction reaction)
        {


        }






        private void WipeMenuBuilder()
        {
            Vars.menuBuilder.IsActive = false;
            Vars.menuBuilder.EmojiMenuID = 0;
            Vars.menuBuilder.EditMenuID = 0;
            Vars.menuBuilder.ChannelID = 0;
            Vars.menuBuilder.UserID = 0;
            Vars.menuBuilder.KinkGroupName = "";
            Vars.menuBuilder.CommandStep = 0;
            Vars.menuBuilder.IsLimitMenu = false;
            Vars.menuBuilder.KinksToUpdate = null;
        }




    }
}
