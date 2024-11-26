using BoykisserBot.Database.Types.Characters;
using Npgsql;

namespace BoykisserBot.Database.Handlers.Characters;

public class PrototypesHandler(string connectionString) : BaseHandler(connectionString)
{
    public async Task<PrototypesRow?> Get(Guid id)
    {
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlCommand command = new("SELECT * FROM characters.prototypes WHERE id = @id", connection);
        command.Parameters.AddWithValue("id", id);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        return !await reader.ReadAsync() ? null : new PrototypesRow(ConnectionString, Handlers, reader);
    }
}