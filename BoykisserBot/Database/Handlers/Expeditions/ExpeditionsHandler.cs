using BoykisserBot.Database.Types.Expeditions;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Handlers.Expeditions;

public class ExpeditionsHandler(string connectionString) : BaseHandler(connectionString)
{
    public async Task<ExpeditionsRow?> Get(ulong id)
    {
        // Get a new connection
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM expeditions.expeditions WHERE id = @id";
        command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Numeric) { Value = (long)id });

        await using NpgsqlDataReader reader = await ExecuteReader(command);
        return !reader.Read() ? null : new ExpeditionsRow(ConnectionString, Config!, Handlers, reader);
    }
}