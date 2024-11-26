using System.Data;
using BoykisserBot.Database.Types.Characters;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Types.Expeditions;

public class ExpeditionsRow(string connectionString, HandlersGroup handlers, IDataRecord reader)
    : BaseRow(connectionString, handlers, reader)
{
    public string Name
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM expeditions.expeditions WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (string)command.ExecuteScalar()!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE expeditions.expeditions SET name = @value WHERE id = @id;";

            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("value", DbType.String) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    public string Description
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT description FROM expeditions.expeditions WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (string)command.ExecuteScalar()!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE expeditions.expeditions SET description = @value WHERE id = @id;";

            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("value", DbType.String) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    public DateTime StartedAt
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT started_at FROM expeditions.expeditions WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (DateTime)command.ExecuteScalar()!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE expeditions.expeditions SET started_at = @value WHERE id = @id;";

            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("value", DbType.DateTime) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    public DateTime EndsAt
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT ends_at FROM expeditions.expeditions WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (DateTime)command.ExecuteScalar()!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE expeditions.expeditions SET ends_at = @value WHERE id = @id;";

            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("value", DbType.DateTime) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    public bool Completed
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT completed FROM expeditions.expeditions WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (bool)command.ExecuteScalar()!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE expeditions.expeditions SET completed = @value WHERE id = @id;";

            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("value", DbType.Boolean) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    public Guid[] Characters
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT characters FROM expeditions.expeditions WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            using NpgsqlDataReader reader = command.ExecuteReader();
            return reader.GetFieldValue<Guid[]>(0);
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE expeditions.expeditions SET characters = @value WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Array) { Value = value });

            ExecuteNonQuery(command);
        }
    }

    public async Task<IEnumerable<PrototypesRow>> GetPrototypes()
    {
        await using NpgsqlConnection connection = await GetConnectionAsync();
        await using NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT characters FROM expeditions.expeditions WHERE id = @id;";
        command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

        await using NpgsqlDataReader reader = command.ExecuteReader();
        Guid[] characterIds = reader.GetFieldValue<Guid[]>(0);

        List<PrototypesRow> prototypes = [];
        foreach (Guid characterId in characterIds)
        {
            prototypes.Add(await Handlers.Characters.Prototypes.Get(characterId));
        }

        return prototypes;
    }
}