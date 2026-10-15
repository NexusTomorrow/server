using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityBoneUpdate)]
public class ServerEntityBoneUpdate : IWritable
{
    public uint UnitId { get; set; }
    public List<float> Bones { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Bones.Count);
        Bones.ForEach(u => writer.Write(u));
    }
}
