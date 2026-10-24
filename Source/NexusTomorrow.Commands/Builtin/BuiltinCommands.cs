using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence;

namespace NexusTomorrow.Commands;

/// <summary>
/// Every command the server registers. The server's dispatcher and the generated commands page both read this list.
/// </summary>
public static class BuiltinCommands
{
    /// <summary>
    /// The account, character and server commands, in registration order, over the owners and persistence they post to.
    /// </summary>
    public static IReadOnlyList<CommandDefinition> Create(
        AccountOwner accounts, IRealmInbox realm, IEnumerable<IOwner> owners, IWriteBehind persistence, TimeProvider time) =>
        [.. AccountCommands.Create(accounts, time), .. CharacterCommands.Create(realm), .. ServerCommands.Create(accounts, owners, persistence)];
}
