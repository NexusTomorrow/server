using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicCameraTransition, Direction.ServerToClient)]
public sealed partial class ServerCinematicCameraTransition
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint Type { get; set; }
    [UInt(16)] public ushort DurationStart { get; set; }
    [UInt(16)] public ushort DurationMid { get; set; }
    [UInt(16)] public ushort DurationEnd { get; set; }
}
