using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FifthBot.Resources.Datatypes;
using FifthBot.Resources.Settings;
using FifthBot.Core.Utils;
using FifthBot.Core.Data;

namespace FifthBot.Core.Commands
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {

        /*
        [Command("mute"), Summary("silence the naughty")]
        public async Task Mute(IUser Target = null, int delayMins = 1)
        {
            //ServerSetting serverSettings = SetSettings.get(Context.Guild.Id);

            // check if command user is administrator

            SocketGuildUser ServerUser = Context.User as SocketGuildUser;
            if (!ServerUser.GuildPermissions.Administrator)
            {
                await Context.Channel.SendMessageAsync($":x: You don't have administrator permissions in this discord server! Ask an administrator or the owner to execute this command!");
                return;
            }

            // check if target is valid

            if (Target == null)
            {
                await Context.Channel.SendMessageAsync($"Who were you trying to mute, {Context.User.Mention}?");
                return;
            }

            // checks if role is valid

            if (Context.Guild.Roles.Where(x => x.Name == serverSettings.mutedRole).Count() < 1)
            {
                await Context.Channel.SendMessageAsync(":x: Muted role not found!");
                return;
            }


            
            IRole muteRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Muted");

            await (Target as IGuildUser).AddRoleAsync(muteRole);

            int delaySecs = delayMins * 60;

            int delay = delaySecs * 1000;

            await Context.Channel.SendMessageAsync($"{Target.Mention} muted by {Context.User.Mention}. Unmuting in {delayMins} minutes");



            await Task.Delay(delay);

            await Context.Channel.SendMessageAsync($"Attempting to unmute {Target.Username}");

            await unMuteAfterTimer(Target, delayMins);




        }
        */

        /*
        private async Task unMuteAfterTimer(IUser target, int timerMinutes)
        {
            Util ut = new Util(Context);
            ServerSetting serverSettings = SetSettings.get(Context.Guild.Id);

            SocketGuildUser guildTarget = target as SocketGuildUser;
            if (guildTarget.Roles.Where(x => x.Name == serverSettings.mutedRole).Count() < 1)
            {

                // await ut.sma($"**{target.Username}#{target.Discriminator}** has already been unmuted");
                return;
            }

            IRole muteRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == serverSettings.mutedRole);

            // await guildTarget.RemoveRoleAsync(muteRole);
            await (target as IGuildUser).RemoveRoleAsync(muteRole);

            await ut.sma($"{target.Mention} un-muted after {timerMinutes} minutes!");
        }
        */

        /*
        [Command("unmute"), Summary("unmutes muted people!")]
        public async Task unMute(IUser target = null, bool onTimer = false)
        {
            Util ut = new Util(Context);
            ServerSetting serverSettings = SetSettings.get(Context.Guild.Id);

            // check if admin
            SocketGuildUser ServerUser = Context.User as SocketGuildUser;
            if (!ServerUser.GuildPermissions.Administrator)
            {
                await ut.sma($":x: You don't have administrator permissions in this discord server! Ask an administrator or the owner to execute this command!");
                return;
            }

            // check if target is valid
            if (target == null)
            {
                await ut.sma($"Who were you trying to mute, {Context.User.Mention}?");
                return;
            }

            // check if muted role is valid
            if (Context.Guild.Roles.Where(x => x.Name == serverSettings.mutedRole).Count() < 1)
            {
                await ut.sma($":x: **{serverSettings.mutedRole}** role not found!");
                return;
            }

            // check if muted or not

            SocketGuildUser guildTarget = target as SocketGuildUser;
            if (guildTarget.Roles.Where(x => x.Name == serverSettings.mutedRole).Count() < 1)
            {
                await ut.sma($"{target.Username}{target.Discriminator} is not muted!");
                return;
            }

            IRole muteRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == serverSettings.mutedRole);

            await (target as IGuildUser).RemoveRoleAsync(muteRole);

            await ut.sma($"{target.Mention} un-muted by {Context.User.Mention}");



        }
        */

        /*
        [Command("muteconfig"), Summary("Auto-sets channels to recognize the mute!")]
        public async Task muteConfig()
        {
            Util ut = new Util(Context);

            ServerSetting serverSettings = SetSettings.get(Context.Guild.Id);

            // check if command user is administrator

            SocketGuildUser ServerUser = Context.User as SocketGuildUser;
            if (!ServerUser.GuildPermissions.Administrator)
            {
                await ut.sma($":x: You don't have administrator permissions in this discord server! Ask an administrator or the owner to execute this command!");
                return;
            }
            if (Context.Guild.Roles.Where(x => x.Name == serverSettings.mutedRole).Count() < 1)
            {
                await ut.sma($"**{serverSettings.mutedRole}** role not found!");
                return;
            }

            IRole muteRole = Context.Guild.Roles.Where(x => x.Name == serverSettings.mutedRole).First();

            ///*
            if (muteRole == null)
            {
                await ut.sma("mute role not found!");
                return;
            }
            ////

            SocketGuildChannel[] channelsArray =   Context.Guild.Channels.ToArray();
            foreach (SocketGuildChannel thisChannel in channelsArray)
            {
                await thisChannel.AddPermissionOverwriteAsync(role: muteRole, perms: new OverwritePermissions
                (
                sendMessages: PermValue.Deny
                ));
            }
                
                ///*
                FirstOrDefault(x => x.Name == "general").AddPermissionOverwriteAsync(role: muteRole, perms: new OverwritePermissions
                (
                sendMessages: PermValue.Deny
                ));
                ////
               

            ///*

            IGuildChannel general = Context.Guild.Channels.FirstOrDefault(x => x.Name == "general");

            IRole roleToCheck = Context.Guild.Roles.FirstOrDefault(x => x.Name == "@everyone");

            OverwritePermissions? perms = general.GetPermissionOverwrite(roleToCheck);

            if (perms != null)
            {
                await Context.Channel.SendMessageAsync("General: " + perms.ToString());
            }

            IGuildChannel welcomes = Context.Guild.Channels.First(x => x.Name == "welcomes");

            perms = welcomes.GetPermissionOverwrite(roleToCheck);

            if (perms != null)
            {
                await Context.Channel.SendMessageAsync("Welcomes: " + perms.ToString());







            }
            ////

            //IRole muteRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Muted");


            ///*
            await Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "general").AddPermissionOverwriteAsync(role: muteRole, perms: new OverwritePermissions
                (
                sendMessages: PermValue.Deny
                ));
            ////
        }
        */
        [RequireOwner(Group = "Permission")]
        [Command("purgeintros"), Summary("purges intros")]
        public async Task PurgeIntros()
        {



            int interval = 50;
            Console.WriteLine("interval set");


            // check if command user is administrator

            SocketGuildUser ServerUser = Context.User as SocketGuildUser;
            if (!ServerUser.GuildPermissions.Administrator)
            {
                await Context.Channel.SendMessageAsync($":x: You don't have administrator permissions in this discord server! Ask an administrator or the owner to execute this command!");
                return;
            }

            Console.WriteLine("Trying to delete messages lol");
            await Context.Channel.SendMessageAsync("Trying to delete messages!");
            Console.WriteLine("Trying to delete messages lol 2");


            var GuildUsers = Context.Guild.Users;

            int messagesScanned = 0;
            int messagesDeleted = 0;

            //string[] channelNamesToPurge = serverSettings.introChannels.ToArray();

            ulong[] channelsToPurge = DataMethods.GetIntroChannelIDs(Context.Guild.Id);


            foreach(ulong channelToPurge in channelsToPurge)
            {
                int messagesScannedThisChannel = 0;
                int messagesDeletedThisChannel = 0;

                SocketTextChannel scannedChannel = Context.Guild.TextChannels.Where(x => x.Id == channelToPurge ).FirstOrDefault();

                var MessagesFromChannel = await Context.Guild.TextChannels.Where(x => x.Id == channelToPurge).FirstOrDefault().GetMessagesAsync(interval).FlattenAsync();

                ulong lastMsgID = 0;
                //IMessage bombTest = await Context.Guild.TextChannels.Where(x => x.Name == channelNameToPurge).FirstOrDefault().GetMessageAsync(lastID);

                //int msgCount = MessagesFromChannel.Count();

                //await Context.Channel.SendMessageAsync($"First messages from #{channelNameToPurge} supposedly loaded! Message count: {msgCount}");

                foreach (IMessage CurrentMsg in MessagesFromChannel)
                {
                    //await Context.Channel.SendMessageAsync($"Looping message!");
                    //bool userFound = false;

                    lastMsgID = CurrentMsg.Id;
                    messagesScanned++;
                    messagesScannedThisChannel++;

                    //await Context.Channel.SendMessageAsync($"Looping message - ID {lastID}!");

                    /*
                    foreach (SocketGuildUser CurrentUser in GuildUsers)
                    {
                        if (CurrentMsg.Author.Id == CurrentUser.Id)
                        {
                            userFound = true;
                        }
                    }
                    */

                    SocketGuildUser msgAuthor = Context.Guild.GetUser(CurrentMsg.Author.Id);

                    //if (!userFound)
                    if(msgAuthor == null)
                    {
                        await CurrentMsg.DeleteAsync();
                        messagesDeleted++;
                        messagesDeletedThisChannel++;
                    }

                }

                //await Context.Channel.SendMessageAsync($"First messages scanned in #{channelNameToPurge}!");

            
                while (true)
                {

                    IMessage currMessage = await Context.Guild.TextChannels.Where(x => x.Id == channelToPurge).FirstOrDefault().GetMessageAsync(lastMsgID);

                    //await Context.Channel.SendMessageAsync("We're in the while loop!");

                    if (currMessage == null)
                    {
                        var mgsToCount = await Context.Guild.TextChannels.Where(x => x.Id == channelToPurge).FirstOrDefault().GetMessagesAsync(limit: 1, fromMessageId: lastMsgID, dir: Direction.Before).FlattenAsync();
                        
                        int theCount = mgsToCount.Count();

                        //await Context.Channel.SendMessageAsync("currMessage is returning null!");

                        if (theCount < 1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        //messagesScanned--;
                        //messagesScannedThisChannel--;

                        var mgsToCount = await Context.Guild.TextChannels.Where(x => x.Id == channelToPurge).FirstOrDefault().GetMessagesAsync(limit: 2, fromMessageId: lastMsgID, dir: Direction.Before).FlattenAsync();
                        
                        int theCount = mgsToCount.Count();
                        

                        //await Context.Channel.SendMessageAsync($"currMessage is not returning null! count = {theCount}");

                        if ( theCount < 2)
                        {
                            break;
                        }
                    }

                    //await Context.Channel.SendMessageAsync($"Deleting from message ID: {lastID}");
                    MessagesFromChannel = await Context.Guild.TextChannels.Where(x => x.Id == channelToPurge).FirstOrDefault().GetMessagesAsync(limit: interval, fromMessageId: lastMsgID, dir: Direction.Before).FlattenAsync();

                    foreach (IMessage CurrentMsg in MessagesFromChannel)
                    {
                        //bool userFound = false;
                        
                        lastMsgID = CurrentMsg.Id;
                        messagesScanned++;
                        messagesScannedThisChannel++;
                        SocketGuildUser msgAuthor = Context.Guild.GetUser(CurrentMsg.Author.Id);

                        /*
                        foreach (SocketGuildUser CurrentUser in GuildUsers)
                        {
                            if (CurrentMsg.Author.Id == CurrentUser.Id)
                            {
                                userFound = true;
                            }
                        }
                        */

                        //if (!userFound)
                        if (msgAuthor == null)
                        {
                            await CurrentMsg.DeleteAsync();
                            messagesDeleted++;
                            messagesDeletedThisChannel++;
                        }

                    }
                }

                await Context.Channel.SendMessageAsync($"All messages scanned from {scannedChannel.Mention}! Channel total: {messagesScannedThisChannel} messages scanned, {messagesDeletedThisChannel} messages deleted!");

            }



            await Context.Channel.SendMessageAsync($"All messages scanned! Total: {messagesScanned} messages scanned, {messagesDeleted} messages deleted!");



        }

        /*
        [Command("markinactives"), Summary("Mark everyone who hasn't posted in X duration in the inactive usergroup")]
        public async Task markInactives()
        {
            Util ut = new Util(Context);
            ServerSetting serverSettings = SetSettings.get(Context.Guild.Id);
            string inactiveRoleName = "Inactive";

            // check if command user is administrator

            SocketGuildUser ServerUser = Context.User as SocketGuildUser;
            if (!ServerUser.GuildPermissions.Administrator)
            {
                await ut.sma($":x: You don't have administrator permissions in this discord server! Ask an administrator or the owner to execute this command!");
                return;
            }

            IRole inactiveRole = Context.Guild.Roles.Where(x => x.Name == inactiveRoleName).FirstOrDefault();
            if(inactiveRole == null)
            {
                await ut.sma("Role not found!");
                return;
            }

            await ut.sma($"**{inactiveRole.Name}** group found! We're in business!");

            List<SocketGuildUser> allUsers = Context.Guild.Users.ToList();
            List<SocketTextChannel> allChannels = Context.Guild.TextChannels.ToList();

            ///*
            foreach (SocketTextChannel thisChannel in allChannels)
            {
                await ut.sma($"{thisChannel.Mention}");
            }
            ////

            int totalInactive = 0;

            foreach(SocketGuildUser thisUser in allUsers)
            {
                await ut.sma($"Checking user {thisUser.Username}#{thisUser.Discriminator}!");

                if (thisUser.IsBot)
                {
                    continue;
                }
                 
                DateTime joinDate = thisUser.JoinedAt?.DateTime ?? DateTime.Now.AddHours(8);
                TimeSpan membershipLength = DateTime.Now.AddHours(8) - joinDate;
                if(membershipLength.TotalHours < 24 )
                {
                    continue;
                }

                bool validUserFound = false;
                
                foreach (SocketTextChannel thisChannel in allChannels)
                {
                    bool messageAgeLimitReached = false;

                    await ut.sma($"Now scanning channel {thisChannel.Mention}");

                    if (validUserFound)
                    {
                        break;
                    }

                    var someMessages = await thisChannel.GetMessagesAsync(1).Flatten();
                    if(someMessages.Count() < 1)
                    {
                        // await ut.sma($"No messages in {thisChannel.Name}!");

                        continue;
                    }

                    ulong messageID = someMessages.FirstOrDefault().Id;
                    DateTime messageTime;
                    DateTime now;
                    TimeSpan messageAge;

                    

                    while (true)
                    {
                        if (validUserFound)
                        {
                            break;
                        }
                        if (messageAgeLimitReached)
                        {
                            break;
                        }

                        someMessages = await thisChannel.GetMessagesAsync(messageID, Direction.Before, 100).Flatten();

                        if (someMessages.Count() < 1)
                        {
                            await ut.sma($"End of channel {thisChannel.Mention}");

                            break;
                        }

                        foreach (IMessage thisMessage in someMessages)
                        {

                            if (thisMessage.Author.Id == thisUser.Id)
                            {
                                messageTime = thisMessage.Timestamp.DateTime;
                                now = DateTime.Now.AddHours(8);
                                messageAge = now - messageTime;
                                if (messageAge.TotalDays < 30)
                                {
                                    validUserFound = true;
                                    break;
                                    
                                }
                            }

                            ///*
                            if (thisMessage == someMessages.Last())
                            {
                                messageID = someMessages.Last().Id;

                                messageTime = thisMessage.Timestamp.DateTime;
                                now = DateTime.Now.AddHours(8);
                                messageAge = now - messageTime;
                                if (messageAge.TotalDays > 30)
                                {
                                    messageAgeLimitReached = true;
                                    await ut.sma($"Searched 30 days back in {thisChannel.Mention}");
                                    break;
                                }

                            }
                            ////



                        }
                        

                        messageTime = someMessages.Last().Timestamp.DateTime;
                        now = DateTime.Now.AddHours(8);
                        messageAge = now - messageTime;
                        if (messageAge.TotalDays > 30)
                        {
                            messageAgeLimitReached = true;
                            await ut.sma($"Searched 30 days back in {thisChannel.Mention}");
                            break;
                        }

                        messageID = someMessages.Last().Id;

                        if (someMessages.Count() < 2)
                        {
                            await ut.sma($"End of channel {thisChannel.Mention}");

                            break;
                        }
                        

                    }

                }

                if (!validUserFound)
                {
                    await ut.sma($"Marking user {thisUser.Username}#{thisUser.Discriminator} inactive!");
                    await thisUser.AddRoleAsync(inactiveRole);
                    totalInactive++;

                }
                else
                {
                    await ut.sma($"{thisUser.Username}#{thisUser.Discriminator} is a valid user!");
                }

            }

            await ut.sma($"Wow, finished! {totalInactive} people were marked inactive, out of {allUsers.Count} total members!");

        }
        */
    }

}
