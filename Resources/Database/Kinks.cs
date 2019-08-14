using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FifthBot.Resources.Database
{
    public class Kink : ITableEntry
    {
        [Key]
        public ulong KinkID { get; set; }
        public string KinkName { get; set; }
        public string KinkDesc { get; set; }
        public ulong KinkGroupID { get; set; }

    }
}
