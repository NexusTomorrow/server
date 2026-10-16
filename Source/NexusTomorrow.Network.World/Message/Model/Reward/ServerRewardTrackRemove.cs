using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server07D0, Direction.ServerToClient)]
public sealed partial class Server07D0
{
    [UInt(14)] public uint Unknown0 { get; set; }
}
