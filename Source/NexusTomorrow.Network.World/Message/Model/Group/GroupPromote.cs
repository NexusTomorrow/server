using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client0446, Direction.ClientToServer)]
public sealed partial class GroupPromote
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    public RealmScopedId Unknown1 { get; set; } = new();
}
