using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerMissileActorListAdd)]
[WireMessage(GameMessageOpcode.ServerMissileActorListAdd, Direction.ServerToClient)]
public sealed partial class ServerMissileActorListAdd : IWritable
{
    [Array(CountBits = 32)] public List<ServerMissileActorAdd> MissileActors { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MissileActors.Count);
        foreach (ServerMissileActorAdd actor in MissileActors)
        {
            actor.Write(writer);
        }
    }
}

