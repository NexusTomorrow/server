using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0260, Direction.ServerToClient)]
public sealed partial class Server0260
{
    [Array(CountBits = 32)] public List<Server025F> Unknown0 { get; set; } = [];
}

