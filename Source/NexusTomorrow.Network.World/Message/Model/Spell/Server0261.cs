using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0261, Direction.ServerToClient)]
public sealed partial class Server0261
{
    [Array(CountBits = 32)] public List<Server0263> Unknown0 { get; set; } = [];
}
