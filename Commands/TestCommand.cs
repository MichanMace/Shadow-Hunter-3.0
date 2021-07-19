using System;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using ShadowHunter;
using ShadowHunter.Common;
using System.Linq;

using static ShadowHunter.GeneralJSON;
using Newtonsoft.Json;

namespace TheCommands
{
    public class InfoCommands : ModuleBase<SocketCommandContext>
    {
        string SpecifyStatement = GetKey("SPECIFY_REQUEST");
        string ErrorStatement = GetKey("ERROR_REQUEST");
        string ErrorSearch = GetKey("ERROR_SEARCH");
        CommonFunctions cf = new CommonFunctions();

        [Command("Commands")]
        [Alias("Help")]
        public async Task ReturnCommands()
        {
            await Context.Channel.SendMessageAsync("Here is what I can do:" +
                "\n\n`!commands`/`!help` - Returns my list of commands (Which I am doing right now.)" +
                "\n`!block (block name)` - Returns information about the requested block" +
                "\n`!mob (creature name)`/`!creature (creature name)` - Returns information about the requested mob" +
                "\n`!item (item name)` - Returns information about the requested item" +
                "\n`!biome (biome name)` - Returns information about the requested biome" +
                "\n`!pet (pet name)` - Returns information about the requested pet" +
                "\n`!trait (trait name)` - Returns information about the requested trait. Leave this blank for a list of all traits" +
                "\n`!drop (item/block)` - Returns a list of all creatures that drop the specified block or item" +
                "\n`!search (category, string)` - Returns a list of all things in the specified category that starts with the specified letter or contains the combination of letters given" +
                "\n`!loot (letter)` - Returns a list of all blocks and items that can be looted in chests that begins with the specified letter" +
                "\n`!showcase` - Returns a random cool creation or piece of artwork made by a community member" +
                "\n`!meme` - Returns a random Block Story meme" +
                "\n`!credits` - Acknowledges everyone who has contributed to this bot" +
                "\n`!sendnudes` - Don't bother with this command..." +
                "\n\nTEMPORARY COMMAND\n`!useless (feature)` - What is the most useless feature you can think of? Write it with this command and I'll log your response.");
        }

        [Command("Block")]
        public async Task Block([Remainder]string BlockName = "")
        {
            await RequestInfo("Block", BlockName, "block stone");
        }

        [Command("Creature")]
        [Alias("Mob")]
        public async Task Mob([Remainder]string MobName = "")
        {
            await RequestInfo("Creature", MobName, "creature dragon");
        }

        [Command("Item")]
        public async Task Item([Remainder]string ItemName = "")
        {
            await RequestInfo("Item", ItemName, "item coal ore");
        }

        [Command("Trait")]
        public async Task Trait([Remainder]string TraitName = "")
        {
            if (TraitName == "")
                await RequestInfo("Trait", "TraitList", "");
            else
                await RequestInfo("Trait", TraitName, "trait neutral");
        }

        [Command("Biome")]
        public async Task Biome([Remainder]string BiomeName = "")
        {
            await RequestInfo("Biome", BiomeName, "biome desert");
        }

        [Command("Pet")]
        public async Task Pet([Remainder]string PetName = "")
        {
            await RequestInfo("Pet", PetName, "pet dragon");
        }

        [Command("Drop")]
        public async Task Drop([Remainder]string DropName = "")
        {
            await RequestInfo("Drop", DropName, "drop sword");
        }

        [Command("Search")]
        public async Task Search(string category, [Remainder]string SearchRequest = "")
        {
            string[] theCategories = { "Block", "Mob", "Creature", "Item", "Biome", "Pet" };
            string c = category.ToLower().Replace(category[0].ToString(), category[0].ToString().ToUpper());

            foreach (string cat in theCategories)
                if (c == cat)
                {
                    await RequestInfo(c, SearchRequest, "Search");
                    return;
                }

            await Context.Channel.SendMessageAsync("Please specify which category you'd like to search in. Searchable categories are `block`, `mob`, `item`, `biome`, and `pet`" +
                " (Example: `!search block s`)");
        }

        [Command("Loot")]
        //public async Task Loot([Remainder]string category = "")
        public async Task Loot (char letter)
        {
            string l = letter.ToString().ToUpper();
            await RequestInfo("Loot", l, "");
            /*string[] theCategories = { "Block", "Item" };
            try
            {
                //string c = category.ToLower().Replace(category[0].ToString(), category[0].ToString().ToUpper());

                foreach (string cat in theCategories)
                    if (c == cat)
                    {
                        await RequestInfo("Loot", c, "");
                        return;
                    }
                throw new Exception();
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Please Specify if you want to see the list of lootable blocks or lootable items. (``!loot block`` or ``!loot item``");
            }*/
        }

