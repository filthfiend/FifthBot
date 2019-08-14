using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FifthBot.Resources.Database
{
    public class ServerSetting
    {
        [Key]
        public ulong ServerID { get; set; }
        public string ServerName { get; set; }

        /*
        public int purgerInterval { get; set; }
        public string mutedRole { get; set; }
        
        public string intAdminGroups { get; set; }
        [NotMapped]
        public string[] adminGroups
        {
            get
            {
                return intAdminGroups.Split(';');
            }
            set
            {
                string[] data = value;
                this.intAdminGroups = String.Join(";", data);
            }
        }

        public string intModGroups { get; set; }
        [NotMapped]
        public string[] modGroups
        {
            get
            {
                return intModGroups.Split(';');
            }
            set
            {
                string[] data = value;
                this.intModGroups = String.Join(";", data);
            }
        }

        public string intIntroChannels { get; set; }
        [NotMapped]
        public string[] introChannels
        {
            get
            {
                return intIntroChannels.Split(';');
            }
            set
            {
                string[] data = value;
                this.intIntroChannels = String.Join(";", data);
            }
        }
        */
    }
}
