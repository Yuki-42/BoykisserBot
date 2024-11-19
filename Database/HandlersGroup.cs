using BoykisserBot.Database.Handlers;

namespace BoykisserBot.Database;

public class HandlersGroup(string connectionString)
{
    public readonly Handlers.Common.Handler Common = new(connectionString);
    public readonly Handlers.Config.Handler Config = new(connectionString);
    public readonly Handlers.Discord.Handler Discord = new(connectionString);

    public IEnumerable<BaseHandler> Handlers =>
    [
        Common,
        Config,
        Discord,
    ];
}