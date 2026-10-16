using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0493, Direction.ServerToClient)]
public sealed partial class Server0493
{
    public Server0493Part0 Unknown0 { get; set; } = new();
    public Server0493Part5 Unknown1 { get; set; } = new();
    public Server0493Part6 Unknown2 { get; set; } = new();
    [UInt(1)] public bool Unknown3 { get; set; }
}
