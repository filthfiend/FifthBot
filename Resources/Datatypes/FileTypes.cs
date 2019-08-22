using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FifthBot.Resources.Datatypes
{
    public class Setting
    {
        [JsonProperty]
        public static string token { get; set; }
        [JsonProperty]
        public static ulong owner { get; set; }
        [JsonProperty]
        public static string dblocation { get; set; }
        [JsonProperty]
        public static string phase { get; set; }
        [JsonProperty]
        public static List<ulong> log { get; set; }
        [JsonProperty]
        public static string prefix { get; set; }
        [JsonProperty]
        public static string version { get; set; }

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
