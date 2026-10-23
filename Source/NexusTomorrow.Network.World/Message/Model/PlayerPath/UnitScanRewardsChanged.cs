using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Sets whether a unit can be scanned and which of its scan rewards the player already has.
/// </summary>
[WireMessage(GameMessageOpcode.Server08B3, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitScanRewardsChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's scan rewards the player already has, one bit per kind: bit 0 the scan mission of type 2 or 14, bit 1 the creature's own
    /// scan, bit 2 the progress reward, bit 3 the type-23 mission and bit 4 a sixth reward kind. A set bit withholds that kind from the
    /// unit's rewards, which Unit:GetRewardInfo reports.
    /// </summary>
    [UInt(32)] public uint CompletedScanRewards { get; set; }

    /// <summary>
    /// Whether the unit can be scanned at all, which every scan reward requires.
    /// </summary>
    [UInt(1)] public bool CanBeScanned { get; set; }
}
