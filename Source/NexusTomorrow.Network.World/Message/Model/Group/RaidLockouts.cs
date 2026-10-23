using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Lists the player's raid lockouts.
/// </summary>
/// <remarks>
/// The client raises RaidInfoResponse with one entry per lockout.
/// </remarks>
[WireMessage(GameMessageOpcode.Server071A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RaidLockouts
{
    /// <summary>
    /// One raid lockout.
    /// </summary>
    [WireStruct]
    public sealed partial class RaidLockout
    {
        /// <summary>
        /// The saved instance, by id.
        /// </summary>
        [UInt(64)] public ulong SavedInstanceId { get; set; }

        /// <summary>
        /// The instance's world, a World table id.
        /// </summary>
        [UInt(15)] public ushort WorldId { get; set; }

        /// <summary>
        /// When the lockout ends, as a Windows file time.
        /// </summary>
        [UInt(64)] public ulong ExpiryTime { get; set; }

        /// <summary>
        /// How many days remain until the lockout ends.
        /// </summary>
        [Float32] public float DaysUntilReset { get; set; }

        /// <summary>
        /// The instance's prime level.
        /// </summary>
        [UInt(32)] public uint PrimeLevel { get; set; }
    }

    /// <summary>
    /// The lockouts.
    /// </summary>
    [Array(CountBits = 32)] public List<RaidLockout> Lockouts { get; set; } = [];
}
