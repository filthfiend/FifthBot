using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FifthBot.Resources.Database
{
    public class KinkEmoji
    {
        [Key]
        public ulong JoinID { get; set; }
        public ulong KinkID { get; set; }
        public ulong ServerID { get; set; }
        public string EmojiName { get; set; }
        

    }
}
