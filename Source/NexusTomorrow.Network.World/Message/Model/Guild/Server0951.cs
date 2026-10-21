using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0951, Direction.ServerToClient)]
public sealed partial class Server0951
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [Array(CountBits = 32)] public List<Server0951Part0> Unknown1 { get; set; } = [];
}
