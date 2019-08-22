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
    public class UtilMethods
    {
        public async Task AddKinkData(SocketCommandContext Context)
        {
            

            ulong userID = Context.User.Id;

            Command kinkCommand = Vars.activeCommands.Where(x => x.ActorID == userID).FirstOrDefault();

            Console.WriteLine(" kink command data is -  " + kinkCommand.CommandData);

            /*
            Command kinkCommand = DataMethods.GetKinkAdder(Context.User.Id);
            */

            if (Context.Channel.Id != kinkCommand.ChannelID)
            {
                return;
            }

            if (kinkCommand.CommandData.Equals("start"))
            {
                string kinkName = Context.Message.Content;
                
                kinkCommand.CommandData = kinkName;

                //await DataMethods.UpdateKinkAdder(kinkCommand.CommandID, kinkName );

                string newMessage = "Welcome " + Context.User.Mention + "\n" + 
                    "New Kink Creation Step 2 - Enter Kink Description:";

                ulong msgToEditID = kinkCommand.MessageID;

                var msgToEdit = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToEditID, CacheMode.AllowDownload);

                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Context.Message.DeleteAsync();

            }
            else
            {
                Console.WriteLine(" entering 2nd else  ");

                string kinkDesc = Context.Message.ToString();
                string kinkName = kinkCommand.CommandData;

                Console.WriteLine(" attempting to add kink  ");

                bool kinkAdded = await DataMethods.AddKink(kinkName, kinkDesc);

                Console.WriteLine(" kink should be added  " + kinkCommand.CommandData);

                // need to remove description message
                // need to remove command list entry

                await Context.Message.DeleteAsync();

                Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id && x.CommandName == "addkink");

                /*
                // need to delete commands record
                // need to delete vars list entry
                Vars.usersAddingKinks.Remove(Context.User.Id);
                await DataMethods.RemoveKinkAdder(Context.User.Id);
                
                */

                ulong msgToDelID = kinkCommand.MessageID;

                var msgToDel = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToDelID);

                string newMessage = "Welcome " + Context.User.Mention + "\n";

                if(kinkAdded)
                {
                    newMessage += "New kink added - " + kinkName + " - " + kinkDesc;
                }
                else
                {
                    newMessage += "Kinkname in DB already, not added!";
                }


                await msgToDel.ModifyAsync(x => x.Content = newMessage);

                /*
                await Task.Delay(5000);

                await msgToDel.DeleteAsync();

                Console.WriteLine(" bot message deleted ");
                */



                

            }





            //return "fartz";
        }


        public async Task EditKinkData(SocketCommandContext Context)
        {
            ulong userID = Context.User.Id;

            Command kinkCommand = Vars.activeCommands.Where(x => x.ActorID == userID).FirstOrDefault();

            Console.WriteLine(" we've got our kink command ");


            Kink kinkToEdit = Vars.tableEntries.Where(x => x.GetType().Name == "Kink").ToList().Cast<Kink>().ToList().Where(x => x.KinkID.ToString() == kinkCommand.CommandData).FirstOrDefault();

            Console.WriteLine(" we've got our kink to edit ");



            if (kinkCommand.CommandData == "start")
            {
                string kinkName = Context.Message.Content;


                Kink kinkFromDB = DataMethods.GetKink(kinkName);


                if (kinkFromDB == null)
                {
                    ulong msgEndID = kinkCommand.MessageID;

                    var msgEnd = (RestUserMessage)await Context.Channel.GetMessageAsync(msgEndID);

                    string endMessage = "Welcome " + Context.User.Mention + "\n" + 
                        "Kink not found, quitting!";

                    await msgEnd.ModifyAsync(x => x.Content = endMessage);

                    Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id);


                    await Context.Message.DeleteAsync();

                    return;

                }

                Vars.tableEntries.Add(kinkFromDB);

                kinkCommand.CommandData = kinkFromDB.KinkID.ToString();
                kinkCommand.CommandStep = 1;

                string newMessage = "Welcome " + Context.User.Mention + "\n" + 
                    "Edit Kink Step 1: Enter New Name, or Fartz to skip";

                ulong msgToEditID = kinkCommand.MessageID;

                var msgToEdit = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToEditID, CacheMode.AllowDownload);

                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Context.Message.DeleteAsync();

            }

            else if (kinkToEdit != null && kinkCommand.CommandStep == 1)
            {
                string newKinkName = Context.Message.Content;

                if (!newKinkName.Equals("fartz", StringComparison.OrdinalIgnoreCase))
                {
                    kinkToEdit.KinkName = newKinkName;

                    Kink kinkToCheck = Vars.tableEntries.Where(x => x.GetType().Name == "Kink").ToList().Cast<Kink>().ToList().Where(x => x.KinkID.ToString() == kinkCommand.CommandData).FirstOrDefault();

                    if (kinkToEdit.KinkName != kinkToCheck.KinkName)
                    {
                        Console.WriteLine("Kink in edit list not updating");
                    }

                }

                kinkCommand.CommandStep = 2;

                string newMessage = "Welcome " + Context.User.Mention + "\n" + 
                    "Edit Kink Step 2: Enter New Description, or Fartz to skip";

                ulong msgToEditID = kinkCommand.MessageID;

                var msgToEdit = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToEditID, CacheMode.AllowDownload);

                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Context.Message.DeleteAsync();

            }

            else if (kinkToEdit != null && kinkCommand.CommandStep == 2)
            {
                string newKinkDescrip = Context.Message.Content;
                if (!newKinkDescrip.Equals("fartz", StringComparison.OrdinalIgnoreCase))
                {
                    kinkToEdit.KinkDesc = newKinkDescrip;

                    Kink kinkToCheck = Vars.tableEntries.Where(x => x.GetType().Name == "Kink").ToList().Cast<Kink>().ToList().Where(x => x.KinkID.ToString() == kinkCommand.CommandData).FirstOrDefault();

                    if (kinkToEdit.KinkDesc != kinkToCheck.KinkDesc)
                    {
                        Console.WriteLine("Kink in edit list not updating");
                    }

                }

                await Context.Message.DeleteAsync();


                string newMessage = "Welcome " + Context.User.Mention + "\n" + 
                    "Now updating entry with new Name and Description: \n"
                    + "Name: " + kinkToEdit.KinkName + "\n"
                    + "Desc: " + kinkToEdit.KinkDesc + "\n";


                ulong msgToEditID = kinkCommand.MessageID;

                var msgToEdit = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToEditID, CacheMode.AllowDownload);

                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await DataMethods.EditKink(kinkToEdit.KinkID, kinkToEdit.KinkName, kinkToEdit.KinkDesc);


                await Task.Delay(1000);

                newMessage += "\n.";
                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Task.Delay(1000);

                newMessage += ".";
                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Task.Delay(1000);

                newMessage += ".";
                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Task.Delay(1000);

                newMessage += " Done.";
                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id);

                Vars.tableEntries.RemoveAll(x => x.GetType().Name == "Kink" && (x as Kink).KinkID == kinkToEdit.KinkID);

            }


        }


        public async Task AddGroupData(SocketCommandContext Context)
        {


            ulong userID = Context.User.Id;

            Command groupCommand = Vars.activeCommands.Where(x => x.ActorID == userID).FirstOrDefault();

            Console.WriteLine(" kink command data is -  " + groupCommand.CommandData);

            /*
            Command kinkCommand = DataMethods.GetKinkAdder(Context.User.Id);
            */

            if (Context.Channel.Id != groupCommand.ChannelID)
            {
                return;
            }

            if (groupCommand.CommandData.Equals("start"))
            {
                string groupName = Context.Message.Content;

                groupCommand.CommandData = groupName;

                //await DataMethods.UpdateKinkAdder(kinkCommand.CommandID, kinkName );

                string newMessage = "Welcome " + Context.User.Mention + "\n" +
                    "New Group Creation Step 2 - Enter Group Description:";

                ulong msgToEditID = groupCommand.MessageID;

                var msgToEdit = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToEditID, CacheMode.AllowDownload);

                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Context.Message.DeleteAsync();

            }
            else
            {
                Console.WriteLine(" entering 2nd else  ");

                string groupDesc = Context.Message.ToString();
                string groupName = groupCommand.CommandData;

                Console.WriteLine(" attempting to add group  ");

                await DataMethods.AddGroup(groupName, groupDesc);

                Console.WriteLine(" group should be added  " + groupCommand.CommandData);

                // need to remove description message
                // need to remove command list entry

                await Context.Message.DeleteAsync();

                Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id && x.CommandName == "addgroup");


                ulong msgToDelID = groupCommand.MessageID;

                var msgToDel = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToDelID);

                string newMessage = "Welcome " + Context.User.Mention + "\n" +
                    "New group added - " + groupName + " - " + groupDesc;
                await msgToDel.ModifyAsync(x => x.Content = newMessage);






            }





            //return "fartz";
        }


        public async Task EditGroupData(SocketCommandContext Context)
        {
            ulong userID = Context.User.Id;

            Command groupCommand = Vars.activeCommands.Where(x => x.ActorID == userID).FirstOrDefault();

            Console.WriteLine(" we've got our group command ");


            KinkGroup groupToEdit = Vars.tableEntries.Where(x => x.GetType().Name == "KinkGroup").ToList().Cast<KinkGroup>().ToList().Where(x => x.KinkGroupID.ToString() == groupCommand.CommandData).FirstOrDefault();

            Console.WriteLine(" we've got our group to edit ");



            if (groupCommand.CommandData == "start")
            {
                string groupName = Context.Message.Content;


                KinkGroup groupFromDB = DataMethods.GetGroup(groupName);


                if (groupFromDB == null)
                {
                    ulong msgEndID = groupCommand.MessageID;

                    var msgEnd = (RestUserMessage)await Context.Channel.GetMessageAsync(msgEndID);

                    string endMessage = "Welcome " + Context.User.Mention + "\n" +
                        "Group not found, quitting!";

                    await msgEnd.ModifyAsync(x => x.Content = endMessage);

                    Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id);


                    await Context.Message.DeleteAsync();

                    return;

                }

                Vars.tableEntries.Add(groupFromDB);

                groupCommand.CommandData = groupFromDB.KinkGroupID.ToString();
                groupCommand.CommandStep = 1;

                string newMessage = "Welcome " + Context.User.Mention + "\n" +
                    "Edit Group Step 1: Enter New Name, or Fartz to skip";

                ulong msgToEditID = groupCommand.MessageID;

                var msgToEdit = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToEditID, CacheMode.AllowDownload);

                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Context.Message.DeleteAsync();

            }

            else if (groupToEdit != null && groupCommand.CommandStep == 1)
            {
                string newGroupName = Context.Message.Content;

                if (!newGroupName.Equals("fartz", StringComparison.OrdinalIgnoreCase))
                {
                    groupToEdit.KinkGroupName = newGroupName;

                    KinkGroup kinkToCheck = Vars.tableEntries.Where(x => x.GetType().Name == "KinkGroup").ToList().Cast<KinkGroup>().ToList().Where(x => x.KinkGroupID.ToString() == groupCommand.CommandData).FirstOrDefault();

                    if (groupToEdit.KinkGroupName != kinkToCheck.KinkGroupName)
                    {
                        Console.WriteLine("Kink in edit list not updating");
                    }

                }

                groupCommand.CommandStep = 2;

                string newMessage = "Welcome " + Context.User.Mention + "\n" +
                    "Edit Kink Step 2: Enter New Description, or Fartz to skip";

                ulong msgToEditID = groupCommand.MessageID;

                var msgToEdit = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToEditID, CacheMode.AllowDownload);

                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Context.Message.DeleteAsync();

            }

            else if (groupToEdit != null && groupCommand.CommandStep == 2)
            {
                string newKinkDescrip = Context.Message.Content;
                if (!newKinkDescrip.Equals("fartz", StringComparison.OrdinalIgnoreCase))
                {
                    groupToEdit.KinkGroupDescrip = newKinkDescrip;

                    KinkGroup kinkToCheck = Vars.tableEntries.Where(x => x.GetType().Name == "KinkGroup").ToList().Cast<KinkGroup>().ToList().Where(x => x.KinkGroupID.ToString() == groupCommand.CommandData).FirstOrDefault();

                    if (groupToEdit.KinkGroupDescrip != kinkToCheck.KinkGroupDescrip)
                    {
                        Console.WriteLine("Kink in edit list not updating");
                    }

                }

                await Context.Message.DeleteAsync();


                string newMessage = "Welcome " + Context.User.Mention + "\n" +
                    "Now updating entry with new Name and Description: \n"
                    + "Name: " + groupToEdit.KinkGroupName + "\n"
                    + "Desc: " + groupToEdit.KinkGroupDescrip + "\n";


                ulong msgToEditID = groupCommand.MessageID;

                var msgToEdit = (RestUserMessage)await Context.Channel.GetMessageAsync(msgToEditID, CacheMode.AllowDownload);

                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await DataMethods.EditGroup(groupToEdit.KinkGroupID, groupToEdit.KinkGroupName, groupToEdit.KinkGroupDescrip);


                await Task.Delay(1000);

                newMessage += "\n.";
                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Task.Delay(1000);

                newMessage += ".";
                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Task.Delay(1000);

                newMessage += ".";
                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                await Task.Delay(1000);

                newMessage += " Done.";
                await msgToEdit.ModifyAsync(x => x.Content = newMessage);

                Vars.activeCommands.RemoveAll(x => x.ActorID == Context.User.Id);

                Vars.tableEntries.RemoveAll(x => x.GetType().Name == "KinkGroup" && (x as KinkGroup).KinkGroupID == groupToEdit.KinkGroupID);

            }


        }



        /*
        public async Task DeleteKinkData(SocketCommandContext Context)
        {

        }
        */


    }
}
