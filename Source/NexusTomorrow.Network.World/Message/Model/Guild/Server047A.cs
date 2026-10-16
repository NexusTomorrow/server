using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server047A, Direction.ServerToClient)]
public sealed partial class Server047A
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    [UInt(9)] public ushort Unknown1 { get; set; }
    [Array(CountBits = 32)] public List<Server0111Part0> Unknown2 { get; set; } = [];
}
