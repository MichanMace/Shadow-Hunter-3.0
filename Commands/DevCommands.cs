using System;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using ShadowHunter;
using Newtonsoft.Json;

namespace TheCommands
{
    class DevCommands : ModuleBase<SocketCommandContext>
    {
        [Command("Update")]
        public async Task Echo()
        {
            await Context.Channel.SendMessageAsync("Hello World");
        }

        [Command("Embed")]
        public async Task SimpleEmbed()
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Here is the title!");
            embed.WithDescription("This is my embedded message.");
            embed.WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("CreateProfile")]
        public async Task CreateProfile()
        {
            if (Context.User.Id != 306956988913287168) return;

            CreateProfilesToJSON("Block"); //CHANGE THIS WHEN NEEDED

            await Context.Channel.SendMessageAsync("Command finished Executing!");
        }

        void CreateProfilesToJSON(string category)
        {
            //BlockProfile[] profiles = new BlockProfile[Directory.GetFiles(category).Length];

            //File.WriteAllText("JSON/Block.json", JsonConvert.SerializeObject(profiles));

            Console.WriteLine("Initializing...");
            string problemFile = "";
            string problemPath = "";
            var files = Directory.EnumerateFiles(category, "*.txt");
            try
            {
                Console.WriteLine("Enter Try Block");
                BlockProfile[] theProfiles = new BlockProfile[Directory.GetFiles(category).Length];
                int count = 0;

                foreach (string file in files)
                {
                    Console.WriteLine("Beginning data transfer");
                    problemPath = file;
                    BlockProfile blp = new BlockProfile(); //CHANGE THIS WHEN NEEDED

                    string[] lines = File.ReadAllLines(file);
                    Console.WriteLine("Got All lines");

                    for (int x = 0; x < lines.Length; x++)
                    {
                        lines[x] = lines[x].Replace("**", "");

                        if (lines[x].Contains(":"))
                        {
                            string[] s = lines[x].Replace(": ", ":").Split(':');
                            lines[x] = s[1];
                        }
                    }
                    Console.WriteLine("Cleaned up lines");

                    problemFile = lines[0].ToUpper();

                    //CHANGE THIS WHEN NEEDED
                    blp.Title = lines[0].ToUpper();
                    blp.Name = lines[0];
                    blp.Category = lines[1];
                    blp.Location = lines[2];
                    blp.DigTool = lines[3];
                    blp.Properties = lines[4];
                    blp.Recipe = lines[5];
                    blp.Additional = lines[6];
                    blp.PictureURL = "";
                    //CHANGE THIS WHEN NEEDED

                    Console.WriteLine("Created object");

                    theProfiles[count] = blp;

                    count++;
                }

                File.WriteAllText("JSON/Block.json", JsonConvert.SerializeObject(theProfiles, Formatting.Indented));

                Console.WriteLine("JSON Written");
            }
            catch(Exception)
            {
                Console.WriteLine("\n\n\n***PROBLEM FILE DETECTED***");
                Console.WriteLine($"{problemFile} is a bad file and must be deleted!");
                File.Delete(problemPath);
                Console.WriteLine("File deleted! Recommencing check up.");
                CreateProfilesToJSON(category);
            }
        }

        [Command("list")]
        public async Task List(string category)
        {
            string theList = "";
            switch (category)
            {
                case "Block":
                case "block":
                    List<BlockProfile> BlockJSON = JsonConvert.DeserializeObject<List<BlockProfile>>(File.ReadAllText("JSON/Block.json"));

                    foreach (BlockProfile b in BlockJSON)
                        theList += b.Name + "\n";

                    await Context.Channel.SendMessageAsync(theList);
                    break;

                case "Mob":
                case "mob":
                    List<MobProfile> MobJSON = JsonConvert.DeserializeObject<List<MobProfile>>(File.ReadAllText("JSON/Mob.json"));

                    foreach (MobProfile b in MobJSON)
                        theList += b.Name + "\n";

                    await Context.Channel.SendMessageAsync(theList);
                    break;

                case "Item":
                case "item":
                    List<ItemProfile> ItemJSON = JsonConvert.DeserializeObject<List<ItemProfile>>(File.ReadAllText("JSON/Item.json"));

                    foreach (ItemProfile b in ItemJSON)
                        theList += b.Name + "\n";

                    await Context.Channel.SendMessageAsync(theList);
                    break;

                case "Biome":
                case "biome":
                    /*List<BiomeProfile> BiomeJSON = JsonConvert.DeserializeObject<List<BiomeProfile>>(File.ReadAllText("JSON/Biome.json"));
                    IEnumerable<BiomeProfile> theBiomeProfiles = from a in BiomeJSON where a.Title == theRequest select a;
                    var BiomeResult = theBiomeProfiles.FirstOrDefault();
                    text = new string[] {BiomeResult}*/
                    break;

                case "Pet":
                case "pet":
                    List<PetProfile> PetJSON = JsonConvert.DeserializeObject<List<PetProfile>>(File.ReadAllText("JSON/Pet.json"));

                    foreach (PetProfile b in PetJSON)
                        theList += b.Name + "\n";

                    await Context.Channel.SendMessageAsync(theList);
                    break;
            }
        }

        /*[Command("qwerty")]
        public async Task qwerty()
        {
            if (Context.User.Id != 306956988913287168) return;

            string theChunk = "1_0_0\\17_1_1.chunk";

            for (int x = 17; x > 6; x--)
            {
                string d = (x > 15) ? "1_0_0" : "0_0_0";

                for (int z = 1; z <= 10; z++)
                {
                    string dest = $"{d}\\{x}_1_{z}.chunk";

                    if (!File.Exists(dest))
                        File.Copy(theChunk, dest);
                }
            }

            await Context.Channel.SendMessageAsync("Task completed!");
        }*/
    }
}
