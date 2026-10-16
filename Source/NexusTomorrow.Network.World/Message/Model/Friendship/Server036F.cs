using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server036F, Direction.ServerToClient)]
public sealed partial class Server036F
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
