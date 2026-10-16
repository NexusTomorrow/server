using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client015E, Direction.ClientToServer)]
public sealed partial class Client015E
{
    [UInt(9)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
}
