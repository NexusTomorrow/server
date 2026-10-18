using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Commands;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Persistence;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// The server's command list as the commands page reads it.
/// </summary>
public static class Registries
{
    /// <summary>
    /// Every command the server registers (<see cref="BuiltinCommands"/>), built over an account owner that never starts: the page reads
    /// the definitions and runs none of them.
    /// </summary>
    public static IReadOnlyList<CommandDefinition> Commands()
    {
        var persistence = new NoPersistence();
        var accounts = new AccountOwner(TimeProvider.System, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, persistence);
        return BuiltinCommands.Create(accounts, [accounts], persistence, TimeProvider.System);
    }

    // Nothing runs a command here, so nothing is ever queued.
    private sealed class NoPersistence : IWriteBehind
    {
        public int Backlog => 0;

        public bool IsAvailable => false;

        public void Enqueue(PersistenceWork work) => throw new InvalidOperationException("The reference pages run no command.");
    }
}
