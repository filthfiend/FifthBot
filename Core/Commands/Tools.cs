using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

using FifthBot.Core.Data;
using FifthBot.Core.Utils;
using FifthBot.Resources.Database;
using FifthBot.Resources.Preconditions;


namespace FifthBot.Core.Commands
{
    public class Tools : ModuleBase<SocketCommandContext>
    {

        /*
        [Command("search"), Alias("searchtags"), Summary("Search all of a user's Discord tags\n" + "usage - !!search tagname \"tag name\" \"multi word tag name\" singlewordtagname")]
        [RequireOwner(Group = "Permission")]
        //[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        //[RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        //[RequireRole(name: "Testers", Group = "Permission")]
        [RequireRole(name: "Sinners", Group = "Permission")]
        [RequireChannel(name: "search", Group ="Channels")]
        [RequireChannel(name: "bot", Group = "Channels")]
        public async Task Search(params string[] text)
        {

            string memberRole = "Sinners";

            SocketGuildUser commandUser = Context.User as SocketGuildUser;
            if (!commandUser.Roles.Any(x => x.Name.Equals(memberRole, StringComparison.OrdinalIgnoreCase)))
            {
                await Context.Channel.SendMessageAsync("Sorry, you may not use the search command!");
                return;
            }


            string reslut = "";
            string reslut2 = "​\nYour search terms:\n\n";

            foreach (string term in text)
            {
                reslut2 += term + ", ";
            }

            reslut2 = reslut2.Remove(reslut2.LastIndexOf(","));
            reslut2 += "\n\n";



            reslut2 += "Search Resluts:\n" + "━━━━━━━\n\n";
            reslut2 += "Note: please respect the Ask to DM tag by asking users who have it in the #Ask to DM channel before you DM them.\n\n";
            List<SocketGuildUser> userList = new List<SocketGuildUser>();
            bool start = true;
            string noDMs = "DMs Closed";
            string nonSearch = "Non Searchable";
            string askDM = "Ask to DM";

            string[] text1 = text;
            foreach (string searchParam in text1)
            {
                Console.WriteLine("searching for - " + searchParam);
                if (start)
                {
                    userList = Context.Guild.Users.Where(x => x.Roles.Any(y => y.Name.Equals(searchParam, StringComparison.OrdinalIgnoreCase))).ToList();
                    start = false;



                }
                else if (userList.Count() > 0)
                {
                    userList = userList.Where(x => x.Roles.Any(y => y.Name.Equals(searchParam, StringComparison.OrdinalIgnoreCase))).ToList();

                }
            }
            if (userList.Count() > 0)
            {
                userList.RemoveAll(x => x.Roles.Any(y => y.Name.Equals(noDMs, StringComparison.OrdinalIgnoreCase)));
                userList.RemoveAll(x => x.Roles.Any(y => y.Name.Equals(nonSearch, StringComparison.OrdinalIgnoreCase)));



            }

            // set resluts
            if (userList.Count() > 0)
            {
                reslut += "DM sent.";

                foreach (SocketGuildUser userFound in userList)
                {

                    reslut2 += userFound.Username;
                    reslut2 += "#";
                    reslut2 += userFound.Discriminator;



                    //reslut2 += userFound.Mention;

                    if (userFound.Roles.Any(x => x.Name.Equals(askDM, StringComparison.OrdinalIgnoreCase)))
                    {
                        reslut2 += " - Ask to DM";
                    }

                    reslut2 += "\n";

                }



            }
            else
            {
                reslut = "No resluts found!";
                reslut2 += "No resluts found!";
            }

            reslut2 += "\n​";

            //List<string> slutChunks = new List<string>();



            while (reslut2.Length > 2)
            {

                int subLength = 2000;

                if (subLength > reslut2.Length)
                {
                    subLength = reslut2.Length;
                }

                string tempSlut = reslut2.Substring(0, subLength);

                tempSlut = tempSlut.Substring(0, tempSlut.LastIndexOf("\n"));

                Console.WriteLine(tempSlut);

                reslut2 = reslut2.Remove(0, tempSlut.Length);

                Console.WriteLine(tempSlut);

                tempSlut += "\n​";

                await Context.User.SendMessageAsync(tempSlut);

            }



            await Context.Channel.SendMessageAsync(reslut);

        }

        */





