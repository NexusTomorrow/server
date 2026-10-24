using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Commands.Tests;

public sealed class BuiltinCommandsTests
{
    [Fact]
    public void TheBuiltinCommandsAreTheAccountAndServerCommandsInRegistrationOrder()
    {
        var persistence = new FakeWriteBehind();
        var accounts = new AccountOwner(TimeProvider.System, Options.Create(new AccountOptions()), NullLogger<AccountOwner>.Instance, persistence, FixedEntitlementCaps.Defaults());

        IReadOnlyList<CommandDefinition> commands = BuiltinCommands.Create(accounts, [accounts], persistence, TimeProvider.System);

        Assert.Equal(["account create", "account suspend", "account unsuspend", "server status"], commands.Select(command => command.Name));
    }
}
