using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace IsleBot;

public class IsleBot {
    private Config config;
    private DiscordSocketClient client;
    Random rng = new();
    private DbManager db = new();
    
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
            // Guild Commands
            List<SlashCommandProperties> guildCommands = new();
            
            guildCommands.Add(new SlashCommandBuilder()
                .WithName("register-player")
                .WithDescription("Register yourself as a player")
                .Build());
            
            guildCommands.Add(new SlashCommandBuilder()
                .WithName("profile")
                .WithDescription("See your profile")
                .Build());

            guildCommands.Add(new SlashCommandBuilder()
                .WithName("get-card")
                .WithDescription("Get/Replace your current card")
                .Build());

            guildCommands.Add(new SlashCommandBuilder()
                .WithName("attack")
                .WithDescription("Attack a player")
                .AddOption("target", ApplicationCommandOptionType.User, "The target of the attack")
                .Build());

            try {
                var commands = guild.GetApplicationCommandsAsync().Result.ToList();
                /*
                guildCommands.ForEach(command => {
                    //if the command already exists and is duplicated
                    if(commands.FindAll(oldCommand => oldCommand.Name == (string)command.Name)?.Count > 1) {
                        guild.DeleteApplicationCommandsAsync(); //delete all commands
                        //else if the command doesn't already exists
                    }else if(commands.Find(oldCommand => oldCommand.Name == (string)command.Name) == null) {
                        guild.CreateApplicationCommandAsync(command); //create it
                    }//otherise, do nothing.
                });*/
                guild.BulkOverwriteApplicationCommandAsync(guildCommands.ToArray());
            } catch (HttpException exception) {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        });
    }

    private async Task SlashCommandHandler(SocketSlashCommand command) {
        switch (command.Data.Name) {
            case "register-player":
                if (db.GetPlayerById(command.User.Id) == null) {
                    var user = new User(command.User.Id, command.User.GlobalName);
                    db.AddPlayer(user);
                    await command.RespondAsync("You are now registered. In future you will also be able to see your profile page.");
                } else {
                    await command.RespondAsync("You are already registered. In future you will also be able to see your profile page.");
                }
                break;
            case "profile":
                if(CheckIfPlayerExists(command.User.Id) == false) {
                    await command.RespondAsync("You are not registered. Use /register-player to register yourself.");
                } else {
                    await command.RespondAsync(db.GetPlayerById(command.User.Id)!.ToString());
                }
                break;
            case "get-card":
                var card = GenerateCard();
                if (CheckIfPlayerExists(command.User.Id) == false) {
                    await command.RespondAsync("You are not registered. Use /register-player to register yourself.");
                } else {
                    var player = db.GetPlayerById(command.User.Id)!;
                    player.Cards.Add(card);
                    db.UpdatePlayer(player);
                    await command.RespondAsync($"You got a new card: \n{card.ToFormattedString()}");
                }
                break;
            case "attack":
                var target = command.Data.Options.First().Value as SocketUser;
                await command.RespondAsync($"You attacked {target.Username}!");
                break;
        }
    }
    
    private bool CheckIfPlayerExists(ulong id) {
        return db.GetPlayerById(id) != null;
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
