using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// A matching queue to join: its match type, the maps and game type chosen, and the queue flags.
/// </summary>
[WireStruct]
public sealed partial class MatchingQueueRequest
{
    /// <summary>
    /// The queue's match type.
    /// </summary>
    [UInt(5)] public MatchType MatchType { get; set; }

    /// <summary>
    /// The maps queued for, as MatchingGameMap ids.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> GameMapIds { get; set; } = [];

    /// <summary>
    /// The game type, as a MatchingGameType id.
    /// </summary>
    [UInt(14)] public ushort GameTypeId { get; set; }

    /// <summary>
    /// The queue flags. Only <see cref="MatchingQueueFlags.GroupIsQueued"/> (4) has a known meaning here, and the client also sets
    /// 0x40, which <see cref="MatchingQueueFlags"/> lacks.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
