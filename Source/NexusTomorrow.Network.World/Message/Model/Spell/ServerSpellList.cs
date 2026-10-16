using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0551, Direction.ServerToClient)]
public sealed partial class Server0551
{
    [Array(CountBits = 32)] public List<Server0262Part7> Unknown0 { get; } = [];
}

