using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace FifthBot.Resources.Database
{
    public class IntroChannel
    {
        [Key]
        public ulong ChannelID { get; set; }
        public ulong ServerID { get; set; }
    }
}