        [Command("sinsearch"), Alias("search", "ss", "sinnersearch"), Summary("Search Den of Sinners kinks and limits in addition to Discord tags\n" + 
            "usage - !!sinsearch tagname \"tag name\" \"multi word tag name\" singlewordtagname")]
        [RequireOwner(Group = "Permission")]
        //[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        //[RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        //[RequireRole(name: "Testers", Group = "Permission")]
        [RequireRole(name: "Sinners", Group = "Permission")]
        [RequireChannel(name: "search", Group = "Channels")]
        [RequireChannel(name: "bot", Group = "Channels")]
        [RequireDM(Group = "Channels")]
        public async Task SinSearch(params string[] text)
        {


            if ((text == null || text.Length < 1))
            {

                await CommandStatus("No search terms given", "Sinsearch");
                return;
                
            }

            string termsString = string.Join(",", text);


            (string[] isAKink, string[] isNotAKink) = DataMethods.ValidateKinkNames(text);

            Console.WriteLine(" Completing the validate kink names method ");

            List<ulong> listOfUserIDs = new List<ulong>();
            if (isAKink != null && isAKink.Length > 0)
            {
                listOfUserIDs = DataMethods.GetUserIDsFromKinknames(isAKink);
            }

            Console.WriteLine(" Completing the user IDs from kinknames method ");

            List<string> listOfDiscordTags = new List<string>();
            if (isNotAKink != null && isNotAKink.Length > 0)
            {
                listOfDiscordTags = isNotAKink.ToList();
            }

            Console.WriteLine(" Completed setting discord tags to list ");


            if ((listOfUserIDs == null || listOfUserIDs.Count < 1) && (listOfDiscordTags == null || listOfDiscordTags.Count < 1))
            {
                await CommandStatus("No users found", "Sinsearch - " + termsString);

                return;
            }

            Console.WriteLine(" Completing the check for whether either list is null or zero ");

            string reslut2 = "​\nYour search terms:\n━━━━━━━\n\n";

            foreach (string term in text)
            {
                reslut2 += term + ", ";
            }

            reslut2 = reslut2.Remove(reslut2.LastIndexOf(","));
            reslut2 += "\n\n";

            


            List<SocketGuildUser> userList = new List<SocketGuildUser>();
            string noDMs = "DMs Closed";
            string nonSearch = "Non Searchable";
            string askDM = "Ask to DM";

            if (listOfUserIDs != null && listOfUserIDs.Count > 0)
            {

                userList = Context.Guild.Users.Where(user => listOfUserIDs.Any(dbUserID => user.Id == dbUserID)).ToList();

                /*
                foreach (ulong userIdent in listOfUserIDs)
                {
                    if (Context.Guild.GetUser(userIdent) != null)
                    {
                        userList.Add(Context.Guild.GetUser(userIdent));
                    }

                }
                */
            }

            Console.WriteLine("Completed getting users from userlist by dbtags");


            if ((isAKink == null || isAKink.Length < 1) && (listOfDiscordTags != null && listOfDiscordTags.Count() > 0))
            {
                Console.WriteLine("entering the discord tag if get");

                Console.WriteLine("discord tag array zero position - " + listOfDiscordTags[0]);
                userList = Context.Guild.Users.Where(user => user.Roles.Any(userRoles => userRoles.Name.Equals(listOfDiscordTags[0], StringComparison.OrdinalIgnoreCase))).ToList();

                Console.WriteLine("completed filling userlist");
                listOfDiscordTags.RemoveAt(0);
                Console.WriteLine("completed removing tag from userlist");

            }

            Console.WriteLine("Completed getting users from userlist by discord tags");

            if (listOfDiscordTags != null && listOfDiscordTags.Count() > 0)
            {
                if (userList != null && userList.Count() > 0)
                {
                    userList = userList.Where(aUser => listOfDiscordTags.All(discordTag => aUser.Roles.Any(userRole => userRole.Name.Equals(discordTag, StringComparison.OrdinalIgnoreCase)))).ToList();
                }

            }

            int resultCount = userList.Count();

            Console.WriteLine("Completed flushing users from userlist by discord tags");



            if (userList != null && userList.Count() > 0)
            {
                userList.RemoveAll(x => x.Roles.Any(y => y.Name.Equals(noDMs, StringComparison.OrdinalIgnoreCase)));
                userList.RemoveAll(x => x.Roles.Any(y => y.Name.Equals(nonSearch, StringComparison.OrdinalIgnoreCase)));



            }

            int resultMinusExcludes = userList.Count();

            reslut2 += "Search Resluts:\n" + "━━━━━━━\n";
            reslut2 += "Total - " + resultCount + ", Total minus nodm/nosearch - " + resultMinusExcludes + "\n";
            reslut2 += "Note: please respect the Ask to DM tag by asking users who have it in the #Ask to DM channel before you DM them.\n\n";

            Console.WriteLine("Completed flushing users from userlist by exclusion tags");


            if (userList.Count() > 0)
            {


                foreach (SocketGuildUser userFound in userList)
                {

                    reslut2 += userFound.Username;
                    reslut2 += "#";
                    reslut2 += userFound.Discriminator;



                    //reslut2 += userFound.Mention;

                    if (userFound.Roles.Any(x => x.Name.Equals(askDM, StringComparison.OrdinalIgnoreCase)))
                    {
                        reslut2 += " - Ask to DM";
                    }

                    reslut2 += "\n";

                }



            }
            else
            {
                await CommandStatus("No resluts found", "Sinsearch - " + termsString);

                return;
            }


            reslut2 += "\n​";

            await DmSplit(reslut2);

            await CommandStatus("DM sent.", "Sinsearch - " + termsString);



        }



