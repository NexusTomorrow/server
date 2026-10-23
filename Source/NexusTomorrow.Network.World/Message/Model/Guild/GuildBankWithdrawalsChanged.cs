using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04A1, Direction.ServerToClient)]
public sealed partial class GuildBankWithdrawalsChanged
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    public GuildBankWithdrawals Unknown1 { get; private set; } = new();
}
