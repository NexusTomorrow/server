using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicMissileActorRemove)]
[WireMessage(GameMessageOpcode.ServerCinematicMissileActorRemove, Direction.ServerToClient)]
public sealed partial class ServerCinematicMissileActorRemove : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint MissileActorUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(MissileActorUnitId);
    }
}
