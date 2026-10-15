using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicPlayerControl, Direction.ServerToClient)]
public sealed partial class ServerCinematicPlayerControl
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(1)] public bool Allow { get; set; }
}
