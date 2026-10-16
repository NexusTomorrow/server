using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server039A, Direction.ServerToClient)]
public sealed partial class Server039A
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public Client00B5Part0 Unknown1 { get; set; } = new();
}
