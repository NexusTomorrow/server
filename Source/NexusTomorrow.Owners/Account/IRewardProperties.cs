using System.Collections.Immutable;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The premium tier every account holds and the reward properties that tier gives, which world entry sends
/// (<c>RewardPropertiesChanged</c>). The client never derives a player's reward properties itself: one it was not sent reads as its
/// type's default, 0 for Discrete and 1.0 for a scalar.
/// </summary>
public interface IRewardProperties
{
    /// <summary>
    /// The premium tier the values are composed for.
    /// </summary>
    byte Tier { get; }

    /// <summary>
    /// The composed values, one per property and sub-key; empty when there is nothing to send.
    /// </summary>
    ImmutableArray<RewardPropertyValue> Values { get; }
}
