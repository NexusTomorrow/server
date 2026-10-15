using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicScaleTransition, Direction.ServerToClient)]
public sealed partial class ServerCinematicTransitionDurationSet
{
    [UInt(32)] public ScaleTransitionType Type { get; set; }
    [UInt(16)] public ushort DurationStart { get; set; }
    [UInt(16)] public ushort DurationMid { get; set; }
    [UInt(16)] public ushort DurationEnd { get; set; }
}
