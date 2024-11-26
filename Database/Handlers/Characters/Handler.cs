namespace BoykisserBot.Database.Handlers.Characters;

public class Handler(string connectionString) : BaseHandler(connectionString)
{
    /// <summary>
    ///     Instances handler.
    /// </summary>
    public InstancesHandler Instances { get; private set; } = new(connectionString);

    /// <summary>
    ///     Prototypes handler.
    /// </summary>
    public PrototypesHandler Prototypes { get; private set; } = new(connectionString);
}