using System;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;

namespace ShadowHunter
{
    class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        public static SocketGuild resourceGuild;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            Console.WriteLine("SHADOWHUNTER\n\n");
            _client = client;
            _commands = new CommandService();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

            _client.MessageReceived += HandleCommandAsync;
            _client.Ready += GetGuildData;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;


            var Context = new SocketCommandContext(_client, msg);
            if (Context.User.IsBot) return;

            int argPos = 0;

            if (msg.HasCharPrefix('!', ref argPos)) //command prefix
            {
                var result = await _commands.ExecuteAsync(Context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand) //if an error happens
                {
                    Console.WriteLine(result.ErrorReason); //error result
                }
            }
        }

        private async Task GetGuildData()
        {
            resourceGuild = await Task.Run(() => _client.GetGuild(639289227934171167));
        }
    }
}
