﻿using System.Data;
using Npgsql;

namespace BoykisserBot.Database.Types.Users;

public class GuildsUsersRow(string connectionString, HandlersGroup handlersGroup, IDataRecord record)
    : BaseRow(connectionString, handlersGroup, record)
{
    public ulong UserId { get; } = (ulong)record.GetInt64(record.GetOrdinal("user_id"));
    public ulong GuildId { get; } = (ulong)record.GetInt64(record.GetOrdinal("guild_id"));


    public async Task<UsersRow?> GetUser()
    {
        return await Handlers.Discord.Users.Get(UserId) ?? throw new MissingMemberException();
    }

    public async Task<GuildsRow?> GetGuild()
    {
        return await Handlers.Discord.Guilds.Get(GuildId) ?? throw new MissingMemberException();
    }

    public async Task Delete()
    {
        await using NpgsqlConnection connection = await GetConnectionAsync();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM discord.guilds_users WHERE id = @id;";
        command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

        await command.ExecuteNonQueryAsync();
    }
}