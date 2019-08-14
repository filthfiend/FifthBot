using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


using Discord;
using Discord.Commands;

namespace FifthBot.Core.Commands
{
    public class HelloWorld : ModuleBase<SocketCommandContext>
    {
        [RequireOwner(Group = "Permission")]
        [Command("hello"), Alias("helloworld", "world"), Summary("Hello world command")]
        public async Task helloWorldTask()
        {
            await Context.Channel.SendMessageAsync("Hello World!");
        }
        [RequireOwner(Group = "Permission")]
        [Command("embed"), Summary("Embed test command")]
        public async Task Embed([Remainder]string Input = "lol")
        {
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.WithAuthor("Test embed", Context.User.GetAvatarUrl());
            Embed.WithAuthor("Test 2", Context.Client.CurrentUser.GetAvatarUrl());
            Embed.WithColor(40, 200, 150);
            Embed.WithFooter("Footer", Context.Guild.IconUrl);
            Embed.WithDescription("Blah blah blah, here is a link!\n" + "[here you go!](http://www.google.com)");
            Embed.AddField("User Input:", Input);




            await Context.Channel.SendMessageAsync("", false, Embed.Build());
        }
    }
}
