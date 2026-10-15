using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerMissileActorAdd, Direction.ServerToClient)]
public sealed partial class ServerMissileActorAdd
{
    [UInt(16)] public ushort Spell4ClientMissileId { get; set; }
    [UInt(32)] public uint UnitId_1 { get; set; }
    [Float32] public Vector3 Position_Unit1 { get; set; }
    [UInt(32)] public uint UnitId_2 { get; set; }
    [Float32] public Vector3 Position_Unit2 { get; set; }
    [UInt(32)] public uint MissileActorId { get; set; }
    [UInt(32)] public uint TimeElapsed { get; set; }
}
