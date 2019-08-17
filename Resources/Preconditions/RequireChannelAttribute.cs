using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;

namespace FifthBot.Resources.Preconditions
{
    public class RequireChannelAttribute : PreconditionAttribute
    {
        private readonly string _channelName;

        public RequireChannelAttribute(string name) => _channelName = name;

        

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if(context.Channel.Name.Contains(_channelName))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            return Task.FromResult(PreconditionResult.FromError("You cannot run this command in this channel, sowwy!"));

            
        }
    }
}
