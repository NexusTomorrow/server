using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerMissileActorRemove)]
[WireMessage(GameMessageOpcode.ServerMissileActorRemove, Direction.ServerToClient)]
public sealed partial class ServerMissileActorRemove : IWritable
{
    [UInt(32)] public uint MissileActorId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MissileActorId);
    }
}
