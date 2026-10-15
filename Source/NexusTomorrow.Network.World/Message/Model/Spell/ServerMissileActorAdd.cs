using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerMissileActorAdd)]
[WireMessage(GameMessageOpcode.ServerMissileActorAdd, Direction.ServerToClient)]
public sealed partial class ServerMissileActorAdd : IWritable
{
    [UInt(16)] public ushort Spell4ClientMissileId { get; set; }
    [UInt(32)] public uint UnitId_1 { get; set; }
    [Float32] public Vector3 Position_Unit1 { get; set; }
    [UInt(32)] public uint UnitId_2 { get; set; }
    [Float32] public Vector3 Position_Unit2 { get; set; }
    [UInt(32)] public uint MissileActorId { get; set; }
    [UInt(32)] public uint TimeElapsed { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4ClientMissileId);
        writer.Write(UnitId_1);
        writer.WriteVector3(Position_Unit1);
        writer.Write(UnitId_2);
        writer.WriteVector3(Position_Unit2);
        writer.Write(MissileActorId);
        writer.Write(TimeElapsed);
    }
}
