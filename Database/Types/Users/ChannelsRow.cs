using System.Data;
using DisCatSharp.Enums;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Types.Users;

public class ChannelsRow(string connectionString, HandlersGroup handlersGroup, IDataRecord record)
    : BaseRow(connectionString, handlersGroup, record)
{
    /// <summary>
    ///     User's discord id.
    /// </summary>
    public new ulong Id { get; } = (ulong)record.GetInt64(record.GetOrdinal("id"));

    /// <summary>
    ///     Guild id.
    /// </summary>
    public ulong GuildId { get; } = (ulong)record.GetInt64(record.GetOrdinal("guild_id"));

    /// <summary>
    ///     If the channel has message tracking enabled.
    /// </summary>

    public string Name
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM discord.channels WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)Id });

            return (command.ExecuteScalar() as string)!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE discord.channels SET name = @value WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)Id });

            command.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Text) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    public ChannelType Type
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT type FROM discord.channels WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)Id });

            // Convert the string to ChannelType using TryParse
            ChannelType type;

            return Enum.TryParse((command.ExecuteScalar() as string)!, out type) ? type : ChannelType.Unknown;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE discord.channels SET type = @value WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)Id });

            command.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Text) { Value = value.ToString() });
            ExecuteNonQuery(command);
        }
    }

    public async Task Delete()
    {
        await using NpgsqlConnection connection = await GetConnectionAsync();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM discord.channels WHERE id = @id;";
        command.Parameters.Add(new NpgsqlParameter("id", DbType.VarNumeric) { Value = (long)Id });

        await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    ///     Associated guild object.
    /// </summary>
    /// <returns>Guild</returns>
    public async Task<GuildsRow?> GetGuild()
    {
        return await Handlers.Discord.Guilds.Get(GuildId);
    }
}