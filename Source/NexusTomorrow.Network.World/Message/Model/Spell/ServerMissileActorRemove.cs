using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerMissileActorRemove, Direction.ServerToClient)]
public sealed partial class ServerMissileActorRemove
{
    [UInt(32)] public uint MissileActorId { get; set; }
}
