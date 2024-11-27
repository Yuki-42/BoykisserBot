using System.Data;
using BoykisserBot.Database.Types.Common;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Types.Characters;

public class PrototypesRow(
    string connectionString,
    Configuration.Config config,
    HandlersGroup handlersGroup,
    IDataRecord record)
    : BaseRow(connectionString, config, handlersGroup, record)
{
    /// <summary>
    ///     Filetype of the image.
    /// </summary>
    private string FileType { get; } = record.GetString(record.GetOrdinal("file_type"));

    /// <summary>
    ///     Image for prototype.
    /// </summary>
    public FileInfo Image => new(Config!.Filesystem.Prototypes + $"{Id.ToString()}.{FileType}");

    /// <summary>
    ///     Name of the prototype.
    /// </summary>
    public string Name
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM characters.prototypes WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (string)command.ExecuteScalar()!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE characters.prototypes SET name = @value WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            command.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Text) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    /// <summary>
    ///     Prototype description.
    /// </summary>
    public string Description
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT description FROM characters.prototypes WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (string)command.ExecuteScalar()!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE characters.prototypes SET description = @value WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            command.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Text) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    /// <summary>
    ///     Rarity of the prototype.
    /// </summary>
    public Guid RarityId
    {
        get
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT rarity_id FROM characters.prototypes WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            return (Guid)command.ExecuteScalar()!;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE characters.prototypes SET rarity_id = @value WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Guid) { Value = Id });

            command.Parameters.Add(new NpgsqlParameter("value", DbType.Guid) { Value = value });
            ExecuteNonQuery(command);
        }
    }

    /// <summary>
    ///     Gets the rarity row for the prototype.
    /// </summary>
    /// <returns>Rarity</returns>
    public async Task<RaritiesRow?> GetRarity()
    {
        return await Handlers.Common.Rarities.Get(RarityId);
    }
}