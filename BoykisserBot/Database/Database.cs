using BoykisserBot.Configuration;
using BoykisserBot.Database.Handlers;

namespace BoykisserBot.Database;

public class Database
{
    public readonly HandlersGroup Handlers;

    public Database
    (
        Config config
    )
    {
        string connectionString =
            $"Host={config.Database.Host};" +
            $"Port={config.Database.Port};" +
            $"Username={config.Database.Username};" +
            $"Database={config.Database.Name};" +
            $"Password={config.Database.Password};" +
            $"Include Error Detail=true;";

        // Create handlers
        Handlers = new HandlersGroup(connectionString);

        // Now give all handlers access to each other
        foreach (BaseHandler handler in Handlers.Handlers) handler.Populate(Handlers, config);
    }
}