        [Command("mysins"), Alias("mykinks", "mylimits"), Summary("Get a list of your own kinks and limits")]
        [RequireOwner(Group = "Permission")]
        //[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        //[RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        //[RequireRole(name: "Testers", Group = "Permission")]
        [RequireRole(name: "Sinners", Group = "Permission")]
        [RequireChannel(name: "search", Group = "Channels")]
        [RequireChannel(name: "bot", Group = "Channels")]
        [RequireDM(Group = "Channels")]
        public async Task MySins(params string[] text)
        {
            // var userKinks = DataMethods.GetUserKinks(Context.User.Id);
            // var userLimits = DataMethods.GetUserLimits(Context.User.Id);

            var userKinks = DataMethods.GetUserKinksAndLimits(Context.User.Id);


            if (userKinks == null)
            {

                await CommandStatus("You appear to have no kinks or limits", "MySins");

                return;
            }

            string dmString = "​\n" + "**" + "Your Kinks:" + "\n━━━━━━━" + "**" + "\n\n​";

            foreach (var gk in userKinks.Where(x => !x.isLimit))
            {
                dmString += gk.Group.KinkGroupName + "\n━━━━━━━" + "\n\n​";

                foreach (var k in gk.KinksForGroup)
                {
                    dmString += k.KinkName /*+ " - " + k.KinkDesc*/ + "\n";



                }

                dmString += "\n​";

            }

            dmString += "​\n" + "**" + "Your Limits:" + "\n━━━━━━━" + "**" + "\n\n​";

            foreach (var gk in userKinks.Where(x => x.isLimit))
            {
                dmString += gk.Group.KinkGroupName + "\n━━━━━━━" + "\n\n​";

                foreach (var k in gk.KinksForGroup)
                {
                    dmString += k.KinkName /*+ " - " + k.KinkDesc*/ + "\n";



                }

                dmString += "\n​";

            }

            await DmSplit(dmString);


            await CommandStatus("DM Sent", "MySins");


            /*
            var dmSentMsg = await Context.Channel.SendMessageAsync("DM Sent");

            await Task.Delay(1500);
            // if this is NOT a dm, we want to try to delete the messages


            if (Context.Message.Channel.GetType() != typeof(SocketDMChannel))
            {
                

                await Context.Message.DeleteAsync();
                await Context.Channel.DeleteMessageAsync(dmSentMsg);
            }
            */


        }



