using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Client0460, Direction.ClientToServer)]
public sealed partial class Client0460
{
    [UInt(15)] public ushort Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
}
