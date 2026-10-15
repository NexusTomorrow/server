using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoldierHoldOutNextWave, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierHoldOutNextWave
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(32)] public uint WaveIndex { get; set; }
    [UInt(1)] public bool IsBoss { get; set; }
}
