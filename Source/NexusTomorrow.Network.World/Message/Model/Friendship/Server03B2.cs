using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03B2, Direction.ServerToClient)]
public sealed partial class Server03B2
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public Client00B5Part0 Unknown1 { get; set; } = new();
    [UInt(8)] public uint Unknown2 { get; set; }
}
