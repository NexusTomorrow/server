using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03C6, Direction.ServerToClient)]
public sealed partial class Server03C6
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
}
