using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace FifthBot.Resources.Database
{
    public class KinkGroup : ITableEntry
    {
        [Key]
        public ulong KinkGroupID { get; set; }
        public string KinkGroupName { get; set; }
        public string KinkGroupDescrip { get; set; }


    }
}