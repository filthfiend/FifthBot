using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using System.Linq;

using FifthBot.Resources.Database;
using FifthBot.Core.Utils;



namespace FifthBot.Core.Data
{
    public static class DataMethods
    {
        public static int GetStones(ulong UserId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Stones.Where(x => x.UserId == UserId).Count() < 1)
                {
                    return 0;
                }
                return DbContext.Stones.Where(x => x.UserId == UserId).Select(x => x.Amount).FirstOrDefault();
            }
        }

        public static async Task SaveStones(ulong UserId, int Amount)
        {
            using (var DbContext = new SqliteDbContext())
            {

                if (DbContext.Stones.Where(x => x.UserId == UserId).Count() < 1)
                {
                    // this user doesn't have a row yet, create one for him
                    DbContext.Stones.Add(new Stone
                    {
                        UserId = UserId,
                        Amount = Amount
                    });
                }
                else
                {
                    Stone Current = DbContext.Stones.Where(x => x.UserId == UserId).FirstOrDefault();
                    Current.Amount += Amount;
                    DbContext.Stones.Update(Current);
                }

                await DbContext.SaveChangesAsync();
            }
        }
        public static (ulong, string) getAttacker(ulong TargetId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.Attacks.Where( x => x.TargetId == TargetId).Count() < 1)
                {
                    return (0, "");
                }
                else
                {
                    ulong AttackerID = DbContext.Attacks.Where(x => x.TargetId == TargetId).FirstOrDefault().AttackerId;

                    string Name = DbContext.Attacks.Where(x => x.TargetId == TargetId).FirstOrDefault().Name;

                    return (AttackerID, Name);

                }
            }
        }

        public static async Task SaveAttack(ulong AttackerId, ulong TargetId, string Name )
        {
            using (var DbContext = new SqliteDbContext())
            {
                DbContext.Attacks.Add(new Attack
                {
                    AttackerId = AttackerId,
                    TargetId = TargetId,
                    Name = Name,
                    DateandTime = DateTime.Now

                });

                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task RemoveAttack(ulong TargetId)
        {
            using (var DbContext = new SqliteDbContext())
            {
                DbContext.Attacks.Remove(DbContext.Attacks.Where(x => x.TargetId == TargetId).FirstOrDefault());
                await DbContext.SaveChangesAsync();
            }
        }

        public static async Task RemoveOldAttacks()
        {
            using (var DbContext = new SqliteDbContext())
            {
                DateTime CurrentDT = DateTime.Now;

                DbContext.Attacks.RemoveRange
                    (
                    DbContext.Attacks.Where(x => (CurrentDT - x.DateandTime).TotalSeconds > 30)
                    );
                await DbContext.SaveChangesAsync();
            }
        }



        public static ServerSetting GetServer (ulong serverID)
        {
            using (var DbContext = new SqliteDbContext())
            {

                if (DbContext.ServerSettings.Where(x => x.ServerID == serverID).Count() < 1)
                {
                    Console.WriteLine("null lol");
                    return null;
                }

                Console.WriteLine("not null lol");
                return DbContext.ServerSettings.Where(x => x.ServerID == serverID).FirstOrDefault();

            }

        }






        public static async Task AddServer(ulong serverID, string serverName /*, int purgerInterval = 50, string mutedRole = "Muted", string[] adminGroups = null, string[] modGroups = null, string[] introChannels = null*/ ) 
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.ServerSettings.Where(x => x.ServerID == serverID).Count() < 1)
                {
                    DbContext.ServerSettings.Add(new ServerSetting
                    {
                        ServerID = serverID,
                        ServerName = serverName,

                        /*
                        purgerInterval = purgerInterval,
                        mutedRole = "Muted",
                        adminGroups = adminGroups ?? new string[] { "Admins" },
                        modGroups = modGroups ?? new string[] { "Moderators" },
                        introChannels = introChannels ?? new string[] { "male-intros", "female-intros", "other-intros" }
                        */

                    });

                    



                }
                await DbContext.SaveChangesAsync();
            }

        }

        public static List<IntroChannel> GetIntroChannels(ulong serverID)
        {
            using (var DbContext = new SqliteDbContext())
            {

                if (DbContext.IntroChannels.Where(x => x.ServerID == serverID).Count() < 1)
                {
                    return null;
                }

                return DbContext.IntroChannels.Where(x => x.ServerID == serverID).ToList();

            }

        }

        public static ulong[] GetIntroChannelIDs(ulong serverID)
        {
            using (var DbContext = new SqliteDbContext())
            {

                if (DbContext.IntroChannels.Where(x => x.ServerID == serverID).Count() < 1)
                {
                    return null;
                }


                var channelRecords = DbContext.IntroChannels.Where(x => x.ServerID == serverID).ToList();

                List<ulong> channelIDs = new List<ulong>();

                foreach (IntroChannel channelRecord in channelRecords)
                {
                    channelIDs.Add(channelRecord.ChannelID);
                }

                return channelIDs.ToArray();



            }

        }

        public static async Task AddIntroChannel(ulong serverID, ulong channelID)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.IntroChannels.Where(x => x.ChannelID == channelID).Count() < 1)
                {
                    DbContext.IntroChannels.Add(new IntroChannel
                    {
                        ServerID = serverID,
                        ChannelID = channelID,

                    });


                }
                await DbContext.SaveChangesAsync();
            }




        }

        public static async Task AddKink(string kinkName, string kinkDesc)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (!DbContext.Kinks.Any(x => x.KinkName == kinkName))
                {
                    DbContext.Kinks.Add(new Kink
                    {
                        KinkName = kinkName,
                        KinkDesc = kinkDesc,

                    });
                }




                await DbContext.SaveChangesAsync();
            }

        }

        public static async Task AddGroup(string groupName, string groupDesc)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (!DbContext.KinkGroups.Any(x => x.KinkGroupName == groupName))
                {
                    DbContext.KinkGroups.Add(new KinkGroup
                    {
                        KinkGroupName = groupName,
                        KinkGroupDescrip = groupDesc,

                    });
                }




                await DbContext.SaveChangesAsync();
            }

        }

        public static Kink GetKink(string kinkName)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if( DbContext.Kinks.Where(x => x.KinkName.Equals(kinkName)).Count() < 1)
                {
                    return null;
                }

                return DbContext.Kinks.Where(x => x.KinkName.Equals(kinkName)).FirstOrDefault();


            }

        }

        public static KinkGroup GetGroup(string groupName)
        {
            using (var DbContext = new SqliteDbContext())
            {
                if (DbContext.KinkGroups.Where(x => x.KinkGroupName.Equals(groupName)).Count() < 1)
                {
                    return null;
                }

                return DbContext.KinkGroups.Where(x => x.KinkGroupName.Equals(groupName)).FirstOrDefault();


            }

        }

        public static List<Kink> GetKinkList()
        {
            using (var DbContext = new SqliteDbContext())
            {


                if (DbContext.Kinks.Count() < 1)
                {
                    return null;
                }

                return DbContext.Kinks.ToList();


            }

        }

        public static List<Kink> GetKinksInGroup(ulong groupID)
        {
            using (var DbContext = new SqliteDbContext())
            {


                if (DbContext.Kinks.Where(x => x.KinkGroupID == groupID).Count() < 1)
                {
                    return null;
                }

                return DbContext.Kinks.Where(x => x.KinkGroupID == groupID).ToList();


            }

        }


        public static List<KinkWithEmoji> GetKinksInGroupWithEmojis(ulong groupID, ulong serverID)
        {
            using (var DbContext = new SqliteDbContext())
            {
                
                if (
                        DbContext.Kinks.GroupJoin
                            (
                                DbContext.KinkEmojis,
                                Kink => Kink.KinkID,
                                KinkEmoji => KinkEmoji.ServerID,
                                (Kink, KinkEmoji) => new { Kink, KinkEmoji }


                            )
                        .SelectMany
                            (
                                Kink => Kink.KinkEmoji.DefaultIfEmpty(),
                                (KinkHolder, KinkEmoji) => new KinkWithEmoji
                                {
                                    KinkID = KinkHolder.Kink.KinkID,
                                    KinkName = KinkHolder.Kink.KinkName,
                                    KinkDesc = KinkHolder.Kink.KinkDesc,
                                    KinkGroupID = KinkHolder.Kink.KinkGroupID,
                                    ServerID = KinkEmoji.ServerID,
                                    EmojiName = KinkEmoji.EmojiName,
                                }
                            )
                        .Where(x => x.ServerID == serverID && (x.KinkGroupID == groupID || x.KinkGroupID == 0))
                        .Count() < 1
                    )
                {
                    return null;
                }

                return DbContext.Kinks.GroupJoin
                    (
                        DbContext.KinkEmojis,
                        Kink => Kink.KinkID,
                        KinkEmoji => KinkEmoji.ServerID,
                        (Kink, KinkEmoji) => new { Kink, KinkEmoji }


                    )
                .SelectMany
                    (
                        Kink => Kink.KinkEmoji.DefaultIfEmpty(),
                        (KinkHolder, KinkEmoji) => new KinkWithEmoji
                        {
                            KinkID = KinkHolder.Kink.KinkID,
                            KinkName = KinkHolder.Kink.KinkName,
                            KinkDesc = KinkHolder.Kink.KinkDesc,
                            KinkGroupID = KinkHolder.Kink.KinkGroupID,
                            ServerID = KinkEmoji.ServerID,
                            EmojiName = KinkEmoji.EmojiName,
                        }
                    )
                .Where(x => x.ServerID == serverID && (x.KinkGroupID == groupID || x.KinkGroupID == 0))
                .ToList();



                 /*
                 return DbContext.Kinks.Join
                    (
                        DbContext.KinkEmojis,
                        Kink => Kink.KinkID,
                        KinkEmoji => KinkEmoji.ServerID,
                        (Kink, KinkEmoji) => new KinkWithEmoji
                        {
                            KinkID = Kink.KinkID,
                            KinkName = Kink.KinkName,
                            KinkDesc = Kink.KinkDesc,
                            KinkGroupID = Kink.KinkGroupID,
                            ServerID = KinkEmoji.ServerID,
                            EmojiName = KinkEmoji.EmojiName,
                        }
                    )
                   .Where(x => x.ServerID == serverID && x.KinkGroupID == groupID).ToList();
                 */
                    
             }
        }






        public static List<KinkGroup> GetGroupList()
        {
            using (var DbContext = new SqliteDbContext())
            {


                if (DbContext.KinkGroups.Count() < 1)
                {
                    return null;
                }

                return DbContext.KinkGroups.ToList();


            }

        }

        public static async Task<bool> EditKink(ulong kinkID, string kinkName, string kinkDesc)
        {

            using (var DbContext = new SqliteDbContext())
            {
                if(!DbContext.Kinks.Any(x => x.KinkID == kinkID ))
                {
                    return false;
                }

                Kink kinkToEdit = DbContext.Kinks.Where(x => x.KinkID == kinkID).FirstOrDefault();
                kinkToEdit.KinkName = kinkName;
                kinkToEdit.KinkDesc = kinkDesc;


                await DbContext.SaveChangesAsync();

                return true;

            }

        }

        public static async Task<bool> EditGroup(ulong groupID, string groupName, string groupDesc)
        {

            using (var DbContext = new SqliteDbContext())
            {
                if (!DbContext.KinkGroups.Any(x => x.KinkGroupID == groupID))
                {
                    return false;
                }

                KinkGroup groupToEdit = DbContext.KinkGroups.Where(x => x.KinkGroupID == groupID).FirstOrDefault();
                groupToEdit.KinkGroupName = groupName;
                groupToEdit.KinkGroupDescrip = groupDesc;


                await DbContext.SaveChangesAsync();

                return true;

            }

        }

        public static async Task<bool> AddKinkToGroup(ulong groupID, string kinkName)
        {
            using (var DbContext = new SqliteDbContext())
            {
                Kink kinkToGroup = DbContext.Kinks.Where(x => x.KinkName == kinkName).FirstOrDefault();

                if (kinkToGroup == null)
                {
                    return false;
                }

                kinkToGroup.KinkGroupID = groupID;
                await DbContext.SaveChangesAsync();

                return true;
            }
        }

    }
}
