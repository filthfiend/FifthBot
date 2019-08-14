using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FifthBot.Resources.Database
{
    public class Attack
    {
        [Key]
        public ulong MessageID { get; set; }
        public ulong AttackerId { get; set; }
        public ulong TargetId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime DateandTime { get; set; }

    }
}
