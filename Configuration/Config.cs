using Microsoft.Extensions.Configuration;

namespace BoykisserBot.Configuration;

/// <summary>
///     Bot configuration.
/// </summary>
public class Bot(string token, ulong testingGuild)
{
    /// <summary>
    ///     Bot token.
    /// </summary>
    public readonly string Token = token;
}

public class Logging(ulong logsChannel)
{
    /// <summary>
    ///     Discord channel for the bot to send log messages to (normally just errors).
    /// </summary>
    public readonly ulong LogsChannel = logsChannel;
}

public class Config(IConfiguration config)
{
    public Bot Bot { get; } = new(
        config["Bot:Token"] ?? throw new MissingFieldException("Bot:Token"),
        ulong.Parse(config["Bot:TestingGuild"] ?? throw new MissingFieldException("Bot:TestingChannel"))
    );

    public Logging Logging { get; } = new(
        ulong.Parse(config["Logging:Channel"] ?? throw new MissingFieldException("Logging:LogsChannel"))
    );
}