using FifthBot.Resources.Datatypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FifthBot.Resources.Helpers
{
    public static class HelperMethods
    {
        public static void LoadSettings()
        {

            string JSON = "";
            string settingsLocation = @"C:\discordbots\FifthBot\FifthBot\Data\Settings.json";

            if (!File.Exists(settingsLocation))
            {
                settingsLocation = @"FifthBot/Data/ReleaseSettings.json";
            }

            using (var fileStream = new FileStream(settingsLocation, FileMode.Open, FileAccess.Read))
            using (var ReadSettings = new StreamReader(fileStream))
            {
                JSON = ReadSettings.ReadToEnd();
            }

            JsonConvert.DeserializeObject<Setting>(JSON);
        }
    }
}
