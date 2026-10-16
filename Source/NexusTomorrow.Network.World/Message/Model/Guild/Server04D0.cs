using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04D0, Direction.ServerToClient)]
public sealed partial class Server04D0
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    public Client0481Part0 Unknown1 { get; set; } = new();
}
