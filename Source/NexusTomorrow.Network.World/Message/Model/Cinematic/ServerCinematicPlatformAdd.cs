using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicPlatformAdd, Direction.ServerToClient)]
public sealed partial class ServerCinematicPlatformAdd
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint ActorUnitId { get; set; }
    [UInt(32)] public uint PlatformUnitId { get; set; }
    [UInt(32)] public uint AttachmentId { get; set; }
}
