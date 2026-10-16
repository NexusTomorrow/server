using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client01B0, Direction.ClientToServer)]
public sealed partial class Client01B0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(3)] public byte Unknown2 { get; set; }
}
