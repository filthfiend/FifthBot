using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FifthBot.Resources.Database
{
    public class KinkGroupMenu
    {
        [Key]
        public ulong JoinID { get; set; }
        public ulong KinkGroupID { get; set; }
        public ulong ServerID { get; set; }
        public ulong KinkMsgID { get; set; }
        public ulong KinkChannelID { get; set; }
        public ulong LimitMsgID { get; set; }
        public ulong LimitChannelID { get; set; }
    }
}
