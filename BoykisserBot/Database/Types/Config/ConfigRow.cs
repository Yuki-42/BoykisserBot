﻿using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Types.Config;

public class ConfigRow(
    string connectionString,
    Configuration.Config config,
    HandlersGroup handlersGroup,
    IDataRecord record)
    : BaseRow(connectionString, config, handlersGroup, record)
{
    /// <summary>
    ///     Configuration string key.
    /// </summary>
    public string Key { get; } = record.GetString(record.GetOrdinal("key"));

    /// <summary>
    ///     Configuration string value.
    /// </summary>
    public string? Value
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT value FROM config.data WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            dynamic? result = command.ExecuteScalar();
            return result is DBNull ? null : result;
        }
        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE config.data SET value = @value WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            command.Parameters.Add(new NpgsqlParameter("value", NpgsqlDbType.Text)
                { Value = value }); // TODO: Add error handling for when value is null
            ExecuteNonQuery(command);
        }
    }
}