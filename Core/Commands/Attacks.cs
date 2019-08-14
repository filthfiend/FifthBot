using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using FifthBot.Core.Data;

namespace FifthBot.Core.Commands
{
    public class Attacks : ModuleBase<SocketCommandContext>
    {
        [RequireOwner(Group = "Permission")]
        [Command("shoot"), Alias("shootwithagun"), Summary("Shoot someone with a gun!")]
        public async Task shoot(IUser User = null)
        {
            if (User == null)
            {
                await Context.Channel.SendMessageAsync($"Who were you trying to shoot, {Context.User.Mention}?");
                return;
            }

            await Context.Channel.SendMessageAsync($"*{Context.User.Mention} shoots {User.Mention} with a gun! BLAM!*  :gun:");

            await DataMethods.SaveAttack(Context.User.Id, User.Id, "shoot");

            return;

        }
        [RequireOwner(Group = "Permission")]
        [Command("spank"), Summary("Spank a butt!")]
        public async Task Spank(IUser User = null)
        {
            if (User == null)
            {
                await Context.Channel.SendMessageAsync($"Who were you trying to spank, {Context.User.Mention}?");
                return;
            }

            await Context.Channel.SendMessageAsync($"*{Context.User.Mention} spanks {User.Mention} right on the butt! YOWCH!*  :clap:");

            await DataMethods.SaveAttack(Context.User.Id, User.Id, "spank");

            return;

        }
        [RequireOwner(Group = "Permission")]
        [Command("whip"), Summary("Crack a whip!")]
        public async Task Whip(IUser User = null)
        {
            if (User == null)
            {
                await Context.Channel.SendMessageAsync($"Who were you trying to whip, {Context.User.Mention}?");
                return;
            }

            await Context.Channel.SendMessageAsync($"*{Context.User.Mention} lashes {User.Mention} with a whip! CRACK!*");

            await DataMethods.SaveAttack(Context.User.Id, User.Id, "whip");

            return;

        }
        [RequireOwner(Group = "Permission")]
        [Command("dodge"), Alias("dodges"), Summary("Dodge an attack!")]
        public async Task Dodge()
        {

            await DataMethods.RemoveOldAttacks();

            (ulong AttackerId, string Name) = DataMethods.getAttacker(Context.User.Id);

            if(AttackerId == 0)
            {
                await Context.Channel.SendMessageAsync("what are you dodging?");
                return;
            }
            else
            {
                Random rng = new Random();
                int DodgeAttempt = rng.Next(1, 100);

                IUser Attacker = Context.Guild.GetUser(AttackerId);
                if (Name.Equals("shoot"))
                {
                    if (DodgeAttempt > 50)
                    {
                        await Context.Channel.SendMessageAsync($"*{Context.User.Mention} dodges a bullet from {Attacker.Mention}! Wow, that was quick!*");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"*{Context.User.Mention} fails to dodge, and takes a bullet right in the face from {Attacker.Mention}!*  :boom:");
                    }
                    
                }
                else if (Name.Equals("spank"))
                {
                    if (DodgeAttempt > 70)
                    {
                        await Context.Channel.SendMessageAsync($"*{Context.User.Mention} gets their butt out of the way! Better luck next time {Attacker.Mention}!*");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"*{Context.User.Mention} fails to dodge, and takes a spank right on the butt from {Attacker.Mention}!*  :boom:");
                    }
                    
                }
                else if (Name.Equals("whip"))
                {
                    if (DodgeAttempt > 60)
                    {
                        await Context.Channel.SendMessageAsync($"*{Attacker.Mention} cracks their whip at thin air as {Context.User.Mention} evades!*");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"*{Context.User.Mention} fails to dodge, and takes a vicious lashing from {Attacker.Mention}!*  :boom:");
                    }

                }

                await DataMethods.RemoveAttack(Context.User.Id);
            }




        }



    }
}
