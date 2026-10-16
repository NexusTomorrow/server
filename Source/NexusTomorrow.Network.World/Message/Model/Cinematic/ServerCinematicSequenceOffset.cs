using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server0226, Direction.ServerToClient)]
public sealed partial class Server0226
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
