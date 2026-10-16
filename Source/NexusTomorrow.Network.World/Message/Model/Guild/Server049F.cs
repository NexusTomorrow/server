using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server049F, Direction.ServerToClient)]
public sealed partial class Server049F
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    public Client00B5Part0 Unknown1 { get; set; } = new();
    [UInt(32)] public uint Unknown2 { get; set; }
}
