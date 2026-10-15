using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerCooldownList, Direction.ServerToClient)]
public sealed partial class ServerCooldownList
{
    [Array(CountBits = 32)] public List<Cooldown> Cooldowns { get; set; } = [];
}
