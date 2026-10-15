using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoldierHoldoutStatus, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierHoldoutStatus
{
    [UInt(14)] public uint PathSoldierEventId { get; set; }
    [Array(CountBits = 32)] public List<TowerDefenseUnit> Units { get; set; } = [];
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool IsBoss { get; set; }
    [UInt(32)] public PlayerPathSoldierEventMode Mode { get; set; }
    [UInt(32)] public int DelayTime { get; set; }
    [UInt(32)] public int WaveIndex { get; set; }
    [Float32] public float MaxDefendHealth { get; set; }
    [Float32] public float MaxAuxiliaryHealth { get; set; }
    [UInt(32)] public int StartTimeOffset { get; set; }
}
