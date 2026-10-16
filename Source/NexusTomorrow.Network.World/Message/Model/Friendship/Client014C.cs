using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client014C, Direction.ClientToServer)]
public sealed partial class Client014C
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
}