        string SendDisclaimer()
        {
            return "This command is currently under development. Please be patient until the command is complete.";
        }

        async Task RequestInfo(string category, string request, string example)
        {
            if (request == "")
            {
                await Context.Channel.SendMessageAsync((example != "Search") ?
                    String.Format(SpecifyStatement, category, example) :
                    "Please include a letter or group of letters in your search.");
                return;
            }

            string theCategory = category.ToLower();
            string list = "";

            try
            {
                string theRequest = request.Replace(" ", "").ToUpper();

                string[] text = null;

                switch (category)
                {
                    case "Block":
                        List<BlockProfile> BlockJSON = JsonConvert.DeserializeObject<List<BlockProfile>>(File.ReadAllText("JSON/Block.json"));

                        if (example != "Search")
                        {
                            IEnumerable<BlockProfile> theBlockProfiles = from a in BlockJSON where a.Title == theRequest select a;
                            BlockProfile BlockResult = theBlockProfiles.FirstOrDefault();
                            text = new string[]
                            {
                            BlockResult.Name,
                            $"**CATEGORY**: {BlockResult.Category}",
                            $"**PRIMARY LOCATION**: {BlockResult.Location}",
                            $"**RECOMMENDED TOOL**: {BlockResult.DigTool}",
                            $"**PROPERTIES**: {BlockResult.Properties}",
                            $"**FOUND IN CHESTS?**: {(BlockResult.Loot? "Yes" : "No")}",
                            $"**RECIPE**: {BlockResult.Recipe}",
                            $"**ADDITIONAL INFORMATION**: {BlockResult.Additional}",
                            BlockResult.PictureURL
                            };
                        }
                        else
                        {
                            IEnumerable<BlockProfile> theBlockProfiles = null;

                            if (theRequest.Length == 1)
                                theBlockProfiles = from a in BlockJSON where a.Title.StartsWith(theRequest) select a;
                            else
                                theBlockProfiles = from a in BlockJSON where a.Title.Contains(theRequest) select a;

                            foreach (BlockProfile b in theBlockProfiles)
                                if (b.Name != "???")
                                    list += b.Name + "\n";

                            if (list.Length == 0) throw new Exception();

                            text = new string[]
                            {
                                $"Search Results For \"{request}\"",
                                $"The following blocks {((request.Length == 1)? "start with" : "include")} \"***{request}***\"\n\n{list}",
                                ""
                            };
                        }
                        break;

                    case "Creature":
                    case "Mob":
                        List<MobProfile> MobJSON = JsonConvert.DeserializeObject<List<MobProfile>>(File.ReadAllText("JSON/Mob.json"));
                        if (example != "Search")
                        {
                            IEnumerable<MobProfile> theMobProfiles = from a in MobJSON where a.Title == theRequest select a;
                            MobProfile MobResult = theMobProfiles.FirstOrDefault();
                            text = new string[]
                            {
                            MobResult.Name,
                            $"**PRIMARY LOCATION**: {MobResult.Location}",
                            $"**TRAITS**: {MobResult.Traits}",
                            $"**LEVEL RANGE**: {MobResult.LevelRange}",
                            (MobResult.HighDamage != 0)? ((MobResult.LowDamage != MobResult.HighDamage)? $"**LOWEST/HIGHEST ATTACK DAMAGE**: {MobResult.LowDamage}/{MobResult.HighDamage}" : $"**ATTACK DAMAGE**: {MobResult.HighDamage}") : "",
                            (MobResult.GrowthDamage != 0)? $"**ATTACK GROWTH**: {MobResult.GrowthDamage}" : "",
                            (MobResult.LowHP != MobResult.HighHP)? $"**LOWEST/HIGHEST HP**: {MobResult.LowHP}/{MobResult.HighHP}" : $"**HP**: {MobResult.HighHP}",
                            (MobResult.LowXP != MobResult.HighXP)? $"**LOWEST/HIGHEST XP YIELD**: {MobResult.LowXP}/{MobResult.HighXP}" : $"**XP YIELD**: {MobResult.HighXP}",
                            $"**POSSIBLE DROPS**: {MobResult.Drops}",
                            $"**ADDITIONAL INFORMATION**: {MobResult.Additional}",
                            MobResult.PictureURL
                            };
                        }
                        else
                        {
                            IEnumerable<MobProfile> theMobProfiles = null;

                            if (theRequest.Length == 1)
                                theMobProfiles = from a in MobJSON where a.Title.StartsWith(theRequest) select a;
                            else
                                theMobProfiles = from a in MobJSON where a.Title.Contains(theRequest) select a;

                            foreach (MobProfile b in theMobProfiles)
                                if (b.Name != "???")
                                {
                                    if (b.Name == "Ted" && list.Contains("Ted") || b.Name == "Alchemist" && list.Contains("Alchemist")) continue;
                                    else list += b.Name + "\n";
                                }

                            if (list.Length == 0) throw new Exception();

                            text = new string[]
                            {
                                $"Search Results For \"{request}\"",
                                $"The following mobs {((request.Length == 1)? "start with" : "include")} \"***{request}***\"\n\n{list}",
                                ""
                            };
                        }
                        break;

                    case "Item":
                        List<ItemProfile> ItemJSON = JsonConvert.DeserializeObject<List<ItemProfile>>(File.ReadAllText("JSON/Item.json"));
                        if (example != "Search")
                        {
                            IEnumerable<ItemProfile> theItemProfiles = from a in ItemJSON where a.Title == theRequest select a;
                            var ItemResult = theItemProfiles.FirstOrDefault();
                            text = new string[]
                            {
                            ItemResult.Name,
                            $"**CATEGORY**: {ItemResult.Category}",
                            $"**HOW TO OBTAIN**: {ItemResult.Obtain}",
                            $"**USE**: {ItemResult.Use}",
                            $"**DURABILITY**: {ItemResult.Durability}",
                            $"**FOUND IN CHESTS?**: {(ItemResult.Loot? "Yes" : "No")}",
                            $"**RECIPE**: {ItemResult.Recipe}",
                            $"**ADDITIONAL INFORMATION**: {ItemResult.Additional}",
                            ItemResult.PictureURL
                            };
                        }
                        else
                        {
                            IEnumerable<ItemProfile> theItemProfiles = null;

                            if (theRequest.Length == 1)
                                theItemProfiles = from a in ItemJSON where a.Title.StartsWith(theRequest) select a;
                            else
                                theItemProfiles = from a in ItemJSON where a.Title.Contains(theRequest) select a;

                            foreach (ItemProfile b in theItemProfiles)
                                if (b.Name != "???")
                                    list += b.Name + "\n";

                            if (list.Length == 0) throw new Exception();

                            text = new string[]
                            {
                                $"Search Results For \"{request}\"",
                                $"The following items {((request.Length == 1)? "start with" : "include")} \"***{request}***\"\n\n{list}",
                                ""
                            };
                        }
                        break;

                    case "Biome":
                        List<BiomeProfile> BiomeJSON = JsonConvert.DeserializeObject<List<BiomeProfile>>(File.ReadAllText("JSON/Biome.json"));
                        if (example != "Search")
                        {
                            List<MobProfile> MobForBiomeJSON = JsonConvert.DeserializeObject<List<MobProfile>>(File.ReadAllText("JSON/Mob.json"));
                            BiomeProfile BiomeResult = null;

                            foreach (BiomeProfile bp in BiomeJSON)
                            {
                                string[] bpTitle = bp.Title.Split(',');
                                bool match = false;

                                foreach (string t in bpTitle)
                                    if (theRequest == t)
                                    {
                                        match = true;
                                        BiomeResult = bp;
                                        break;
                                    }

                                if (match) break;
                            }

                            if (BiomeResult == null) throw new Exception();
                            string mobList = "";

                            foreach (MobProfile mp in MobForBiomeJSON)
                            {
                                string[] location = mp.Location.Replace(", ", "|").Split('|');

                                bool inBiome = false;

                                foreach (string l in location)
                                    if (l == BiomeResult.Name)
                                    {
                                        mobList += ((mobList == "") ? "" : ", ") + mp.Name;
                                        inBiome = true;
                                        break;
                                    }

                                if (!inBiome && BiomeResult.Alias.Contains(mp.Location)) mobList += ((mobList == "") ? "" : ", ") + mp.Name;
                            }

                            text = new string[]
                            {
                            BiomeResult.Name,
                            $"**DESCRIPTION**: {BiomeResult.Description}",
                            (mobList == "")? "" : $"**CREATURES LOCATED HERE**: {mobList}",
                            BiomeResult.PictureURL
                            };
                        }
                        else
                        {
                            IEnumerable<BiomeProfile> theBiomeProfiles = null;

                            if (theRequest.Length == 1)
                                theBiomeProfiles = from a in BiomeJSON where a.Title.StartsWith(theRequest) select a;
                            else
                                theBiomeProfiles = from a in BiomeJSON where a.Title.Contains(theRequest) select a;

                            foreach (BiomeProfile b in theBiomeProfiles)
                                if (b.Name != "???")
                                    list += b.Name + "\n";

                            if (list.Length == 0) throw new Exception();

                            text = new string[]
                            {
                                $"Search Results For \"{request}\"",
                                $"The following biomes {((request.Length == 1)? "start with" : "include")} \"***{request}***\"\n\n{list}",
                                ""
                            };
                        }
                        break;

                    case "Pet":
                        List<PetProfile> PetJSON = JsonConvert.DeserializeObject<List<PetProfile>>(File.ReadAllText("JSON/Pet.json"));
                        if (example != "Search")
                        {
                            IEnumerable<PetProfile> thePetProfiles = from a in PetJSON where a.Title == theRequest select a;
                            var PetResult = thePetProfiles.FirstOrDefault();
                            text = new string[]
                            {
                            PetResult.Name,
                            $"**DEFAULT NAME**: {PetResult.DefaultName}",
                            $"**ABILITIES**: {PetResult.Abilities}",
                            $"**STARTING HP**: {PetResult.HP}",
                            $"**HP GAIN PER LEVEL**: {PetResult.GrowthHP}",
                            $"**STARTING ATTACK DAMAGE**: {PetResult.Damage}",
                            $"**DAMAGE GAIN PER LEVEL**: {PetResult.GrowthDamage}",
                            (PetResult.ProjectileDamage != 0) ? $"**SECONDARY ATTACK STARTING DAMAGE**: {PetResult.ProjectileDamage}" : "",
                            (PetResult.ProjectileGrowth != 0) ? $"**SECONDARY ATTACK DAMAGE GAIN PER LEVEL**: {PetResult.ProjectileGrowth}" : "",
                            "\n\n**__LEVEL 50 STATS__**",
                            $"**HP**: {PetResult.MaxHP}",
                            $"**ATTACK DAMAGE**: {PetResult.MaxDamage}",
                            (PetResult.ProjectileMax != 0) ? $"**SECONDARY ATTACK DAMAGE**: {PetResult.ProjectileMax}" : "",
                            $"**ADDITIONAL INFORMATION**: {PetResult.Additional}",
                            PetResult.PictureURL
                            };
                        }
                        else
                        {
                            IEnumerable<PetProfile> thePetProfiles = null;

                            if (theRequest.Length == 1)
                                thePetProfiles = from a in PetJSON where a.Title.StartsWith(theRequest) select a;
                            else
                                thePetProfiles = from a in PetJSON where a.Title.Contains(theRequest) select a;

                            foreach (PetProfile b in thePetProfiles)
                                if (b.Name != "???")
                                    list += b.Name + "\n";

                            if (list.Length == 0) throw new Exception();

                            text = new string[]
                            {
                                $"Search Results For \"{request}\"",
                                $"The following items {((request.Length == 1)? "start with" : "include")} \"***{request}***\"\n\n{list}",
                                ""
                            };
                        }
                        break;

                    case "Trait":
                        List<TraitProfile> TraitJSON = JsonConvert.DeserializeObject<List<TraitProfile>>(File.ReadAllText("JSON/Trait.json"));

                        if (theRequest == "TRAITLIST")
                        {
                            foreach (TraitProfile t in TraitJSON)
                                if (t.Name != "???")
                                    list += t.Name + "\n";

                            text = new string[]
                            {
                                "List of Traits",
                                list,
                                "",
                            };
                        }
                        else
                        {
                            IEnumerable<TraitProfile> theTraitProfiles = from a in TraitJSON where a.Title == theRequest select a;
                            var TraitResult = theTraitProfiles.FirstOrDefault();
                            text = new string[]
                            {
                            TraitResult.Name,
                            $"**DESCRIPTION**: {TraitResult.Description}",
                            $"**CREATURES WITH THIS TRAIT**: {TraitResult.Mobs}",
                            ""
                            };
                        }
                        break;
                    case "Drop":
                        DropProfiles dropList = new DropProfiles();
                        BlockProfile blockDrop = dropList.GetBlockProfile(theRequest);
                        if (blockDrop != null && (blockDrop.Name == "Apple Block" || blockDrop.Name == "Coconut Block")) blockDrop = null;
                        ItemProfile itemDrop = dropList.GetItemProfile(theRequest);

                        if (blockDrop == null && itemDrop == null) throw new Exception();

                        string droplist = "";

                        foreach (MobProfile m in dropList.Mobs)
                        {
                            string[] mobDropList = m.Drops.Replace(", ", "|").Split('|');

                            foreach (string s in mobDropList)
                                if (((blockDrop != null && s == blockDrop.Name) || (itemDrop != null && s == itemDrop.Name)) && !droplist.Contains(m.Name))
                                {
                                    droplist += m.Name + "\n";
                                    break;
                                }
                        }

                        string drop = (blockDrop != null) ? blockDrop.Name : itemDrop.Name;
                        if (droplist == "")
                        {
                            await Context.Channel.SendMessageAsync($"There are no creatures that drop the {drop}");
                            return;
                        }

                        text = new string[]
                        {
                            drop,
                            $"The following creatures may possibly drop the {drop} \n\n {droplist}",
                            ""
                        };

                        break;
                    case "Loot":

                        /*if (request == "Block")
                        {
                            List<BlockProfile> BlockLoot = JsonConvert.DeserializeObject<List<BlockProfile>>(File.ReadAllText("JSON/Block.json"));
                            IEnumerable<BlockProfile> LootableBlocks = from a in BlockLoot where a.Loot == true select a;

                            foreach(BlockProfile b in LootableBlocks)
                            {
                                if (b.Name.StartsWith('W'))
                                    list += b.Name + "\n";
                            }
                        }
                        else if (request == "Item")
                        {
                            List<ItemProfile> ItemLoot = JsonConvert.DeserializeObject<List<ItemProfile>>(File.ReadAllText("JSON/Item.json"));
                            IEnumerable<ItemProfile> LootableItems = from a in ItemLoot where a.Loot == true select a;

                            foreach (ItemProfile b in LootableItems)
                            {
                                list += b.Name + "\n";
                            }
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync("<@306956988913287168> Something went wrong with the loot command.");
                            return;
                        }

                        Console.WriteLine(list);

                        text = new string[]
                        {
                                $"Lootable {request}s",
                                $"The following {request}s could be found in chests: \n\n{list}",
                                ""
                        };*/
                        List<BlockProfile> BlockLoot = JsonConvert.DeserializeObject<List<BlockProfile>>(File.ReadAllText("JSON/Block.json"));
                        IEnumerable<BlockProfile> LootableBlocks = from a in BlockLoot where a.Loot == true select a;
                        List<ItemProfile> ItemLoot = JsonConvert.DeserializeObject<List<ItemProfile>>(File.ReadAllText("JSON/Item.json"));
                        IEnumerable<ItemProfile> LootableItems = from a in ItemLoot where a.Loot == true select a;

                        list += "**__BLOCKS__**:\n\n";
                        bool blocksFound = false;
                        foreach (BlockProfile b in LootableBlocks)
                        {
                            if (b.Name.StartsWith(request))
                            {
                                list += $"- {b.Name}\n";
                                blocksFound = true;
                            }
                        }
                        if (!blocksFound) list += "*No blocks found*\n";

                        list += "\n**__ITEMS__**:\n\n";
                        bool itemsFound = false;
                        foreach (ItemProfile b in LootableItems)
                        {
                            if (b.Name.StartsWith(request))
                            {
                                list += $"- {b.Name}\n";
                                itemsFound = true;
                            }
                        }
                        if (!itemsFound) list += "*No items found*\n";

                        text = new string[]
                        {
                                $"Lootables",
                                $"The following blocks and items that begin with {request} could be found in chests: \n\n{list}",
                                ""
                        };

                        break;
                }

                EmbedBuilder theMessage = cf.CreateEmbed(text, theCategory, Context);

                await Context.Channel.SendMessageAsync("", false, theMessage);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync(
                    (example == "Search") ? String.Format(ErrorSearch, theCategory, request, (request.Length == 1) ? "start with" : "include") :
                    String.Format(ErrorStatement, request, theCategory));
            }
        }
    }
}
