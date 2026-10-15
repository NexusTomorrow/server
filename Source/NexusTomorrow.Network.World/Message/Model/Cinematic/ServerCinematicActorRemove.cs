using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicActorRemove, Direction.ServerToClient)]
public sealed partial class ServerCinematicActorRemove
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
}
