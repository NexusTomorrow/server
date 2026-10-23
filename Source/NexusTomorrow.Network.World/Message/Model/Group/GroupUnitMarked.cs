using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server045B, Direction.ServerToClient)]
public sealed partial class GroupUnitMarked
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
}
