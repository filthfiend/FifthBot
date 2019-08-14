using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FifthBot.Resources.Database
{
    public class User
    {
        [Key]
        public ulong UserID { get; set; }
        public ulong ServerID { get; set; }
    }
}
