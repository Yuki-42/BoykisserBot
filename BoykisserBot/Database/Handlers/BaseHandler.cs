using System.Data.Common;
using Npgsql;

namespace BoykisserBot.Database.Handlers;

public class BaseHandler
{
    /// <summary>
    ///     DB connection string.
    /// </summary>
    protected readonly string ConnectionString;

    /// <summary>
    ///     Handlers group.
    /// </summary>
    protected HandlersGroup Handlers = null!;

    protected BaseHandler(string connectionString)
    {
        ConnectionString = connectionString;
    }

    /// <summary>
    ///     Configuration.
    /// </summary>
    protected Configuration.Config? Config { get; private set; }

    /// <summary>
    ///     Populate the handlers at runtime.
    /// </summary>
    /// <param name="handlers">Initialised handlers</param>
    /// <param name="config">Configuration</param>
    public void Populate(HandlersGroup handlers, Configuration.Config config)
    {
        Handlers = handlers;
        Config = config;
    }


    /// <summary>
    ///     Database connection.
    /// </summary>
    protected async Task<NpgsqlConnection> Connection()
    {
        NpgsqlConnection connection;
        int timeWaited = 0;
        while (true)
            try
            {
                connection = new NpgsqlConnection(ConnectionString);
                break;
            }
            catch (PostgresException exception)
            {
                if (exception.SqlState != "53300") throw;
                Console.WriteLine(
                    $"Connection limit hit. Waiting 500ms before trying again. Total time waited {timeWaited}");
                await Task.Delay(500);
                timeWaited += 500;
            }

        connection.Open();
        return connection;
    }

    /// <summary>
    ///     Execute a non-query command.
    /// </summary>
    /// <param name="command">Command.</param>
    protected async Task ExecuteNonQuery(DbCommand command)
    {
        await using NpgsqlConnection connection = await Connection();
        await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction;

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception error)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Failed to execute command: {error.Message}");
            throw;
        }

        await transaction.CommitAsync();
    }

    /// <summary>
    ///     Execute a reader command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <returns>Reader.</returns>
    protected static async Task<NpgsqlDataReader> ExecuteReader(NpgsqlCommand command)
    {
        return await command.ExecuteReaderAsync();
    }
}