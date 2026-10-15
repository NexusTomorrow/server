using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicMissileActorRemove, Direction.ServerToClient)]
public sealed partial class ServerCinematicMissileActorRemove
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint MissileActorUnitId { get; set; }
}
