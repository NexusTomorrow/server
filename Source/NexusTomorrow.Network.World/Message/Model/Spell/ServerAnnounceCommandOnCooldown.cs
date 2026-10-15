using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerAnnounceCommandOnCooldown, Direction.ServerToClient)]
public sealed partial class ServerAnnounceCommandOnCooldown
{
    [UInt(32)] public uint CooldownTime { get; set; }
}
