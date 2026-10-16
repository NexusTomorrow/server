using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client0142, Direction.ClientToServer)]
public sealed partial class Client0142
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
}
