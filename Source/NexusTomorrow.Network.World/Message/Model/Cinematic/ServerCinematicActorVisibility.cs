using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicActorVisibility, Direction.ServerToClient)]
public sealed partial class ServerCinematicActorVisibility
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint UnitId { get; set; } // affects all units if UnitId is 0
    [UInt(1)] public bool Hide { get; set; }
    [UInt(1)] public bool AffectOnlyPlayers { get; set; }
}
