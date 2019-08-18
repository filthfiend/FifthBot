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
    public class MenuAddMethods
    {
        public async Task KinkAdder(SocketReaction reaction)
        {

            var reactedMenu = Vars.groupMenus.Where
                (
                    x => x.KinkMsgID == reaction.MessageId || 
                    x.LimitMsgID == reaction.MessageId
                ).FirstOrDefault();

            bool isLimit = false;

            if (reactedMenu.LimitMsgID == reaction.MessageId)
            {
                isLimit = true;
            }

            // need some type of data retrieval method here
            // it needs to take in 
            // - a kinkgroup id 
            // - an emoji name
            // - and a server id 
            // spit out a kink id
            // call it GetKinkFromMenu

            ulong kinkIDToAdd = DataMethods.GetKinkFromMenu(reactedMenu, reaction.Emote);

            await DataMethods.AddUserKink(reaction.UserId, kinkIDToAdd, isLimit);


        }
    }
}
