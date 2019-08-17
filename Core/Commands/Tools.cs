using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using FifthBot.Core.Data;
using FifthBot.Core.Utils;
using FifthBot.Resources.Database;
using FifthBot.Resources.Preconditions;


namespace FifthBot.Core.Commands
{
    public class Tools : ModuleBase<SocketCommandContext>
    {

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [RequireRole(name: "Sinners", Group = "Permission")]
        [RequireChannel(name: "search")]
        [Command("search"), Alias("searchtags"), Summary("Search all of a user's tags!")]
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

            reslut2 = reslut2.Remove(reslut2.LastIndexOf(",") );
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
            if(userList.Count() > 0 )
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
                
                tempSlut = tempSlut.Substring(0, tempSlut.LastIndexOf("\n") );

                Console.WriteLine(tempSlut);

                reslut2 = reslut2.Remove(0, tempSlut.Length);

                Console.WriteLine(tempSlut);

                tempSlut += "\n​";

                await Context.User.SendMessageAsync(tempSlut);

            }



            await Context.Channel.SendMessageAsync(reslut);
            
        }

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [Command("addkink"), Alias("ak"), Summary("Add a kink to the database")]
        public async Task AddKink()
        {

            
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

        
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [Command("editkink"), Alias("ek"), Summary("Edit a kink in the database")]
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





        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [Command("listkinks"), Alias("lk"), Summary("List all kinks in the database")]
        public async Task ListKinks()
        {
            List<Kink> listOfKinks = DataMethods.GetKinkList();
            List<KinkGroup> listOfGroups = DataMethods.GetGroupList();

            listOfKinks = listOfKinks.OrderBy(x => x.KinkName).ToList();
            listOfKinks = listOfKinks.OrderBy(x => x.KinkGroupID).ToList();
            listOfGroups = listOfGroups.OrderBy(x => x.KinkGroupName).ToList();

            string toPrint = "​\nList of Kinks:\n\n";
            foreach (var group in listOfGroups)
            {

                toPrint += group.KinkGroupName + " - " + group.KinkGroupDescrip 
                    + "\n" + "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n";

                List<Kink> kinksInGroup = listOfKinks.Where(x => x.KinkGroupID == group.KinkGroupID).ToList();

                foreach (Kink kink in kinksInGroup)
                {
                    toPrint += kink.KinkName + " - " + kink.KinkDesc + "\n";
                }

                toPrint += "\n\n​";

            }

            toPrint += "Ungrouped Kinks" + "\n"
                    + "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n";

            List<Kink> ungroupedKinks = listOfKinks.Where(x => x.KinkGroupID == 0).ToList();
            foreach (Kink kink in ungroupedKinks)
            {
                toPrint += kink.KinkName + " - " + kink.KinkDesc + "\n";
            }

            toPrint += "\n";


            while (toPrint.Length > 2)
            {

                int subLength = 2000;

                if (subLength > toPrint.Length)
                {
                    subLength = toPrint.Length;
                }

                string tempSlut = toPrint.Substring(0, subLength);

                tempSlut = tempSlut.Substring(0, tempSlut.LastIndexOf("\n"));

                toPrint = toPrint.Remove(0, tempSlut.Length);

                tempSlut += "\n";

                await Context.User.SendMessageAsync(tempSlut);

            }



            await Context.Channel.SendMessageAsync("DM Sent");

        }




        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [Command("addgroup"), Alias("ag"), Summary("Add a group to the database")]
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


        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [Command("editgroup"), Alias("eg"), Summary("Edit a kink group in the database")]
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

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [Command("groupkinks"), Alias("gk"), Summary("Add kinks to a group. Usage = !!groupkinks [\"Group Name\"] [kink1] [kink2] [\"kink 3\"] etc")]
        public async Task GroupKinks(params string[] parameters)
        {
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

            foreach(string kinkName in kinkList)
            {
                bool kinkFound = await DataMethods.AddKinkToGroup(groupToJoin.KinkGroupID, kinkName);

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

        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [Command("commandlist"), Alias("help"), Summary("list of commands")]
        public async Task commandlist(params string[] parameters)
        {
            string dm = "​\nList of Commands\n" 
                + "━━━━━━━━━━━━━━━━━━━━━━"
                + "\n\n";


            Type type = typeof(Tools);


            foreach (var method in type.GetMethods())
            {
                



                var attrs = System.Attribute.GetCustomAttributes(method);

                foreach(var attrib in attrs)
                {
                    if(attrib is CommandAttribute)
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
                        dm += a.Text + "\n\n";

                    }



                }


                /*
                var myNotes = method.GetCustomAttributes(inherit: false );
                foreach(var attrib in myNotes)
                {
                    Console.WriteLine(" adding method attribute ");
                    dm += "method attribute - " + attrib.ToString() + "\n";
                }

                
                var myNoteData = method.GetCustomAttributesData();
                foreach(var myData in myNoteData)
                {
                    Console.WriteLine(" adding method attribute data");
                    dm += "attribute data - " + myData.ToString() + "\n";


                    var myNamedArguments = myData.NamedArguments;
                    ////
                    foreach(var namedArg in myNamedArguments)
                    {
                        namedArg.
                    }
                    ////

                }
                */




            }
            dm += "\n\n​";

            await Context.Channel.SendMessageAsync("DM Sent.");
            await Context.User.SendMessageAsync(dm);
            
        }


        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [RequireOwner(Group = "Permission")]
        [RequireRole(name: "Seven Deadly Sins", Group = "Permission")]
        [RequireRole(name: "Testers", Group = "Permission")]
        [Command("creategroupmenu"), Alias("cgm"), Summary("Create a menu for a group")]
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







    }
}
