using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace FifthBot.Resources.Database
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Stone> Stones { get; set; }
        public DbSet<Attack> Attacks { get; set; }
        public DbSet<Command> Commands { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ServerSetting> ServerSettings { get; set; }
        public DbSet<IntroChannel> IntroChannels { get; set; }

        public DbSet<JoinedKinkUser> JoinedKinksUsers { get; set; }
        public DbSet<KinkGroup> KinkGroups { get; set; }
        public DbSet<Kink> Kinks { get; set; }

        public DbSet<KinkEmoji> KinkEmojis {get;set;}
        public DbSet<KinkGroupMenu> KinkGroupMenus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            base.OnConfiguring(Options);

            string dbLocation = @"C:\discordbots\FifthBot\FifthBot\Data\Database.sqlite";

            if (!File.Exists(dbLocation))
            {
                dbLocation = @"/home/ubuntu/FifthBot/Data";
            }

            if (!File.Exists(dbLocation))
            {
                dbLocation = @"C:\discordbots\FifthBot\FifthBot\Data\Database.sqlite";
            }

            string sourceString = @"Data Source=" + dbLocation;

            Options.UseSqlite(sourceString);

            //sourceString = @"Data Source=C:\discordbots\FifthBot\FifthBot\Data\Database.sqlite";


            //string localDBString = @"Server=(localdb)\mssqllocaldb;Database=SinBot;Trusted_Connection=True;AttachDBFilename=C:\discordbots\FifthBot\FifthBot\Data\Database.mdf";
            //Options.UseSqlServer(localDBString);


            //string DbLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.1",
            //   @"\Data\");
            //string workingDirectory = Environment.CurrentDirectory;
            //string projectPath = Directory.GetParent(workingDirectory).Parent.FullName;
            //string sourceString = "Data Source=" + workingDirectory + @"\Data\Database.sqlite";



            //string sourceString = @"Data Source=C:\discordbots\FifthBot\FifthBot\Data\Database.sqlite";

            //Console.WriteLine(sourceString);



            // to create database, have Options.UseSqlite("Data Source=" + DbLocation); 
            // or possibly Options.UseSqlite($"Data Source={DbLocation}Database.sqlite");
            // run package manager console and type 
            // add-migration Migration, then do Options.UseSqlite($"Data Source=Database.sqlite");
            // alt: Options.UseSqlite("Data Source=" + DbLocation + "database.sqlite" );


        }
    }
}
