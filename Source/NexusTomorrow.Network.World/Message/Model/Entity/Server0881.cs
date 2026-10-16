using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server0881, Direction.ServerToClient)]
public sealed partial class Server0881
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
