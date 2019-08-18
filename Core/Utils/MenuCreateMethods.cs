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

                Console.WriteLine("We're passing GetKinksInGroupWithEmojis.");



                if (Vars.menuBuilder.KinksToUpdate == null)
                {
                    string quitting = "No kinks in group, quitting!";
                    await emojiMenuMsg.ModifyAsync(x => x.Content = quitting);
                    await editMenuMsg.ModifyAsync(x => x.Content = quitting);

                    WipeMenuBuilder();
                    return;

                }

                Console.WriteLine("We're passing kinkstoupdate null check.");

                //CONSOLE DATA CHECK
                Console.WriteLine("Writing kinks to update\n");
                foreach (var kink in Vars.menuBuilder.KinksToUpdate)
                {
                   Console.WriteLine("name - " + kink.KinkName + ", group - " + kink.KinkGroupID + "\n");
                }
                Console.WriteLine("Done");

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
                Vars.menuBuilder.CommandStep = 3;
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

                if (reuseOrScratch == "Scratch")
                {
                    
                    foreach(KinkWithEmoji kinkWE in Vars.menuBuilder.KinksToUpdate)
                    {
                        kinkWE.ServerID = 0;
                        kinkWE.EmojiName = "";
                    }
                    
                }

                string emojiMenuText = "​\n" + "Building Menu" + "\n​" + "\n​"
                    + limitOrKink + " Group - " + Vars.menuBuilder.KinkGroupName + "\n" + "\n​";
                await emojiMenuMsg.ModifyAsync(x => x.Content = emojiMenuText);

                string editMenuText = "​\n" + "Edit Menu" + "\n​" + "\n​"
                    + "Valid Entry!" + "\n​";

                var setKinks = Vars.menuBuilder.KinksToUpdate.Where(x => x.EmojiName != "").ToList();

                foreach(var kinkWE in setKinks)
                {
                    IEmote myEmote = null;
                    if (Emote.TryParse(kinkWE.EmojiName, out Emote shitfartz))
                    {
                        myEmote = shitfartz;
                    }
                    else
                    {
                        myEmote = new Emoji(kinkWE.EmojiName);
                    }
                        
                        //= Emote.TryParse(kinkWE.EmojiName, out Emote shitfartz) ? shitfartz : new Emoji(kinkWE.EmojiName);

                    /*
                    var anEmoji = Context.Guild.Emotes.Where(x => x.Name == kinkWE.EmojiName).FirstOrDefault();

                    string anEmojiString = anEmoji.ToString();
                    */
                    emojiMenuText += myEmote + " " + kinkWE.KinkName + " - " + kinkWE.KinkDesc + "\n" + "\n​";
                    
                    await Task.Delay(1000);
                    await emojiMenuMsg.ModifyAsync(x => x.Content = emojiMenuText);
                    await emojiMenuMsg.AddReactionAsync(myEmote);
                }

                //Guild.Emotes.Where(x => x.Name == kinkToUpdate.EmojiName).FirstOrDefault();


                await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);

                //remember if you do reuse that this may not work cause it
                // could be full

                if (!Vars.menuBuilder.KinksToUpdate.Any(x => x.EmojiName.Equals("")))
                {
                    editMenuText += "​\n" + "You are all out of kinks to enter! Writing Menu to Database!" + "\n​";
                    await DataMethods.AddKinkMenu();

                    await Task.Delay(2000);

                    await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);

                    WipeMenuBuilder();

                    return;

                }

                string firstKinkName = Vars.menuBuilder.KinksToUpdate.Where(x => x.EmojiName.Equals("")).FirstOrDefault().KinkName;
                editMenuText += "​\n" + "Please react to THIS post with the emoji for: " + firstKinkName + "\n​";

                await Task.Delay(2000);

                await editMenuMsg.ModifyAsync(x => x.Content = editMenuText);


            }


        }




















        public async Task GroupMenuEmojiAdder(SocketReaction reaction)
        {
            Console.WriteLine("Launched GroupMenuEmojiAdder");

            string limitOrKink = Vars.menuBuilder.IsLimitMenu ? "Limit" : "Kink";

            var emojiMenuMsg = (RestUserMessage)await reaction.Channel.GetMessageAsync(Vars.menuBuilder.EmojiMenuID);

            var editMenuMsg = (RestUserMessage)await reaction.Channel.GetMessageAsync(Vars.menuBuilder.EditMenuID);

            Console.WriteLine("Getting kink to update");

            var kinkToUpdate = Vars.menuBuilder.KinksToUpdate.Where(x => x.EmojiName.Equals("")).FirstOrDefault();

            Console.WriteLine("Adding data to kink");

            kinkToUpdate.EmojiName = reaction.Emote.ToString();
            kinkToUpdate.ServerID = Vars.menuBuilder.ServerID;

            Console.WriteLine("Stored emote name - " + kinkToUpdate.EmojiName);
            Console.WriteLine("Emote - No name dropped into string - " + reaction.Emote);

            Console.WriteLine("Parsing same way for both types:");
            Console.WriteLine("Setting emojiMenuString to message content");

            string emojiMenuString = emojiMenuMsg.Content;

            Console.WriteLine("Adding emoji and kink to emoji menu string");

            emojiMenuString += reaction.Emote + " " + kinkToUpdate.KinkName + " - " + kinkToUpdate.KinkDesc + "\n" + "\n​";

            Console.WriteLine("Updating menu message");

            await emojiMenuMsg.ModifyAsync(x => x.Content = emojiMenuString);

            Console.WriteLine("Addding emoji");

            await emojiMenuMsg.AddReactionAsync(reaction.Emote);



            /*
            Console.WriteLine("Hopefully getting the goddamn guild channel");

            var aGuildChannel = reaction.Channel as SocketGuildChannel;

            Console.WriteLine("Hopefully getting the goddamn emote");

            Emote anEmoji = aGuildChannel.Guild.Emotes.Where(x => x.Name == kinkToUpdate.EmojiName).FirstOrDefault();

            
            if (anEmoji != null)
            {
                Console.WriteLine("Parsing first way");

                Console.WriteLine("Setting emoji to string");

                string anEmojiString = anEmoji.ToString();

                Console.WriteLine("Setting emojiMenuString to message content");

                string emojiMenuString = emojiMenuMsg.Content;

                Console.WriteLine("Adding emoji and kink to emoji menu string");

                emojiMenuString += anEmojiString + " " + kinkToUpdate.KinkName + " - " + kinkToUpdate.KinkDesc + "\n" + "\n​";

                Console.WriteLine("Updating menu message");

                await emojiMenuMsg.ModifyAsync(x => x.Content = emojiMenuString);

                Console.WriteLine("Addding emoji");

                await emojiMenuMsg.AddReactionAsync(anEmoji);

            }
            else
            {
                Console.WriteLine("Parsing second way");

                Console.WriteLine("Setting emojiMenuString to message content");

                string emojiMenuString = emojiMenuMsg.Content;

                Console.WriteLine("Adding emoji and kink to emoji menu string");

                emojiMenuString += reaction.Emote + " " + kinkToUpdate.KinkName + " - " + kinkToUpdate.KinkDesc + "\n" + "\n​";

                Console.WriteLine("Updating menu message");

                await emojiMenuMsg.ModifyAsync(x => x.Content = emojiMenuString);


                Console.WriteLine("Addding emoji");

                await emojiMenuMsg.AddReactionAsync(reaction.Emote);


            }

            */



            /*
            bool parseWorks = false;
            if (anEmoji == null)
            {
                parseWorks = Emote.TryParse(kinkToUpdate.EmojiName, out anEmoji);
                Console.WriteLine("Parsing first way");
            }
            if (parseWorks == false)
            {
                parseWorks = Emote.TryParse(":" + kinkToUpdate.EmojiName + ":", out anEmoji);
                Console.WriteLine("Parsing second way");
            }
            if (parseWorks == false)
            {
                Console.WriteLine("Fucked both ways lol");
            }
            */






            string editMenuString = editMenuMsg.Content;

            string please = "​\nPlease";

            if (editMenuString.IndexOf(please) > -1)
            {
                editMenuString = editMenuString.Remove(editMenuString.IndexOf(please));
            }



            var kinksNotupdated = Vars.menuBuilder.KinksToUpdate.Where(x => x.EmojiName.Equals(""));

            if(kinksNotupdated.Count() > 0)
            {
                string firstKinkName = kinksNotupdated.FirstOrDefault().KinkName;

                editMenuString += "​\n" + "Please react to THIS post with the emoji for: " + firstKinkName + "\n​";

                await Task.Delay(2000);

                await editMenuMsg.ModifyAsync(x => x.Content = editMenuString);

            }
            else
            {
                editMenuString += "​\n" + "Finally time to try and write everything to the database!" + "\n​";

                // time to call some sort of write KinkEmojiData method

                await DataMethods.AddKinkMenu();

                await Task.Delay(2000);

                await editMenuMsg.ModifyAsync(x => x.Content = editMenuString);

                WipeMenuBuilder();


            }

            

        }


        private void WipeMenuBuilder()
        {
            Vars.menuBuilder.IsActive = false;
            Vars.menuBuilder.EmojiMenuID = 0;
            Vars.menuBuilder.EditMenuID = 0;
            Vars.menuBuilder.ChannelID = 0;
            Vars.menuBuilder.ServerID = 0;
            Vars.menuBuilder.UserID = 0;
            Vars.menuBuilder.KinkGroupName = "";
            Vars.menuBuilder.CommandStep = 0;
            Vars.menuBuilder.IsLimitMenu = false;
            Vars.menuBuilder.KinksToUpdate = null;
        }




    }
}
