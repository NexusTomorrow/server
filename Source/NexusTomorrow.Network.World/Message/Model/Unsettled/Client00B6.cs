using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client00B6, Direction.ClientToServer)]
public sealed partial class Client00B6
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
