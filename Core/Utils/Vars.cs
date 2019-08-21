using System;
using System.Collections.Generic;
using System.Text;
using FifthBot.Resources.Database;

namespace FifthBot.Core.Utils
{
    public class Vars
    {
        //public static List<ulong> usersAddingKinks = new List<ulong>();

        public static List<Command> activeCommands = new List<Command>();

        public static List<ITableEntry> tableEntries = new List<ITableEntry>();

        public static MenuBuilder menuBuilder = new MenuBuilder();

        public static List<KinkGroupMenu> groupMenus = new List<KinkGroupMenu>();


    }

    public class MenuBuilder
    {

        public bool IsActive { get; set; }
        public ulong ServerID { get; set; }
        public ulong ChannelID { get; set; }
        public ulong EmojiMenuID { get; set; }
        public ulong EditMenuID { get; set; }
        public ulong UserID { get; set; }
        public ulong KinkGroupID { get; set; }
        public string KinkGroupName { get; set; }
        public int CommandStep { get; set; }
        public bool IsLimitMenu { get; set; }
        public List<KinkWithEmoji> KinksToUpdate { get; set; }
        // kink list or limit list

    }

    public class KinkWithEmoji : Kink
    {
        public ulong ServerID { get; set; }
        public string EmojiName { get; set; }
    }

    public class GroupKinks
    {
        public bool isLimit { get; set; }
        public KinkGroup Group { get; set; }
        public List<Kink> KinksForGroup { get; set; }

    }


}
