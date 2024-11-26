namespace BoykisserBot.Database.Handlers.Expeditions;

public class Handler(string connectionString) : BaseHandler(connectionString)
{
    /// <summary>
    ///     Expeditions Handler.
    /// </summary>
    public ExpeditionsHandler Expeditions {get; private set;} = new(connectionString);
}