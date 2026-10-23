using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Entity;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// The player's path state: the active path, each path's XP, which paths are unlocked, and the time the path-change cooldown is measured
/// against.
/// </summary>
/// <remarks>
/// Dropped until <see cref="PlayerUnit"/> has named the player. It replaces the client's path state; until the first one arrives the client
/// drops its other path messages.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06BC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PathState
{
    /// <summary>
    /// The player's active path: Soldier 0, Settler 1, Scientist 2, Explorer 3. The client's path lookups use it when they name no
    /// path.
    /// </summary>
    [UInt(3)] public byte ActivePath { get; set; }

    /// <summary>
    /// The XP of each path, indexed by the path's number (<see cref="ActivePath"/>).
    /// </summary>
    [FixedArray(4), UInt(32)] public uint[] PathXp { get; set; } = new uint[4];

    /// <summary>
    /// Which paths are unlocked: bit n for path number n (<see cref="ActivePath"/>).
    /// </summary>
    [UInt(4)] public byte UnlockedPaths { get; set; }

    /// <summary>
    /// A number of days the client adds to its clock; it measures its path-change cooldown against the result.
    /// </summary>
    [Float32] public float CooldownTime { get; set; }
}
