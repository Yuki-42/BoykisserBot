using System.Data;
using Npgsql;

namespace BoykisserBot.Database.Types.Users;

public class ChannelsUsersRow(
    string connectionString,
    Configuration.Config config,
    HandlersGroup handlersGroup,
    IDataRecord record)
    : BaseRow(connectionString, config, handlersGroup, record)
{
    public ulong UserId { get; } = (ulong)record.GetInt64(record.GetOrdinal("user_id"));
    public ulong ChannelId { get; } = (ulong)record.GetInt64(record.GetOrdinal("channel_id"));

    public async Task<UsersRow?> GetUser()
    {
        return await Handlers.Discord.Users.Get(UserId) ?? throw new MissingMemberException();
    }

    public async Task<ChannelsRow?> GetChannel()
    {
        return await Handlers.Discord.Channels.NGet(ChannelId, null) ?? throw new MissingMemberException();
    }

    public async Task Delete()
    {
        await using NpgsqlConnection connection = await GetConnectionAsync();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM discord.channels_users WHERE id = @id;";
        command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

        await command.ExecuteNonQueryAsync();
    }
}