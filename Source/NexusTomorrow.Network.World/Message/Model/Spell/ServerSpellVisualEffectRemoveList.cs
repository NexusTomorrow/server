using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server077E, Direction.ServerToClient)]
public sealed partial class Server077E
{
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown0 { get; set; } = [];
}
