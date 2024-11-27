using System.Diagnostics.CodeAnalysis;
using BoykisserBot.Commands.Logic;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.ApplicationCommands.EventArgs;
using DisCatSharp.Entities;
using DisCatSharp.Enums;

namespace BoykisserBot.Commands.SlashCommands;

// ReSharper disable once UnusedType.Global
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public abstract class DebugCommands : ApplicationCommandsModule
{
    [SlashCommandGroup("debug", "Debug commands.")]
    public class DebugCommandsGroup : ApplicationCommandsModule
    {
        /// <summary>
        ///     Audit related commands.
        /// </summary>
        [SlashCommandGroup("audit", "Audit commands.")]
        public class AuditGroup : ApplicationCommandsModule
        {
            public async Task SlashCommandErrored(SlashCommandErrorEventArgs e)
            {
                await e.Context.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder
                    {
                        Content =
                            "An error occurred while running this command. The developer has been pinged for review."
                    });

                ErrorHandler.Handle(e.Exception, e.Context);
            }

            /// <summary>
            ///     Audits all categories.
            /// </summary>
            /// <param name="ctx">Context</param>
            [SlashCommand("all", "Audits all categories.")]
            public static async Task AuditAllCommand(InteractionContext ctx)
            {
                // Perform permissions checks
                PermissionCode permission = await Shared.CheckPermissions(ctx);

                switch (permission)
                {
                    case PermissionCode.None:
                        await ctx.CreateResponseAsync(
                            InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder
                            {
                                Content = "You do not have permission to run this command."
                            });
                        return;
                    case PermissionCode.GlobalAdmin:
                        await Debug.AuditAllGlobalAdmin(ctx);
                        return;
                    case PermissionCode.ServerAdmin:
                        await Debug.AuditAllServerAdmin(ctx);
                        return;
                    default:
                        throw new InvalidDataException("Invalid permission code.");
                }
            }

            /// <summary>
            ///     Audits user data.
            /// </summary>
            /// <param name="ctx">Context</param>
            [SlashCommand("users", "Audits global user data.")]
            public static async Task AuditUsersCommand(InteractionContext ctx)
            {
                await ctx.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder
                    {
                        Content = "Running audit, this may take a while."
                    });

                // Do permissions checks
                PermissionCode permission = await Shared.CheckPermissions(ctx);

                switch (permission)
                {
                    case PermissionCode.None:
                        await ctx.EditResponseAsync(
                            new DiscordWebhookBuilder
                            {
                                Content = "You do not have permission to run this command."
                            });
                        return;
                    case PermissionCode.GlobalAdmin:
                        await Debug.AuditAllUsers(ctx);
                        break;
                    case PermissionCode.ServerAdmin:
                        await Debug.AuditGuildUsers(ctx, ctx.Guild);
                        break;
                    default:
                        throw new InvalidDataException("Invalid permission code.");
                }

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder
                    {
                        Content = "Audit completed."
                    });
            }

            /// <summary>
            ///     Audits guild data.
            /// </summary>
            /// <param name="ctx">Context</param>
            [SlashCommand("guilds", "Audits stored server data.")]
            public static async Task AuditGuildsCommand(InteractionContext ctx)
            {
                await ctx.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder
                    {
                        Content = "Running audit, this may take a while."
                    });

                // Do permissions checks
                PermissionCode permission = await Shared.CheckPermissions(ctx);

                switch (permission)
                {
                    case PermissionCode.None:
                        await ctx.EditResponseAsync(
                            new DiscordWebhookBuilder
                            {
                                Content = "You do not have permission to run this command."
                            });
                        return;
                    case PermissionCode.GlobalAdmin:
                        await Debug.AuditAllGuilds(ctx);
                        break;
                    case PermissionCode.ServerAdmin:
                        await Debug.AuditGuild(ctx, ctx.Guild);
                        break;
                    default:
                        throw new InvalidDataException("Invalid permission code.");
                }

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder
                    {
                        Content = "Audit completed."
                    });
            }
        }
    }
}