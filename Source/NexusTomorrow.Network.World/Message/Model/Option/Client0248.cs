using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Client0248, Direction.ClientToServer)]
public sealed partial class Client0248
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(31)] public uint Unknown1 { get; set; }
}
