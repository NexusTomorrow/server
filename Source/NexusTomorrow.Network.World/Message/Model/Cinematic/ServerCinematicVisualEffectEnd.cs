using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicVisualEffectEnd, Direction.ServerToClient)]
public sealed partial class ServerCinematicVisualEffectEnd
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint VisualEffectUniqueId { get; set; }
}
