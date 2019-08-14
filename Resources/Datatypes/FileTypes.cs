using System;
using System.Collections.Generic;
using System.Text;

namespace FifthBot.Resources.Datatypes
{
    public class Setting
    {
        public string token { get; set; }
        public ulong owner { get; set; }
        public List<ulong> log { get; set; }
        public string version { get; set; }
    }
    public class ServerSetting
    {
        public string settingsVersion { get; set; }
        public ulong serverID { get; set; }
        public string name { get; set; }
        public List<string> introChannels { get; set; }
        public List<string> adminGroups { get; set; }
        public List<string> modGroups { get; set; }
        public int purgerInterval { get; set; }
        public string mutedRole { get; set; }
    }
}
