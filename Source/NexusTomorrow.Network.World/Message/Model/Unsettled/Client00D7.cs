using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client00D7, Direction.ClientToServer)]
public sealed partial class Client00D7
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
}
