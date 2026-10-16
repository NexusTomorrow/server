using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Client0249, Direction.ClientToServer)]
public sealed partial class Client0249
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
