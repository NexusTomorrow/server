using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[WireMessage(GameMessageOpcode.Server03D1, Direction.ServerToClient)]
public sealed partial class Server03D1
{
    [UInt(3)] public byte Unknown0 { get; set; }
    [FixedArray(3), UInt(2)] public byte[] Unknown1 { get; set; } = new byte[3];
    [FixedArray(3), UInt(32)] public uint[] Unknown2 { get; set; } = new uint[3];
    [FixedArray(3), UInt(1)] public bool[] Unknown3 { get; set; } = new bool[3];
}
