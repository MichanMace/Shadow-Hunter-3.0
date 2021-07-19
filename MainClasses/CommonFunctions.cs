using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Discord.Commands;

namespace ShadowHunter.Common
{
    class CommonFunctions
    {
        public static List<BlockProfile> blockProfiles;
        public static List<MobProfile> mobProfiles;
        public static List<ItemProfile> itemProfiles;
        public static List<BiomeProfile> biomeProfiles;
        public static List<PetProfile> petProfiles;
        public static List<TraitProfile> traitProfiles;

        public static void UpdateInfo()
        {
            blockProfiles = JsonConvert.DeserializeObject<List<BlockProfile>>(File.ReadAllText("JSON/Block.json"));
            mobProfiles = JsonConvert.DeserializeObject<List<MobProfile>>(File.ReadAllText("JSON/Mob.json"));
            itemProfiles = JsonConvert.DeserializeObject<List<ItemProfile>>(File.ReadAllText("JSON/Item.json"));
            biomeProfiles = JsonConvert.DeserializeObject<List<BiomeProfile>>(File.ReadAllText("JSON/Biome.json"));
            petProfiles = JsonConvert.DeserializeObject<List<PetProfile>>(File.ReadAllText("JSON/Pet.json"));
            traitProfiles = JsonConvert.DeserializeObject<List<TraitProfile>>(File.ReadAllText("JSON\\Trait.json"));
        }

        public EmbedBuilder CreateEmbed(string[] text, string category, SocketCommandContext Context)
        {
            bool isTitle = true;
            string description = "";
            string theCategory = category.ToUpper();

            foreach (string s in text)
            {
                if (!isTitle && s != "") description += s + "\n\n";
                else isTitle = false;
            }

            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField((text[0] != "Showcase" && text[0] != "Meme") ? $"{theCategory} INFORMATION" : text[0].ToUpper(), description)
                .WithAuthor((text[0] != "Showcase" && text[0] != "Meme") ? text[0].ToUpper() : "")
                .WithCurrentTimestamp()
                .WithFooter("Requested by " + Context.User.Username)
                .WithColor(MakeColor(text[2]))
                .WithImageUrl(text[text.Length - 1])
                .Build();

            return embed;
        }

        Color MakeColor(string sample)
        {
            if (sample.Contains("Peaceful"))
                return new Color(0, 255, 0);

            else if (sample.Contains("Neutral"))
                return new Color(255, 255, 0);

            else if (sample.Contains("Aggressive"))
            {
                if (sample.Contains("Boss"))
                    return new Color(50, 0, 0);
                else
                    return new Color(255, 0, 0);
            }

            else
                return new Color(0, 0, 255);
        }
    }
}
