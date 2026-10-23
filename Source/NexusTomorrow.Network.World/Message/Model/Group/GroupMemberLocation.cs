using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0467, Direction.ServerToClient)]
public sealed partial class GroupMemberLocation
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    public RealmScopedId Unknown1 { get; set; } = new();
    [UInt(14)] public uint Unknown2 { get; set; }
    [UInt(15)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
}
