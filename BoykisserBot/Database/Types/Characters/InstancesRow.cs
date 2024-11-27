using System.Data;
using Npgsql;

namespace BoykisserBot.Database.Types.Characters;

public class InstancesRow(
    string connectionString,
    Configuration.Config config,
    HandlersGroup handlersGroup,
    IDataRecord record)
    : BaseRow(connectionString, config, handlersGroup, record)
{
    public Guid PrototypeId { get; } = record.GetGuid(record.GetOrdinal("prototype_id"));
    public Guid UserId { get; } = record.GetGuid(record.GetOrdinal("user_id"));

    public int ExpeditionCount
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT expedition_count FROM characters.instances WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (int)command.ExecuteScalar()!;
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE characters.instances SET expedition_count = @value WHERE id = @id;";

            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("value", DbType.Int32) { Value = value });
            ExecuteNonQuery(command);
        }
    }
}