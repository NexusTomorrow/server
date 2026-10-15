using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicCameraSpline, Direction.ServerToClient)]
public sealed partial class ServerCinematicCameraSpline
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint SplineId { get; set; }
    [UInt(32)] public uint SplineMode { get; set; }
    [Float32] public float Speed { get; set; }
    [UInt(1)] public bool Target { get; set; }
    [UInt(1)] public bool UseRotation { get; set; }
}
