using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets whether the player may queue for the prime-level dungeon and adventure without a full group.
/// </summary>
[WireMessage(GameMessageOpcode.Server05B0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchingPartialGroupAllowedChanged
{
    /// <summary>
    /// Whether the player may queue for <see cref="MatchType.PrimeLevelDungeon"/> and <see cref="MatchType.PrimeLevelAdventure"/>
    /// without a full group. While it is false the client refuses a solo queue for them with
    /// <see cref="MatchingQueueResult.NotInGroup"/>, and a group short of the map's size with
    /// <see cref="MatchingQueueResult.RequiresFullGroup"/>.
    /// </summary>
    [UInt(1)] public bool PartialGroupAllowed { get; set; }
}
