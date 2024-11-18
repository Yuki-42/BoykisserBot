using BoykisserBot.Database.Handlers;
using Config = BoykisserBot.Database.Handlers.Config;
using Discord = BoykisserBot.Database.Handlers.Discord;

namespace BoykisserBot.Database;

public class HandlersGroup
{
    public required Config.Handler Config;
    public required Discord.Handler Discord;

    public IEnumerable<BaseHandler> Handlers =>
    [
        Config,
        Discord
    ];
}