        [Command("sins"), Alias("userkinks", "userlimits"), Summary("Get a list of another user's kinks and limits using their username and identifier\n" 
            + "usage - !!sins username#2105 || !!sins \"multi word user name#5823\"")]
        [RequireOwner(Group = "Permission")]
        //[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        //[RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        //[RequireRole(name: "Testers", Group = "Permission")]
        [RequireRole(name: "Sinners", Group = "Permission")]
        [RequireChannel(name: "search", Group = "Channels")]
        [RequireChannel(name: "bot", Group = "Channels")]
        [RequireDM(Group = "Channels")]
        public async Task Sins([Remainder]string userNameAndNums)
        {
            if(userNameAndNums == null || userNameAndNums.Length < 6)
            {
                await CommandStatus("Invalid Username", "Sins");

                return;

            }


            string strBegin = userNameAndNums.Substring(0, 2);
            string strEnd = userNameAndNums.Substring(userNameAndNums.Length - 1, 1);

            if (strBegin == "<@" && strEnd == ">")
            {


                string toSend = "Do not ping using this command.\n"
                    + "Instead, use the person's user name and numbers without the @. Like [!!sins username#3838] or [!!sins multi word user name#9999]";

                await CommandStatus(toSend, "Sins");


                return;

            }



            string userName = userNameAndNums.Substring(0, userNameAndNums.Length - 5);

            string userNumString = userNameAndNums.Substring(userNameAndNums.Length - 4, 4);


            var user = Context.Guild.Users.Where(x => x.Username == userName && x.Discriminator == userNumString).FirstOrDefault();

            if (user == null)
            {
                await CommandStatus("No such user on this server.", "Sins - " + userNameAndNums);

                return;

            }

            if (user.Roles.Any(x => x.Name == "DMs Closed" || x.Name == "Non Searchable"))
            {

                await CommandStatus("That user's info is private.", "Sins - " + userNameAndNums);

                return;
            }


            ulong userID = user != null ? user.Id : 0;

            var userKinks = DataMethods.GetUserKinksAndLimits(userID);

            if (userKinks == null)
            {
                await CommandStatus("User appears to have no kinks or limits.", "Sins - " + userNameAndNums);

                return;
            }

            string dmString = "​\n" + "**" + userName + "\'s Kinks:" + "\n━━━━━━━" + "**" + "\n\n​";

            foreach (var gk in userKinks.Where(x => !x.isLimit))
            {
                dmString += gk.Group.KinkGroupName + "\n━━━━━━━" + "\n\n​";

                foreach (var k in gk.KinksForGroup)
                {
                    dmString += k.KinkName /*+ " - " + k.KinkDesc*/ + "\n";



                }

                dmString += "\n​";

            }

            dmString += "​\n" + "**" + userName + "\'s Limits:" + "\n━━━━━━━" + "**" + "\n\n​";

            foreach (var gk in userKinks.Where(x => x.isLimit))
            {
                dmString += gk.Group.KinkGroupName + "\n━━━━━━━" + "\n\n​";

                foreach (var k in gk.KinksForGroup)
                {
                    dmString += k.KinkName /*+ " - " + k.KinkDesc*/ + "\n";



                }

                dmString += "\n​";

            }
            await DmSplit(dmString);


            await CommandStatus("DM Sent", "Sins - " + userNameAndNums);


        }


        [Command("allsins"), Alias("allkinks", "listkinks", "lk", "as"), Summary("List all kinks in the database")]
        [RequireOwner(Group = "Permission")]
        //[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        //[RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        //[RequireRole(name: "Testers", Group = "Permission")]
        [RequireRole(name: "Sinners", Group = "Permission")]
        [RequireChannel(name: "search", Group = "Channels")]
        [RequireChannel(name: "bot", Group = "Channels")]
        [RequireDM(Group = "Channels")]
        public async Task ListKinks()
        {

            var kinksAndLimits = DataMethods.GetUserKinksAndLimits();

            if (kinksAndLimits == null)
            {
                await CommandStatus("There appear to be no kinks or limits", "listkinks");
                return;
            }

            string dmString = "​\n" + "**" + "All Kinks:" + "\n━━━━━━━" + "**" + "\n\n​";

            foreach (var gk in kinksAndLimits)
            {
                dmString += gk.Group.KinkGroupName + "\n━━━━━━━" + "\n\n​";

                foreach (var k in gk.KinksForGroup)
                {
                    dmString += k.KinkName + " - " + k.KinkDesc + "\n";

                }

                dmString += "\n​";

            }



            await DmSplit(dmString);
            await CommandStatus("DM Sent", "listkinks");


        }


