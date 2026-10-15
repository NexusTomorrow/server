using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerMissileActorRemove)]
public class ServerMissileActorRemove : IWritable
{
    public uint MissileActorId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MissileActorId);
    }
}
