using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerMissileActorListAdd, Direction.ServerToClient)]
public sealed partial class ServerMissileActorListAdd
{
    [Array(CountBits = 32)] public List<ServerMissileActorAdd> MissileActors { get; set; } = [];
}

