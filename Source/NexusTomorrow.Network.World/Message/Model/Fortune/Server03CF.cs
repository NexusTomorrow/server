using NexusTomorrow.Game.Static.Fortune;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[WireMessage(GameMessageOpcode.Server03CF, Direction.ServerToClient)]
public sealed partial class Server03CF
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [FixedArray(3), UInt(1)] public bool[] Unknown2 { get; set; } = new bool[3];
}
