using System.Data;

namespace BoykisserBot.Database.Types;

/// <summary>
///     Base class for all database types.
/// </summary>
public class BaseRow(
    string connectionString,
    Configuration.Config config,
    HandlersGroup handlersGroup,
    IDataRecord record)
    : TypeBase(connectionString, handlersGroup)
{
    /// <summary>
    ///     Configuration.
    /// </summary>
    protected Configuration.Config? Config = config;

    /// <summary>
    ///     Row id.
    /// </summary>
    protected Guid Id { get; } = TryGetGuid(record, "id") ?? Guid.Empty;

    /// <summary>
    ///     Row added to db.
    /// </summary>
    public DateTime CreatedAt { get; } = record.GetDateTime(record.GetOrdinal("created_at"));


    private static Guid? TryGetGuid(IDataRecord record, string column)
    {
        try
        {
            return record.GetGuid(record.GetOrdinal("id"));
        }
        catch (InvalidCastException)
        {
            return null;
        }
    }
}