using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerResurrectRequest, Direction.ServerToClient)]
public sealed partial class ServerResurrectRequest
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(18)] public uint SpellId { get; set; }
    [UInt(32)] public uint TimeUntilRez { get; set; }
    [Float32] public float PercentageHealthRestored { get; set; }
    [Float32] public float PercentageEnergyRestored { get; set; }
    [Float32] public Vector3 Position { get; set; }
    [Pad(1)] public Pad Pad0 { get; }
    [NotOnWire] public bool SpellCastOnDead { get; set; }
}
