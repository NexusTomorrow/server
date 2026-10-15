using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Used by <see cref="CurrencyManager"/>, and also used during Combat to provide Momentum Boosts.
/// </summary>
[WireMessage(GameMessageOpcode.ServerCombatReward, Direction.ServerToClient)]
public sealed partial class ServerCombatReward
{
    [UInt(5)] public byte Stat { get; set; }
    [UInt(64)] public ulong NewValue { get; set; }
    [UInt(32)] public uint CombatRewardId { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }
}
