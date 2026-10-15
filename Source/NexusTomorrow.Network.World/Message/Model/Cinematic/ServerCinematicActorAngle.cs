using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicActorAngle)]
[WireMessage(GameMessageOpcode.ServerCinematicActorAngle, Direction.ServerToClient)]
public sealed partial class ServerCinematicActorAngle : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [Float32] public float Angle { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(UnitId);
        writer.Write(Angle);
    }
}
