using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;

namespace FifthBot.Resources.Preconditions
{
    public class ExcludeDMAttribute : PreconditionAttribute
    {

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {

            if (context.Channel.GetType() != typeof(SocketDMChannel))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            return Task.FromResult(PreconditionResult.FromError("You cannot run this command via DM, sowwy!"));


        }
    }
}
