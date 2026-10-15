using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityBoneUpdate)]
[WireMessage(GameMessageOpcode.ServerEntityBoneUpdate, Direction.ServerToClient)]
public sealed partial class ServerEntityBoneUpdate : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [Array(CountBits = 32), Float32] public List<float> Bones { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Bones.Count);
        Bones.ForEach(u => writer.Write(u));
    }
}
