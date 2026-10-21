using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0489, Direction.ServerToClient)]
public sealed partial class Server0489
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(1)] public bool Unknown1 { get; set; }
    [Array(CountBits = 32)] public List<Server0487Part0> Unknown2 { get; set; } = [];
}
