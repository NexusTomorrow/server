using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server025F, Direction.ServerToClient)]
public sealed partial class Server025F
{
    [UInt(16)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [Float32] public Vector3 Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [Float32] public Vector3 Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
}
