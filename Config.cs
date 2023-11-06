namespace IsleBot;

public class Config
{
    public string DiscordToken { get; set; }

    public Config(string discordToken)
    {
        DiscordToken = discordToken;
    }
}