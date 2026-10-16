using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server05E3, Direction.ServerToClient)]
public sealed partial class Server05E3
{
    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(5)] public byte Unknown2 { get; set; }
    [FixedArray(2), UInt(8)] public byte[] Unknown3 { get; set; } = new byte[2];
}
