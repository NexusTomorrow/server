using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Client0552, Direction.ClientToServer)]
public sealed partial class Client0552
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
