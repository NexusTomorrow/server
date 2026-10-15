using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerMissileActorListAdd)]
public class ServerMissileActorListAdd : IWritable
{
    public List<ServerMissileActorAdd> MissileActors { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MissileActors.Count);
        foreach (ServerMissileActorAdd actor in MissileActors)
        {
            actor.Write(writer);
        }
    }
}

