using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace IsleBot;

public class IsleBot {
    private Config config;
    private DiscordSocketClient client;
    Random rng = new Random();
    
    public static Task Main(string[] args) {
        var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
        var islebot = new IsleBot(config);
        return islebot.MainAsync();
    }

    public async Task MainAsync() {
        client = new DiscordSocketClient(
            new DiscordSocketConfig { 
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 500,
                GatewayIntents = GatewayIntents.AllUnprivileged
            }   
        );
        
        client.Log += Log;
        client.Connected += () => {
            Console.WriteLine("Connection Estabilished");
            return Task.CompletedTask;
        };
        client.Disconnected += (e) => {
            Console.WriteLine("Connection Lost");
            return Task.CompletedTask;
        };
        
        client.Ready += Client_OnReady;

        client.SlashCommandExecuted += SlashCommandHandler;
        
        await client.LoginAsync(TokenType.Bot, config.DiscordToken);
        await client.StartAsync();
        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task Client_OnReady() {
        client.Guilds.ToList().ForEach(guild =>
        {
            var guildGetCard = new SlashCommandBuilder()
                .WithName("get-card")
                .WithDescription("Get/Replace your current card");
            
            var attackCommand = new SlashCommandBuilder()
                .WithName("attack")
                .WithDescription("Attack a player")
                .AddOption("target", ApplicationCommandOptionType.User, "The target of the attack");

            try {
                guild.CreateApplicationCommandAsync(guildGetCard.Build());
                guild.CreateApplicationCommandAsync(attackCommand.Build());
            } catch (HttpException exception) {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        });
    }

    private async Task SlashCommandHandler(SocketSlashCommand command) {
        switch (command.Data.Name) {
            case "get-card":
                var card = GenerateCard();
                await command.RespondAsync("You got a new card!\n " + card);
                break;
            case "attack":
                var target = command.Data.Options.First().Value as SocketUser;
                await command.RespondAsync($"You attacked {target.Username}!");
                break;
        }
    }
    
    private Card GenerateCard() {
        var card = new Card($"Test Card N. {rng.Next(0, 1000)}",
            rng.Next(1, 20), rng.Next(0, 10),
            rng.Next(25, 100));
        return card;
    }

    private void Attack(ref Card attacker, ref Card defender) {
        defender.CurrentHealth -= Math.Clamp(attacker.Attack - defender.Defense, 0, int.MaxValue);
    }
    
    private Task Log(LogMessage msg) {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public IsleBot(Config config) {
        this.config = config;
    }
}
