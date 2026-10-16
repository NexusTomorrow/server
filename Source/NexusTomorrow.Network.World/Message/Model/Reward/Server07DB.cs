using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server07DB, Direction.ServerToClient)]
public sealed partial class Server07DB
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
