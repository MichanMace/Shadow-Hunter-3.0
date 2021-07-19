using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using ShadowHunter.Common;
using Discord.WebSocket;
using Discord.Rest;
using ShadowHunter;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace TheCommands
{
    public class OtherCommands : ModuleBase<SocketCommandContext>
    {
        CommonFunctions cf = new CommonFunctions();
        Random rand = new Random();
        SocketGuild rg = CommandHandler.resourceGuild;

        [Command("SendNudes")]
        public async Task SendNudes()
        {
            await Context.Channel.SendMessageAsync("Bitch! I ain't doing that!");
        }

        [Command("Credits")]
        public async Task Credits()
        {
            string[] text =
            {
                "Acknowledgements",
                "the \"Shadow Hunter\" bot is a fan made bot developed and programmed by <@306956988913287168>.",
                "Despite being a wiki bot for Block Story, it has no official association to Block Story or MindBlocks.",
                "Additionally, Shadow Hunter would not be possible without the help of these community members:" +
                "\n<@691430188898189393> - Helped with the ``!mob`` command" +
                "\n<@542519622339919874> - Helped with the ``!mob`` command" +
                "\n<@299412261124964353> - Helped with the ``!mob`` command" +
                "\n<@351767983171043330> - Resources used as reference for information commands" +
                "\n<@277275829145305088> - Photo credits" +
                "\n<@588454080813203468> - Photo credits",
                "A very special thanks goes out to all the members of Block Story's community! Thanks for all the love and support during development of this bot." +
                " (And thanks to everyone who reported bugs and information errors too!)",
                ""
            };

            EmbedBuilder theMessage = cf.CreateEmbed(text, "Credits", Context);

            await Context.Channel.SendMessageAsync("", false, theMessage);
        }

        [Command("Showcase")]
        public async Task Showcase()
        {
            var loading = await Context.Channel.SendMessageAsync("Please wait while I look for something to show off...");
            IReadOnlyCollection<RestMessage>[] messagesToShowcase = new IReadOnlyCollection<RestMessage>[] 
            {
                await (rg.GetChannel(745727817144205433) as ISocketMessageChannel).GetPinnedMessagesAsync(),
                await (rg.GetChannel(746118695163527270) as ISocketMessageChannel).GetPinnedMessagesAsync(),
            };

            List<RestMessage> allMessages = new List<RestMessage>();

            foreach(var v in messagesToShowcase)
            {
                RestMessage[] theMessages = v.ToArray();

                foreach(RestMessage rm in theMessages)
                    allMessages.Add(rm);
            }

            RestMessage[] Choices = allMessages.ToArray();
            RestMessage m = Choices[rand.Next(0, Choices.Length)];
            var a = m.Attachments.First();
            string[] text = new string[]
            {
                "Showcase",
                "-",
                m.Content,
                a.Url
            };

            EmbedBuilder theMessage = cf.CreateEmbed(text, "Showcase", Context);
            await Context.Channel.SendMessageAsync("", false, theMessage);
            await loading.DeleteAsync();
        }

        [Command("Meme")]
        public async Task FreshestMemes()
        {
            if (Context.Guild.Id == 312381400861114369 && Context.Channel.Id != 554700239244754954)
            {
                await Context.Channel.SendMessageAsync("This command only works in <#554700239244754954>.");
                return;
            }
            var loading = await Context.Channel.SendMessageAsync("Please wait while I find the freshest meme...");
            IReadOnlyCollection<RestMessage>[] messagesToShowcase = new IReadOnlyCollection<RestMessage>[]
            {
                await (rg.GetChannel(659380880091119627) as ISocketMessageChannel).GetPinnedMessagesAsync(),
                await (rg.GetChannel(748208009330688038) as ISocketMessageChannel).GetPinnedMessagesAsync(),
            };

            List<RestMessage> allMessages = new List<RestMessage>();

            foreach (var v in messagesToShowcase)
            {
                RestMessage[] theMessages = v.ToArray();

                foreach (RestMessage rm in theMessages)
                    allMessages.Add(rm);
            }

            RestMessage[] Choices = allMessages.ToArray();
            RestMessage m = Choices[rand.Next(0, Choices.Length)];
            var a = m.Attachments.First();
            string[] text = new string[]
            {
                "Meme",
                m.Content,
                a.Url
            };

            EmbedBuilder theMessage = cf.CreateEmbed(text, "Meme", Context);
            await Context.Channel.SendMessageAsync("", false, theMessage);
            await loading.DeleteAsync();
        }
        [Command("Useless")]
        public async Task Uselessfeatures([Remainder] string f)
        {
            if (f.Length == 0) return;

            TextWriter tw = new StreamWriter("_UselessFeatures.txt", true);
            tw.WriteLine(f);
            tw.Close();

            await Context.Channel.SendMessageAsync("Your feature has been logged");
        }
    }
}
