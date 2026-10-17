using System.Text;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence;

namespace NexusTomorrow.Commands;

/// <summary>
/// The server commands.
/// </summary>
public static class ServerCommands
{
    /// <summary>
    /// The server commands. <c>server status</c> reads the accounts online from <paramref name="accounts"/>, each owner's queue from
    /// <paramref name="owners"/> and the backlog and reachability from <paramref name="persistence"/>, none of which block.
    /// </summary>
    public static IEnumerable<CommandDefinition> Create(AccountOwner accounts, IEnumerable<IOwner> owners, IWriteBehind persistence)
    {
        yield return new CommandDefinition
        {
            Name = "server status",
            Description = "Show the accounts online, each owner's queue, the persistence backlog and whether the database is reachable.",
            Permission = PermissionKeys.ServerStatus,
            Parameters = [],
            ExecuteAsync = (_, _) =>
            {
                var text = new StringBuilder();
                text.Append("accounts online: ").Append(accounts.OnlineCount).Append('\n');
                foreach (IOwner owner in owners)
                    text.Append(owner.Name).Append(" queue: ").Append(owner.QueueLength).Append('\n');
                text.Append("persistence backlog: ").Append(persistence.Backlog).Append('\n');
                text.Append("database: ").Append(persistence.IsAvailable ? "reachable" : "unreachable").Append('\n');
                return Task.FromResult(new CommandReply(CommandOutcome.Done, text.ToString()));
            },
        };
    }
}
