using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0480, Direction.ServerToClient)]
public sealed partial class Server0480
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    [UInt(3)] public byte Unknown1 { get; set; }
}
