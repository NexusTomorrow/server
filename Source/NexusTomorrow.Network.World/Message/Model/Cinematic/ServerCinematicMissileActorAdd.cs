using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicMissileActorAdd, Direction.ServerToClient)]
public sealed partial class ServerCinematicMissileActorAdd
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint MissileActorUnitId { get; set; }
    [UInt(32)] public uint FromUnitId { get; set; }
    [UInt(32)] public uint ToUnitId { get; set; }
    [UInt(32)] public uint Spell4ClientMissileId { get; set; }
}
