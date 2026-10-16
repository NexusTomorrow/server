using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04C5, Direction.ServerToClient)]
public sealed partial class Server04C5
{
    public Client00B5Part0 Unknown0 { get; set; }
    [FixedArray(10)] public Server0493Part1[] Unknown1 { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => new Server0493Part1())];
}
