using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellCooldownStart, Direction.ServerToClient)]
public sealed partial class ServerSpellCooldownStart
{
    public Cooldown Cooldown { get; set; }
}
