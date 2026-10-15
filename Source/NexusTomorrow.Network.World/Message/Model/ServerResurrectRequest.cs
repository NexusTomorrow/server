using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerResurrectRequest)]
[WireMessage(GameMessageOpcode.ServerResurrectRequest, Direction.ServerToClient)]
public sealed partial class ServerResurrectRequest : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(18)] public uint SpellId { get; set; }
    [UInt(32)] public uint TimeUntilRez { get; set; }
    [Float32] public float PercentageHealthRestored { get; set; }
    [Float32] public float PercentageEnergyRestored { get; set; }
    [Float32] public Vector3 Position { get; set; }
    [Pad(1)] public Pad Pad0 { get; }
    [NotOnWire] public bool SpellCastOnDead { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(SpellId, 18u);
        writer.Write(TimeUntilRez);
        writer.Write(PercentageHealthRestored);
        writer.Write(PercentageEnergyRestored);
        writer.Write(Position.X);
        writer.Write(Position.Y);
        writer.Write(Position.Z);
        writer.Write(false);
    }
}
