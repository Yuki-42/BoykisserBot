using BoykisserBot.Database.Types.Users;
using DisCatSharp.Entities;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Handlers.Discord;

public class UsersHandler(string connectionString) : BaseHandler(connectionString)
{
    public async Task<UsersRow?> Get(ulong id)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM discord.users WHERE id = @id";
        command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)id });

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        return !reader.Read() ? null : new UsersRow(ConnectionString, Config!, Handlers, reader);
    }

    /// <summary>
    ///     Adds a new user to the database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<UsersRow> Get(ulong id, string username)
    {
        // Check if the user already exists.
        UsersRow? user = await Get(id);
        if (user != null) return user;

        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "INSERT INTO discord.users (id, username) VALUES (@id, @username)";
        command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)id });
        command.Parameters.AddWithValue("username", username);

        await ExecuteNonQuery(command);

        return await Get(id) ?? throw new MissingMemberException();
    }


    public async Task<UsersRow> Get(DiscordUser user)
    {
        return await Get(user.Id, user.Username);
    }

    public async Task<IReadOnlyList<UsersRow>> GetAll()
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM discord.users;";

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        List<UsersRow> users = [];
        while (await reader.ReadAsync()) users.Add(new UsersRow(ConnectionString, Config!, Handlers, reader));

        return users;
    }
}