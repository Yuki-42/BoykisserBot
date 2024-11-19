namespace BoykisserBot.Database.Handlers.Common;

public class Handler(string connectionString) : BaseHandler(connectionString)
{
    /// <summary>
    ///     Rarities handler.
    /// </summary>
    public RaritiesHandler Rarities { get; private set; } = new(connectionString);
}