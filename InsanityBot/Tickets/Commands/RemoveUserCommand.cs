﻿using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsanityBot.Tickets.Commands
{
    public class RemoveUserCommand : BaseCommandModule
    {
        [Command("remove")]
        public async Task RemoveUser(CommandContext ctx,
            params DiscordMember[] users)
        {
            IEnumerable<KeyValuePair<Guid, DiscordTicket>> x = from t in InsanityBot.TicketDaemon.Tickets
                                                               where t.Value.DiscordChannelId == ctx.Channel.Id
                                                               select t;

            KeyValuePair<Guid, DiscordTicket> y = x.First();

            if(!x.Any())
            {
                DiscordEmbedBuilder error = new()
                {
                    Description = InsanityBot.LanguageConfig["insanitybot.tickets.remove_user.not_a_ticket_channel"].ReplaceValues(
                        ctx, ctx.Channel),
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(error.Build());
                return;
            }

            foreach(DiscordMember v in users)
            {
                _ = ctx.Channel.AddOverwriteAsync(v, deny: Permissions.AccessChannels);

                DiscordTicket z = InsanityBot.TicketDaemon.Tickets[y.Key];
                z.AddedUsers = (from a in z.AddedUsers
                                where a != v.Id
                                select a).ToArray();
                InsanityBot.TicketDaemon.Tickets[y.Key] = z;
            }
        }

        public async Task AddRole(CommandContext ctx,
            params DiscordRole[] roles)
        {
            IEnumerable<KeyValuePair<Guid, DiscordTicket>> x = from t in InsanityBot.TicketDaemon.Tickets
                                                               where t.Value.DiscordChannelId == ctx.Channel.Id
                                                               select t;

            if(!x.Any())
            {
                DiscordEmbedBuilder error = new()
                {
                    Description = InsanityBot.LanguageConfig["insanitybot.tickets.remove_user.not_a_ticket_channel"].ReplaceValues(
                        ctx, ctx.Channel),
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(error.Build());
                return;
            }

            foreach(DiscordRole v in roles)
            {
                _ = ctx.Channel.AddOverwriteAsync(v, deny: Permissions.AccessChannels);
            }
        }
    }
}