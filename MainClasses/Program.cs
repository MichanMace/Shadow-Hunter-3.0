using System;
using System.Threading;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ShadowHunter
{
    class Program
    {
        Random rand = new Random();

        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public async Task StartAsync()
        {

            _client = new DiscordSocketClient();
            //new CommandHandler();

            //**********SHADOW HUNTER TOKEN**********\\
            await _client.LoginAsync(TokenType.Bot, "Shadow Hunter Token Here");

            //**********LATIBOT TOKEN**********\\
            //await _client.LoginAsync(TokenType.Bot, "Test Bot Token Here");

            await _client.StartAsync();

            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await _client.SetGameAsync("Block Story | !commands");
            while (true)
            {
                /*int currentGame = rand.Next(Games.Length);
                await _client.SetGameAsync(Games[currentGame]);
                Thread.Sleep(30000);*/
                /*_client.Ready += () =>
                {
                    return Task.CompletedTask;
                };*/
            }
        }
    }
}
