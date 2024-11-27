using System.Data;
using BoykisserBot.Database.Types.Config;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Handlers.Config;

public class Handler(string connectionString) : BaseHandler(connectionString)
{
    /// <summary>
    ///     Get a configuration entry by ID.
    /// </summary>
    /// <param name="id">Row ID</param>
    /// <returns>Row if present</returns>
    public async Task<ConfigRow?> Get(Guid id)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM config.data WHERE id = @id;";
        command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = id });

        await using NpgsqlDataReader reader = await ExecuteReader(command);

        return !await reader.ReadAsync() ? null : new ConfigRow(ConnectionString, Config!, Handlers, reader);
    }

    /// <summary>
    ///     Get a configuration entry by key.
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns></returns>
    public async Task<ConfigRow?> Get(string key)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM config.data WHERE key = @key;";
        command.Parameters.Add(new NpgsqlParameter("key", DbType.String) { Value = key });

        await using NpgsqlDataReader? reader = await ExecuteReader(command);

        return !reader.Read() ? null : new ConfigRow(ConnectionString, Config!, Handlers, reader);
    }

    /// <summary>
    ///     Creates a new configuration entry in the database or gets an existing one.
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns></returns>
    public async Task<ConfigRow> NGet(string key)
    {
        ConfigRow? data = await Get(key);
        if (data is not null) return data;

        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();

        command.CommandText = "INSERT INTO config.data (key) VALUES (@key) RETURNING id;";
        command.Parameters.Add(new NpgsqlParameter("key", NpgsqlDbType.Text) { Value = key });

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        await reader.ReadAsync();

        return new ConfigRow(ConnectionString, Config!, Handlers, reader);
    }
}