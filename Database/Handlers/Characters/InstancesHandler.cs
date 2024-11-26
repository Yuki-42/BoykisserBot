using BoykisserBot.Database.Types.Characters;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Handlers.Characters;

public class InstancesHandler(string connectionString) : BaseHandler(connectionString)
{
    public async Task<InstancesRow?> Get(ulong id)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM characters.instances WHERE id = @id";
        command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)id });

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        return !reader.Read() ? null : new InstancesRow(ConnectionString, Handlers, reader);
    }
}