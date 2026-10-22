using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0462, Direction.ServerToClient)]
public sealed partial class GroupMaxSizeChanged
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
}
