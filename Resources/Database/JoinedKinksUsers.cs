using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FifthBot.Resources.Database
{
    public class JoinedKinkUser
    {
        [Key]
        public ulong JoinID { get; set; }
        public ulong KinkID { get; set; }
        public ulong UserID { get; set; }
        public bool IsLimit { get; set; }

    }
}
