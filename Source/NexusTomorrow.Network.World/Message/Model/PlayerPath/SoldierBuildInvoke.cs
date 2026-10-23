using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server0676, Direction.ServerToClient)]
public sealed partial class SoldierBuildInvoke
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown2 { get; set; } = [];
}
