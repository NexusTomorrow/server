using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ClientCinematicCameraSubjectSpline, Direction.ClientToServer)]
public sealed partial class ClientCinematicCameraSplineSubject
{
    [UInt(32)] public uint SplineId { get; private set; }
    [UInt(32)] public uint SplineMode { get; private set; }
    [UInt(32)] public uint Speed { get; private set; }
}
