using BoykisserBot.Database.Types.Common;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Handlers.Common;

public class RaritiesHandler(string connectionString) : BaseHandler(connectionString)
{
    public async Task<IEnumerable<RaritiesRow>> Get()
    {
        await using NpgsqlConnection connection = new(connectionString);
        await connection.OpenAsync();

        await using NpgsqlCommand command = new();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM common.rarities;";

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        List<RaritiesRow> rarities = [];
        while (await reader.ReadAsync())
        {
            rarities.Add(new RaritiesRow(connectionString, Handlers, reader));
        }

        return rarities;
    }

    public async Task<RaritiesRow> Get(Guid id)
    {
        await using NpgsqlConnection connection = new(connectionString);
        await connection.OpenAsync();

        await using NpgsqlCommand command = new();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM common.rarities WHERE id = @id;";
        command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = id });

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();

        return new RaritiesRow(connectionString, Handlers, reader);
    }

    public async Task<RaritiesRow> Get(string name)
    {
        await using NpgsqlConnection connection = new(connectionString);
        await connection.OpenAsync();

        await using NpgsqlCommand command = new();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM common.rarities WHERE name = @name;";
        command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Text) { Value = name });

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();

        return new RaritiesRow(connectionString, Handlers, reader);
    }
}