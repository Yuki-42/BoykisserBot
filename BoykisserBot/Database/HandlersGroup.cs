using BoykisserBot.Database.Handlers;
using BoykisserBot.Database.Handlers.Characters;

namespace BoykisserBot.Database;

public class HandlersGroup(string connectionString)
{
    public readonly Handler Characters = new(connectionString);
    public readonly Handlers.Common.Handler Common = new(connectionString);
    public readonly Handlers.Config.Handler Config = new(connectionString);
    public readonly Handlers.Discord.Handler Discord = new(connectionString);
    public readonly Handlers.Expeditions.Handler Expeditions = new(connectionString);

    public IEnumerable<BaseHandler> Handlers =>
    [
        Characters,
        Common,
        Config,
        Discord,
        Expeditions
    ];
}