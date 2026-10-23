using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0260, Direction.ServerToClient)]
public sealed partial class MissilesAdded
{
    [Array(CountBits = 32)] public List<MissileAdded> Unknown0 { get; set; } = [];
}

