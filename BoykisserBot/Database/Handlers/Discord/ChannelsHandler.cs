using BoykisserBot.Database.Types.Users;
using DisCatSharp.Entities;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Handlers.Discord;

public class ChannelsHandler(string connectionString) : BaseHandler(connectionString)
{
    public async Task<ChannelsRow?> Get(ulong id)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM discord.channels WHERE id = @id";
        command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)id });

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        return !reader.Read() ? null : new ChannelsRow(ConnectionString, Config!, Handlers, reader);
    }

    /// <summary>
    ///     Gets a channel from the database, or if it does not exist, creates it.
    /// </summary>
    /// <param name="id">Discord ID for the channel</param>
    /// <param name="guildId">Discord ID for the guild</param>
    /// <param name="name">Channel name</param>
    /// <exception cref="ArgumentNullException">
    ///     Name is null and the channel was not found. Cannot add a channel without a
    ///     provided name.
    /// </exception>
    /// <exception cref="MissingMemberException">Database Add failed. Check logs for reason.</exception>
    /// <returns>ChannelRow of the channel</returns>
    public async Task<ChannelsRow> NGet(ulong id, ulong? guildId, string? name = null)
    {
        // Check if the channel already exists.
        ChannelsRow? channel = await Get(id);
        if (channel != null) return channel;

        // The channel does not exist in the db, add it.
        ArgumentNullException.ThrowIfNull(name);

        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();

        if (guildId == null)
        {
            command.CommandText = "INSERT INTO discord.channels (id, name) VALUES (@id, @name)";
        }
        else
        {
            command.CommandText = "INSERT INTO discord.channels (id, guild_id, name) VALUES (@id, @guild_id, @name)";
            command.Parameters.Add(new NpgsqlParameter("guild_id", NpgsqlDbType.Numeric) { Value = (long)guildId });
        }

        command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)id });
        command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Text) { Value = name });

        await ExecuteNonQuery(command);

        return await Get(id) ?? throw new MissingMemberException();
    }

    public async Task<ChannelsRow> NGet(DiscordChannel channel)
    {
        return await NGet(channel.Id, channel.GuildId, channel.Name);
    }

    public async Task<IReadOnlyList<ChannelsRow>> GetAll()
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM discord.channels";

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        List<ChannelsRow> channels = [];
        while (await reader.ReadAsync()) channels.Add(new ChannelsRow(ConnectionString, Config!, Handlers, reader));

        return channels;
    }

    public async Task<IReadOnlyList<ChannelsRow>> GetAll(ulong guildId)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM discord.channels WHERE guild_id = @guild_id";
        command.Parameters.Add(new NpgsqlParameter("guild_id", NpgsqlDbType.Numeric) { Value = (long)guildId });

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        List<ChannelsRow> channels = [];
        while (await reader.ReadAsync()) channels.Add(new ChannelsRow(ConnectionString, Config!, Handlers, reader));

        return channels;
    }
}