using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Server0154, Direction.ServerToClient)]
public sealed partial class Server0154
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
