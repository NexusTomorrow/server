using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicActorSpline)]
[WireMessage(GameMessageOpcode.ServerCinematicActorSpline, Direction.ServerToClient)]
public sealed partial class ServerCinematicActorSpline : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint SplineId { get; set; }
    [Float32] public float SplineSpeed { get; set; }
    [UInt(32)] public uint SplineMode { get; set; }
    [UInt(1)] public bool UseRotation { get; set; }
    [UInt(1)] public bool Strafe { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(UnitId);
        writer.Write(SplineId);
        writer.Write(SplineSpeed);
        writer.Write(SplineMode);
        writer.Write(UseRotation);
        writer.Write(Strafe);
    }
}
