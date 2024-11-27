using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Types.Users;

public class GuildsRow(
    string connectionString,
    Configuration.Config config,
    HandlersGroup handlersGroup,
    IDataRecord record)
    : BaseRow(connectionString, config, handlersGroup, record)
{
    /// <summary>
    ///     User's discord id.
    /// </summary>
    public new ulong Id { get; } = (ulong)record.GetInt64(record.GetOrdinal("id"));

    /// <summary>
    ///     Guild name.
    /// </summary>
    public string Name
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM discord.guilds WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)Id });

            return (command.ExecuteScalar() as string)!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE discord.guilds SET name = @value WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)Id });

            command.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Text) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    public async Task Delete()
    {
        await using NpgsqlConnection connection = await GetConnectionAsync();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM discord.guilds WHERE id = @id;";
        command.Parameters.Add(new NpgsqlParameter("id", DbType.VarNumeric) { Value = (long)Id });

        await command.ExecuteNonQueryAsync();
    }
}