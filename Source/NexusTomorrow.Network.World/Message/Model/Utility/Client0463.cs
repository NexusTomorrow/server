using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Client0463, Direction.ClientToServer)]
public sealed partial class Client0463
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [UInt(14)] public ushort Unknown3 { get; set; }
    [UInt(64)] public ulong Unknown4 { get; set; }
}
