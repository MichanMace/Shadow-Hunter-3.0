using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ShadowHunter
{
    class GeneralJSON
    {
        private static Dictionary<string, string> theKeys;

        static GeneralJSON()
        {
            string json = File.ReadAllText("JSON/keys.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);

            theKeys = data.ToObject<Dictionary<string, string>>();
        }

        public static string GetKey(string key)
        {
            if (theKeys.ContainsKey(key)) return theKeys[key];

            return "";
        }
    }

    public static class DataStorage
    {
    }

    public class DropProfiles
    {
        //public ProfileList profiles = new ProfileList();
        public List<BlockProfile> Blocks = JsonConvert.DeserializeObject<List<BlockProfile>>(File.ReadAllText("JSON/Block.json"));
        public List<MobProfile> Mobs = JsonConvert.DeserializeObject<List<MobProfile>>(File.ReadAllText("JSON/Mob.json"));
        public List<ItemProfile> Items = JsonConvert.DeserializeObject<List<ItemProfile>>(File.ReadAllText("JSON/Item.json"));

        public BlockProfile GetBlockProfile(string Name)
        {
            return (from a in Blocks where a.Title == Name select a).FirstOrDefault();
        }

        public MobProfile GetMobProfile(string Name)
        {
            return (from a in Mobs where a.Title == Name select a).FirstOrDefault();
        }

        public ItemProfile GetItemProfile(string Name)
        {
            return (from a in Items where a.Title == Name select a).FirstOrDefault();
        }
    }

    public class ProfileList
    {
        public List<BlockProfile> Blocks { get; set; }
        public List<MobProfile> Mobs { get; set; }
        public List<ItemProfile> Items { get; set; }
    }

    public class BlockProfile
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string DigTool { get; set; }
        public string Properties { get; set; }
        public bool Loot { get; set; }
        public string Recipe { get; set; }
        public string Additional { get; set; }
        public string PictureURL { get; set; }
    }

    public class MobProfile
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Traits { get; set; }
        public string LevelRange { get; set; }
        public int LowDamage { get; set; }
        public int HighDamage { get; set; }
        public float GrowthDamage { get; set; }
        public int LowHP { get; set; }
        public int HighHP { get; set; }
        public float GrowthHP { get; set; }
        public int LowXP { get; set; }
        public int HighXP { get; set; }
        public float GrowthXP { get; set; }
        public string Drops { get; set; }
        public string Additional { get; set; }
        public string PictureURL { get; set; }
    }

    public class ItemProfile
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Obtain { get; set; }
        public string Use { get; set; }
        public string Durability { get; set; }
        public bool Loot { get; set; }
        public string Recipe { get; set; }
        public string Additional { get; set; }
        public string PictureURL { get; set; }
    }

    public class BiomeProfile
    {
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
    }

    public class ToolProfile
    {

    }

    public class WeaponProfile
    {

    }

    public class NPCProfile
    {

    }

    public class PetProfile
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string DefaultName { get; set; }
        public string Abilities { get; set; }
        public int HP { get; set; }
        public int GrowthHP { get; set; }
        public int MaxHP { get; set; }
        public int Damage { get; set; }
        public int GrowthDamage { get; set; }
        public int MaxDamage { get; set; }
        public int ProjectileDamage { get; set; }
        public int ProjectileGrowth { get; set; }
        public int ProjectileMax { get; set; }
        public string Additional { get; set; }
        public string PictureURL { get; set; }
    }

    public class TraitProfile
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Mobs { get; set; }
    }

    public class QuestProfile
    {

    }

    public class AchievementProfile
    {

    }
}
