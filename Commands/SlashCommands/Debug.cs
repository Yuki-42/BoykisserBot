using BoykisserBot.Commands.Logic;
using BoykisserBot.Database.Handlers.Discord;
using BoykisserBot.Database.Types.Users;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.ApplicationCommands.EventArgs;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace BoykisserBot.Commands.SlashCommands;


// ReSharper disable once UnusedType.Global
public class StatisticsCommands : ApplicationCommandsModule
{
    [SlashCommandGroup("debug", "Debug commands.")]
    public class StatisticsCommandGroup : ApplicationCommandsModule
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
                        Content = "An error occurred while running this command. The developer has been pinged for review."
                    });

                ErrorHandler.Handle(e.Exception, e.Context);
            }

            /// <summary>
            ///     Audits all categories.
            /// </summary>
            /// <param name="ctx">Context</param>
            [SlashCommand("all", "Audits all categories.")]
            public async Task AuditAllCommand(InteractionContext ctx)
            {
                // Check if the user is a global bot admin
                Handler handler = ctx.Services.GetRequiredService<Database.Database>().Handlers.Discord;
                UsersRow user = await handler.Users.Get(ctx.User);

                // Perform permissions checks
                int permission = await Shared.CheckPermissions(ctx);

                switch (permission)
                {
                    case 0:
                        await ctx.CreateResponseAsync(
                            InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder
                            {
                                Content = "You do not have permission to run this command."
                            });
                        return;
                    case 1:
                        await Debug.AuditAllGlobalAdmin(ctx);
                        return;
                    case 2:
                        await Debug.AuditAllServerAdmin(ctx);
                        return;
                }
            }

            /// <summary>
            ///     Audits user data.
            /// </summary>
            /// <param name="ctx">Context</param>
            [SlashCommand("users", "Audits global user data.")]
            public async Task AuditUsersCommand(InteractionContext ctx)
            {
                await ctx.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder
                    {
                        Content = "Running audit, this may take a while."
                    });

                // Do permissions checks
                int permission = await Shared.CheckPermissions(ctx);

                switch (permission)
                {
                    case 0:
                        await ctx.EditResponseAsync(
                            new DiscordWebhookBuilder
                            {
                                Content = "You do not have permission to run this command."
                            });
                        return;
                    case 1:
                        await Debug.AuditAllUsers(ctx);
                        break;
                    case 2:
                        await Debug.AuditGuildUsers(ctx, ctx.Guild);
                        break;
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
            public async Task AuditGuildsCommand(InteractionContext ctx)
            {
                await ctx.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder
                    {
                        Content = "Running audit, this may take a while."
                    });

                // Do permissions checks
                int permission = await Shared.CheckPermissions(ctx);

                switch (permission)
                {
                    case 0:
                        await ctx.EditResponseAsync(
                            new DiscordWebhookBuilder
                            {
                                Content = "You do not have permission to run this command."
                            });
                        return;
                    case 1:
                        await Debug.AuditAllGuilds(ctx);
                        break;
                    case 2:
                        await Debug.AuditGuild(ctx, ctx.Guild);
                        break;
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