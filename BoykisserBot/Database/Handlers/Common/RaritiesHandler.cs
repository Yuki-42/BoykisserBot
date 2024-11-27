using BoykisserBot.Database.Types.Common;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Handlers.Common;

public class RaritiesHandler(string connectionString) : BaseHandler(connectionString)
{
    // TODO: All of this code is probably going to throw tons of errors because I just let Github Copilot write it for me.
    public async Task<IEnumerable<RaritiesRow>> Get()
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM common.rarities;";

        await using NpgsqlDataReader reader = await ExecuteReader(command);

        List<RaritiesRow> rarities = [];
        while (await reader.ReadAsync()) rarities.Add(new RaritiesRow(ConnectionString, Config!, Handlers, reader));

        return rarities;
    }

    public async Task<RaritiesRow> Get(Guid id)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();

        command.CommandText = "SELECT * FROM common.rarities WHERE id = @id;";
        command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = id });

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        await reader.ReadAsync();

        return new RaritiesRow(ConnectionString, Config!, Handlers, reader);
    }

    public async Task<RaritiesRow> Get(string name)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();

        command.CommandText = "SELECT * FROM common.rarities WHERE name = @name;";
        command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Text) { Value = name });

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        await reader.ReadAsync();

        return new RaritiesRow(ConnectionString, Config!, Handlers, reader);
    }
}