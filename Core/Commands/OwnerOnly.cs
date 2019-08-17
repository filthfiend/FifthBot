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
    public class OwnerOnly : ModuleBase<SocketCommandContext>
    {
        [RequireOwner(Group = "Permission")]
        [Command("quit"), Alias("shutdown"), Summary("Shuts down the bot.")]
        public async Task Quit()
        {
            Console.WriteLine("quitting?");
            await ReplyAsync("Quitting?");
            Environment.Exit(69);



        }

    }
}
