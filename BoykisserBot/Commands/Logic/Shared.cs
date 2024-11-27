using BoykisserBot.Database.Handlers.Discord;
using BoykisserBot.Database.Types.Users;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace BoykisserBot.Commands.Logic;

public enum PermissionCode
{
    None = 0,
    GlobalAdmin = 1,
    ServerAdmin = 2
}

public abstract class Shared
{
    /// <summary>
    ///     Creates a simple permissions integer.
    /// </summary>
    /// <param name="ctx">Context</param>
    /// <returns>0: No administrator, 1: Global bot admin, 2: Server admin</returns>
    public static async Task<PermissionCode> CheckPermissions(BaseContext ctx)
    {
        // Check if the user is a global bot admin
        Handler handler = ctx.Services.GetRequiredService<Database.Database>().Handlers.Discord;
        UsersRow user = await handler.Users.Get(ctx.User);

        // Check if the user is a global bot admin
        if (user.Admin) return PermissionCode.GlobalAdmin;

        // Check if the user is a server admin
        if (ctx.GuildId != null && ctx.Member!.Permissions.HasPermission(Permissions.Administrator))
            return PermissionCode.ServerAdmin;

        // If the user is not a global bot admin or a server admin, return an error
        return PermissionCode.None;
    }
}