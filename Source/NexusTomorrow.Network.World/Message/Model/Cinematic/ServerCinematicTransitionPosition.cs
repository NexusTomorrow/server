using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicTransitionPosition, Direction.ServerToClient)]
public sealed partial class ServerCinematicTransitionPosition
{
    [UInt(32)] public uint Delay { get; set; }
    public Position Position { get; set; }
}