        [Command("commandlist"), Alias("help"), Summary("list of commands")]
        [RequireOwner(Group = "Permission")]
        //[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        //[RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        //[RequireRole(name: "Testers", Group = "Permission")]
        [RequireRole(name: "Sinners", Group = "Permission")]
        public async Task Commandlist(params string[] parameters)
        {
            string dm = "​\nList of Commands\n"
                + "━━━━━━━━━━━━━━━━━━━━━━"
                + "\n\n";


            Type type = typeof(Tools);


            foreach (var method in type.GetMethods())
            {




                var attrs = System.Attribute.GetCustomAttributes(method);

                foreach (var attrib in attrs)
                {
                    if (attrib is CommandAttribute)
                    {
                        CommandAttribute a = (CommandAttribute)attrib;
                        dm += "!!" + a.Text + "\n";

                    }

                    if (attrib is AliasAttribute)
                    {
                        AliasAttribute a = (AliasAttribute)attrib;
                        dm += "aliases: ";
                        foreach (var alias in a.Aliases)
                        {
                            dm += "!!" + alias + ", ";
                        }
                        dm = dm.Remove(dm.Count() - 2);

                        dm += "\n";

                    }

                    if (attrib is SummaryAttribute)
                    {
                        SummaryAttribute a = (SummaryAttribute)attrib;
                        dm += a.Text + "\n";

                    }

                    if (attrib is RequireOwnerAttribute)
                    {
                        RequireOwnerAttribute a = (RequireOwnerAttribute)attrib;
                        dm += "Permitted users - Bot owner" + "\n";
                    }

                    if (attrib is RequireUserPermissionAttribute)
                    {
                        RequireUserPermissionAttribute a = (RequireUserPermissionAttribute)attrib;
                        dm += "Permitted users - ";
                        if (a.GuildPermission.Value == GuildPermission.Administrator)
                        {
                            dm += "Server Admins";
                        }

                        dm += "\n";
                    }

                    if (attrib is RequireRoleAttribute)
                    {
                        RequireRoleAttribute a = (RequireRoleAttribute)attrib;
                        dm += "Permitted users - " + a.name + "\n";
                    }



                }

                dm += "\n​";


            }
            dm += "\n\n​";
            await DmSplit(dm);
            await CommandStatus("DM Sent.", "Command List");



        }
















        [Command("addkink"), Alias("ak"), Summary("Add a kink to the database")]
        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [ExcludeDM]
        public async Task AddKink(params string[] kinkdata)
        {
            if (kinkdata != null && kinkdata.Length > 1)
            {
                string replyString = "Adding kink" + "\n"
                    + "Name: - " + kinkdata[0] + "\n"
                    + "Descrption: - " + kinkdata[1] + "\n";
                var replyMessage = await Context.Channel.SendMessageAsync(replyString);

                bool kinkAdded = await DataMethods.AddKink(kinkdata[0], kinkdata[1]);

                if(kinkAdded)
                {
                    replyString += "Kink added successfully!" + "\n";
                }
                else
                {
                    replyString += "Kink already in database, not added!" + "\n";
                }

                await replyMessage.ModifyAsync(x => x.Content = replyString);


                return;


            }

            string initMessage = "Welcome " + Context.User.Mention + "\n" +
                "New Kink Creation Step 1 - Enter Kink Name:";
            var replymessage = await Context.Channel.SendMessageAsync(initMessage);


            if (Vars.activeCommands.Where(x => x.ActorID == Context.User.Id).Count() > 0)
            {
                // eventually delete the old posts here too

                Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id);
            }


            var newCommand = new Command
            {
                CommandName = "addkink",
                CommandData = "start",
                ActorID = Context.User.Id,
                ChannelID = Context.Channel.Id,
                MessageID = replymessage.Id,

            };

            Vars.activeCommands.Add(newCommand);

            /*
            if (Vars.usersAddingKinks.Count > 0 && Vars.usersAddingKinks.Exists(x => x == Context.User.Id))
            {
                Vars.usersAddingKinks.RemoveAll(x => x == Context.User.Id);
            }


            Vars.usersAddingKinks.Add(Context.User.Id);







            await DataMethods.RemoveKinkAdder(Context.User.Id);
            await DataMethods.AddKinkAdder(replymessage.Id, Context.User.Id, Context.Channel.Id);
            */
        }



        [Command("editkink"), Alias("ek"), Summary("Edit a kink in the database")]
        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [ExcludeDM]
        public async Task EditKink()
        {
            string initMessage = "Welcome " + Context.User.Mention + "\n" + "Kink Editor - Enter name of Kink to Edit";
            var replymessage = await Context.Channel.SendMessageAsync(initMessage);

            // clear other active commands for this user out of the list
            if (Vars.activeCommands.Where(x => x.ActorID == Context.User.Id).Count() > 0)
            {
                // eventually delete the old posts here too

                Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id);
            }

