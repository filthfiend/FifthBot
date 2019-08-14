using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using FifthBot.Core.Data;
using FifthBot.Resources.Database;

namespace FifthBot.Core.Currency
{
    public class Stones : ModuleBase<SocketCommandContext>
    {
        [RequireOwner(Group = "Permission")]
        [Group("stone"), Alias("stones"), Summary("Group to manage stuff to do with stones")]
        public class StonesGroup : ModuleBase<SocketCommandContext>
        {
            [RequireOwner(Group = "Permission")]
            [Command(""), Alias("me", "my"), Summary("Shows all your current stones")]
            public async Task Me(IUser User = null)
            {
                if (User == null)
                {
                    await Context.Channel.SendMessageAsync($"{Context.User.Username}, you have {DataMethods.GetStones(Context.User.Id)} stones!");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{User.Username}, you have {DataMethods.GetStones(User.Id)} stones!");
                }

            }
            [RequireOwner(Group = "Permission")]
            [Command("give"), Alias("gift"), Summary("Used to give people stones")]
            public async Task Give(IUser User = null, int Amount = 0)
            {
                //stones give @fifthfiend 1000
                //group  cmd  user        amount

                //checks
                //does user have permissions?
                //does user have enough stones?
                if (User == null)
                {
                    //The executer has not mentioned a user
                    await Context.Channel.SendMessageAsync(":x: You didn't mention a user to give the stones to! Please use this syntax: !!stones give **<@user> <amount>");
                    return;
                }

                //Make sure a user has been pinged
                if (User.IsBot)
                {
                    await Context.Channel.SendMessageAsync(":x: Bots can't use this bot, so you can't give stones to a bot!");
                    return;


                }

                //At this point we know a user is pinged and is not a bot
                if (Amount < 1)
                {
                    await Context.Channel.SendMessageAsync($":x: You need to specify a valid amount of stones that I need to give to {User.Username}!");
                    return;
                }

                //user is pinged, not a bot, gave a valid coin number

                SocketGuildUser User1 = Context.User as SocketGuildUser;
                if (!User1.GuildPermissions.Administrator)
                {
                    await Context.Channel.SendMessageAsync($":x: You don't have administrator permissions in this discord server! Ask an administrator or the owner to execute this command!");
                    return;
                }


                //execution
                //calculations (games)
                // telling the user what he's gotten

                await Context.Channel.SendMessageAsync($":tada: {User.Mention} you have received **{Amount}** stones from {Context.User.Username}");

                //saving the data
                //save the data to the database
                //save a file

                await DataMethods.SaveStones(User.Id, Amount);
                return;
            }

            [RequireOwner(Group = "Permission")]
            [Command("reset"), Summary("resets the user's entire progress")]
            public async Task Reset(IUser User = null)
            {
                // checks
                if (User == null)
                {
                    await Context.Channel.SendMessageAsync($":x: You need to tell me the user you want to reset the stones of! For example: !!stones reset {Context.User.Mention}");
                    return;
                }
                if (User.IsBot)
                {
                    await Context.Channel.SendMessageAsync("Bots are not allowed to have stones!");
                    return;
                }
                SocketGuildUser User1 = Context.User as SocketGuildUser;
                if (!User1.GuildPermissions.Administrator)
                {
                    await Context.Channel.SendMessageAsync($":x: You don't have admin permissions on this server! Only an admin can run this command!");
                    return;
                }

                // Execution

                await Context.Channel.SendMessageAsync($":skull: {User.Mention}, you have been reset by {Context.User.Username}! This means you lost all your stones!");

                // Saving the data

                using (var DbContext = new SqliteDbContext())
                {
                    DbContext.Stones.RemoveRange(DbContext.Stones.Where(x => x.UserId == User.Id));
                    await DbContext.SaveChangesAsync();
                }



            }



        }
    }
}
