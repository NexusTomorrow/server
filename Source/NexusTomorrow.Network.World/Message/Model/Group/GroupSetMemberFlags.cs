using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client0408, Direction.ClientToServer)]
public sealed partial class GroupSetMemberFlags
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    public RealmScopedId Unknown1 { get; set; } = new();

    [UInt(32)] public uint Unknown2 { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
