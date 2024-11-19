using BoykisserBot.Database.Handlers;

namespace BoykisserBot.Database;

public class Database
{
    public readonly HandlersGroup Handlers;

    public Database
    (
        string host,
        int port,
        string username,
        string database,
        string password
    )
    {
        string connectionString =
            $"Host={host};Port={port};Username={username};Database={database};Password={password};Include Error Detail=true;";

        // Create handlers
        Handlers = new HandlersGroup(connectionString);

        // Now give all handlers access to each other
        foreach (BaseHandler handler in Handlers.Handlers) handler.Populate(Handlers);
    }
}