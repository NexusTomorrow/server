using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client0477, Direction.ClientToServer)]
public sealed partial class GuildOpenBankTab
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [UInt(9)] public ushort Unknown1 { get; private set; }
}
