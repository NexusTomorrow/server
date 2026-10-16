using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server0134, Direction.ServerToClient)]
public sealed partial class Server0134
{
    [UInt(15)] public uint Unknown0 { get; set; }
    public Server0112Part1 Unknown1 { get; set; } = new();
}
