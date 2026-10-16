using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server0892, Direction.ServerToClient)]
public sealed partial class Server0892
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
