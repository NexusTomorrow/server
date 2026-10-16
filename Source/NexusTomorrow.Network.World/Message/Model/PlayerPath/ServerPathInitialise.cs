using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06BC, Direction.ServerToClient)]
public sealed partial class Server06BC
{
    [UInt(3)] public byte Unknown0 { get; set; }
    [FixedArray(4), UInt(32)] public uint[] Unknown1 { get; set; } = new uint[4];
    [UInt(4)] public byte Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
}
