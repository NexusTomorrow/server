using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server091B, Direction.ServerToClient)]
public sealed partial class Server091B
{
    [Array(CountBits = 32)] public List<Server0804Part0> Unknown0 { get; set; } = [];
}
