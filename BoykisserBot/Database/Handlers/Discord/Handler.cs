﻿namespace BoykisserBot.Database.Handlers.Discord;

public class Handler(string connectionString) : BaseHandler(connectionString)
{
    /// <summary>
    ///     Guilds handler
    /// </summary>
    public GuildsHandler Guilds { get; private set; } = new(connectionString);

    /// <summary>
    ///     Users handler
    /// </summary>
    public UsersHandler Users { get; private set; } = new(connectionString);

    /// <summary>
    ///     Channels handler
    /// </summary>
    public ChannelsHandler Channels { get; private set; } = new(connectionString);

    /// <summary>
    ///     Guilds users handler
    /// </summary>
    public GuildsUsersHandler GuildUsers { get; private set; } = new(connectionString);

    /// <summary>
    ///     Channels users handler
    /// </summary>
    public ChannelsUsersHandler ChannelUsers { get; private set; } = new(connectionString);
}