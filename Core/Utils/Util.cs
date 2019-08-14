using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FifthBot.Core.Utils
{
    public class Util
    {
        SocketCommandContext Context;

        public Util(SocketCommandContext Context)
        {
            this.Context = Context;
        }

        public async Task sma (string message)
        {
            if (Context == null)
            {
                Console.Error.WriteLine($"[{DateTime.Now} at Utils] Context is null!");
            }

            await Context.Channel.SendMessageAsync(message);
            
        }
    }
}