            var newCommand = new Command
            {
                CommandName = "editkink",
                CommandData = "start",
                ActorID = Context.User.Id,
                ChannelID = Context.Channel.Id,
                MessageID = replymessage.Id,

            };

            Vars.activeCommands.Add(newCommand);



        }



        /*
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [Command("deletekink"), Alias("dk"), Summary("Delete a kink from the database")]
        public async Task EditKink(params string[] text)
        {

        }

        */










        [Command("addgroup"), Alias("ag"), Summary("Add a group to the database")]
        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [ExcludeDM]
        public async Task AddGroup()
        {


            string initMessage = "Welcome " + Context.User.Mention + "\n" +
                "New Group Creation Step 1 - Enter Group Name:";
            var replymessage = await Context.Channel.SendMessageAsync(initMessage);


            if (Vars.activeCommands.Where(x => x.ActorID == Context.User.Id).Count() > 0)
            {
                // eventually delete the old posts here too

                Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id);
            }


            var newCommand = new Command
            {
                CommandName = "addgroup",
                CommandData = "start",
                ActorID = Context.User.Id,
                ChannelID = Context.Channel.Id,
                MessageID = replymessage.Id,

            };

            Vars.activeCommands.Add(newCommand);


        }



        [Command("editgroup"), Alias("eg"), Summary("Edit a kink group in the database")]
        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [ExcludeDM]
        public async Task EditGroup()
        {
            string initMessage = "Welcome " + Context.User.Mention + "\n" + "Kink Group Editor - Enter name of group to edit:";
            var replymessage = await Context.Channel.SendMessageAsync(initMessage);

            // clear other active commands for this user out of the list
            if (Vars.activeCommands.Where(x => x.ActorID == Context.User.Id).Count() > 0)
            {
                // eventually delete the old posts here too

                Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id);
            }

            var newCommand = new Command
            {
                CommandName = "editgroup",
                CommandData = "start",
                ActorID = Context.User.Id,
                ChannelID = Context.Channel.Id,
                MessageID = replymessage.Id,

            };

            Vars.activeCommands.Add(newCommand);



        }


        [Command("groupkinks"), Alias("gk"), Summary("Add kinks to a group. Usage = !!groupkinks [\"Group Name\"] [kink1] [kink2] [\"kink 3\"] etc")]
        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [ExcludeDM]
        public async Task GroupKinks(params string[] parameters)
        {
            if (parameters.Length < 2)
            {
                await Context.Channel.SendMessageAsync("Insufficient data, quitting!");
                return;
            }

            List<string> kinkList = parameters.ToList();
            string groupName = kinkList[0];
            kinkList.RemoveAt(0);




            KinkGroup groupToJoin = DataMethods.GetGroup(groupName);
            if (groupToJoin == null)
            {
                await Context.Channel.SendMessageAsync("Invalid group, quitting!");
                return;
            }

            string adding = "Adding kinks to " + groupToJoin.KinkGroupName + "\n";

            foreach (string kinkName in kinkList)
            {
                bool kinkFound = await DataMethods.AddKinkToGroup(groupToJoin.KinkGroupID, kinkName, Context.Guild.Id);

                if (kinkFound)
                {
                    adding += kinkName + " added\n";

                }
                else
                {
                    adding += kinkName + " is not a valid kink\n";
                }

            }


            await Context.Channel.SendMessageAsync(adding);


        }


        [Command("creategroupmenu"), Alias("cgm"), Summary("Create a menu for a group")]
        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [ExcludeDM]
        public async Task CreateGroupMenu(params string[] parameters)
        {

            string emojiMenuMsg = "​\n" + "Emoji Menu" + "\n​";
            string editMenuMsg = "​\n" + "Edit Menu" + "\n​";

            editMenuMsg += "Enter the name of the group you want to make a menu for." + "\n​";

            var menuMessage = await Context.Channel.SendMessageAsync(emojiMenuMsg);
            var menuEditMessage = await Context.Channel.SendMessageAsync(editMenuMsg);

            /*
            if (!Vars.menuBuilder.IsActive)
            {
                await Context.Channel.SendMessageAsync("Fartz (variable is false)");

            }
            */

            Vars.menuBuilder.EmojiMenuID = menuMessage.Id;
            Vars.menuBuilder.EditMenuID = menuEditMessage.Id;
            Vars.menuBuilder.ChannelID = Context.Channel.Id;
            Vars.menuBuilder.UserID = Context.User.Id;
            Vars.menuBuilder.ServerID = Context.Guild.Id;
            Vars.menuBuilder.CommandStep = 0;
            Vars.menuBuilder.IsActive = true;


        }


        [Command("editgroupmenu"), Alias("egm"), Summary("Edit the menu for a group")]
        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [ExcludeDM]
        public async Task EditGroupMenu(ulong postID, params string[] parameters)
        {
            //var postToEdit = await Context.Channel.GetMessageAsync(postID) as SocketMessage;

            (KinkGroupMenu menuRecord, string groupName) = DataMethods.getMenuRecord(postID, Context.Guild.Id);

            if (menuRecord == null)
            {
                await Context.Channel.SendMessageAsync("Invalid post");
                return;
            }
            bool isLimit = false;
            if(menuRecord.LimitMsgID == postID)
            {
                isLimit = true;
            }

            if  (
                    (isLimit && menuRecord.LimitChannelID != Context.Channel.Id) ||
                    (!isLimit && menuRecord.KinkChannelID != Context.Channel.Id)
                )
            {
                await Context.Channel.SendMessageAsync("Wrong channel");
                return;
            }

            string limitOrKink = isLimit ? "Limit" : "Kink";
            string editMenuMsg = "Editing the " + limitOrKink + " menu for Kink Group - " + groupName + "\n\n​";

            editMenuMsg += "Do you want to reuse existing data or start from scratch? Reuse, Scratch, or anything else to quit." + "\n\n​";

            var menuEditMessage = await Context.Channel.SendMessageAsync(editMenuMsg);

            /*
            if (!Vars.menuBuilder.IsActive)
            {
                await Context.Channel.SendMessageAsync("Fartz (variable is false)");

            }
            */

            Vars.menuBuilder.EmojiMenuID = isLimit ? menuRecord.LimitMsgID : menuRecord.KinkMsgID;
            Vars.menuBuilder.EditMenuID = menuEditMessage.Id;
            Vars.menuBuilder.ChannelID = Context.Channel.Id;
            Vars.menuBuilder.UserID = Context.User.Id;
            Vars.menuBuilder.ServerID = Context.Guild.Id;
            Vars.menuBuilder.CommandStep = 2;
            Vars.menuBuilder.IsActive = true;
            Vars.menuBuilder.IsLimitMenu = isLimit;
            Vars.menuBuilder.KinkGroupName = groupName;
            Vars.menuBuilder.KinkGroupID = menuRecord.KinkGroupID;
            Vars.menuBuilder.KinksToUpdate = DataMethods.GetKinksInGroupWithEmojis(menuRecord.KinkGroupID, Context.Guild.Id);

        }

        private async Task CommandStatus (string statusMsgStr, string secretMsgStr)
        {

            await CommandStatus(statusMsgStr);

            var secretChannel = Context.Guild.TextChannels.Where(x => x.Name == "searchdata").FirstOrDefault();
            string finalSecretMsgStr = secretMsgStr + " - " + statusMsgStr + " - " + DateTime.Now;
            await secretChannel.SendMessageAsync(finalSecretMsgStr);

        }
        private async Task CommandStatus(string statusMsgStr)
        {
            var noDMSentMsg = await Context.Channel.SendMessageAsync(statusMsgStr);

            await Task.Delay(1500);

            await DeletePairIfNotDM(noDMSentMsg);
        }
        private async Task DeletePairIfNotDM(RestUserMessage postMsg)
        {
            if (Context.Message.Channel.GetType() != typeof(SocketDMChannel))
            {
                await Context.Message.DeleteAsync();
                await Context.Channel.DeleteMessageAsync(postMsg);
            }
        }

        private async Task DmSplit(string dmString)
        {
            while (dmString.Length > 2)
            {

                int subLength = 2000;

                if (subLength > dmString.Length)
                {
                    subLength = dmString.Length;
                }

                string tempSlut = dmString.Substring(0, subLength);

                tempSlut = tempSlut.Substring(0, tempSlut.LastIndexOf("\n"));

                dmString = dmString.Remove(0, tempSlut.Length);

                tempSlut += "\n​";

                await Context.User.SendMessageAsync(tempSlut);

            }
        }



    }
}
