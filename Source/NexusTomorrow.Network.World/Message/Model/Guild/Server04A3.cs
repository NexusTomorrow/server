using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04A3, Direction.ServerToClient)]
public sealed partial class Server04A3
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    public Server0493Part5 Unknown1 { get; set; } = new();
    [UInt(16)] public ushort Unknown2 { get; set; }
    [UInt(16)] public ushort Unknown3 { get; set; }
}
