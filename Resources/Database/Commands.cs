using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FifthBot.Resources.Database
{
    public class Command
    {
        [Key]
        public ulong CommandID { get; set; }
        public ulong MessageID { get; set; }
        public ulong ActorID { get; set; }
        public ulong TargetID { get; set; }
        public ulong ChannelID { get; set; }
        public string CommandName { get; set; }
        public string CommandData { get; set; }
        public int CommandStep { get; set; }
        public DateTime DateTime { get; set; }
    }
}
