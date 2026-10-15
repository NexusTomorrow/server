using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ClientCinematicCameraSubjectSpline)]
[WireMessage(GameMessageOpcode.ClientCinematicCameraSubjectSpline, Direction.ClientToServer)]
public sealed partial class ClientCinematicCameraSplineSubject : IReadable
{
    [UInt(32)] public uint SplineId { get; private set; }
    [UInt(32)] public uint SplineMode { get; private set; }
    [UInt(32)] public uint Speed { get; private set; }

    public void Read(GamePacketReader reader)
    {
        SplineId = reader.ReadUInt();
        SplineMode = reader.ReadUInt();
        Speed = reader.ReadUInt();
    }
}
