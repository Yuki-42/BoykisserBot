using System.Data;
using BoykisserBot.Common;
using Npgsql;
using NpgsqlTypes;

namespace BoykisserBot.Database.Types.Common;

public class RaritiesRow(string connectionString, HandlersGroup handlers, IDataRecord record)
    : BaseRow(connectionString, handlers, record)
{
    /// <summary>
    /// Rarity name.
    /// </summary>
    public string Name
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return (string)command.ExecuteScalar()!;
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET name = @name WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Text) { Value = value });

            ExecuteNonQuery(command);
        }
    }

    /// <summary>
    /// Rarity colour (hex).
    /// </summary>
    public Colour Colour
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT colour FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return new Colour((string)command.ExecuteScalar()!);
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET colour = @colour WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("colour", NpgsqlDbType.Text) { Value = value });

            ExecuteNonQuery(command);
        }
    }

    public int Weight
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT weight FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return (int)command.ExecuteScalar()!;
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET weight = @weight WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("weight", NpgsqlDbType.Integer) { Value = value });

            ExecuteNonQuery(command);
        }
    }

    public int Power
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT power FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return (int)command.ExecuteScalar()!;
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET power = @power WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("power", NpgsqlDbType.Integer) { Value = value });

            ExecuteNonQuery(command);
        }
    }

    public int ExpeditionMinPower
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT expedition_min_power FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return (int)command.ExecuteScalar()!;
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET expedition_min_power = @expedition_min_power WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("expedition_min_power", NpgsqlDbType.Integer) { Value = value });

            ExecuteNonQuery(command);
        }
    }

    public TimeSpan ExpeditionMinTime
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT expedition_min_time FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return new TimeSpan(
                0,
                (int)command.ExecuteScalar()!,
                0
                );
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET expedition_min_time = @expedition_min_time WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("expedition_min_time", NpgsqlDbType.Integer) { Value = value.TotalMinutes });

            ExecuteNonQuery(command);
        }
    }

    public TimeSpan ExpeditionMaxTime
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT expedition_max_time FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return new TimeSpan(
                0,
                (int)command.ExecuteScalar()!,
                0
                );
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET expedition_max_time = @expedition_max_time WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("expedition_max_time", NpgsqlDbType.Integer) { Value = value.TotalMinutes });

            ExecuteNonQuery(command);
        }
    }

    public int ExpeditionTimeDecreaseBegin
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT expedition_tdb FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return (int)command.ExecuteScalar()!;
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET expedition_tdb = @expedition_time_decrease_begin WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("expedition_time_decrease_begin", NpgsqlDbType.Integer) { Value = value });

            ExecuteNonQuery(command);
        }
    }

    public int ExpeditionTimeDecreaseEnd
    {
        get
        {
            using NpgsqlConnection connection = GetConnectionAsync().Result;
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT expedition_tde FROM common.rarities WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });

            return (int)command.ExecuteScalar()!;
        }

        set
        {
            using NpgsqlConnection connection = GetConnection();
            using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE common.rarities SET expedition_tde = @expedition_time_decrease_end WHERE id = @id;";
            command.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Uuid) { Value = Id });
            command.Parameters.Add(new NpgsqlParameter("expedition_time_decrease_end", NpgsqlDbType.Integer) { Value = value });

            ExecuteNonQuery(command);
        }
    }
}