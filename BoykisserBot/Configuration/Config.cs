using Microsoft.Extensions.Configuration;

namespace BoykisserBot.Configuration;

/// <summary>
///     Bot configuration.
/// </summary>
public class BotConfig(string token, ulong testingGuild)
{
    public readonly ulong TestingGuild = testingGuild;

    /// <summary>
    ///     Bot token.
    /// </summary>
    public readonly string Token = token;
}

/// <summary>
///     Database configuration.
/// </summary>
public class DatabaseConfig(string host, int port, string username, string name, string password)
{
    public readonly string Host = host;
    public readonly string Name = name;
    public readonly string Password = password;
    public readonly int Port = port;
    public readonly string Username = username;
}

public class LoggingConfig(ulong logsChannel)
{
    /// <summary>
    ///     Discord channel for the bot to send log messages to (normally just errors).
    /// </summary>
    public readonly ulong LogsChannel = logsChannel;
}

public class FilesystemConfig(string prototypes)
{
    /// <summary>
    ///     Directory for prototype images.
    /// </summary>
    public readonly DirectoryInfo Prototypes = new(prototypes);
}

public class Config(IConfiguration config)
{
    /// <summary>
    ///     Bot config.
    /// </summary>
    public BotConfig Bot { get; } = new(
        config["Bot:Token"] ?? throw new MissingFieldException("Bot:Token"),
        ulong.Parse(config["Bot:TestingGuild"] ?? throw new MissingFieldException("Bot:TestingChannel"))
    );

    /// <summary>
    ///     Logging config.
    /// </summary>
    public LoggingConfig Logging { get; } = new(
        ulong.Parse(config["Logging:Channel"] ?? throw new MissingFieldException("Logging:LogsChannel"))
    );

    /// <summary>
    ///     Database config.
    /// </summary>
    public DatabaseConfig Database { get; } = new(
        config["Database:Host"] ?? throw new MissingFieldException("Database:Host"),
        int.Parse(config["Database:Port"] ?? throw new MissingFieldException("Database:Port")),
        config["Database:Username"] ?? throw new MissingFieldException("Database:Username"),
        config["Database:Name"] ?? throw new MissingFieldException("Database:Name"),
        config["Database:Password"] ?? throw new MissingFieldException("Database:Password")
    );

    /// <summary>
    ///     Filesystem config.
    /// </summary>
    public FilesystemConfig Filesystem { get; } = new(
        config["Filesystem:Prototypes"] ?? throw new MissingFieldException("Filesystem:Prototypes")
    );
}