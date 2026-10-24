using System.Collections.Immutable;
using NexusTomorrow.Owners.Account;

namespace NexusTomorrow.Testing;

// The tier and the composed values a test names, our own fixture numbers.
public sealed class FixedRewardProperties : IRewardProperties
{
    public FixedRewardProperties(byte tier, params RewardPropertyValue[] values)
    {
        Tier = tier;
        Values = [.. values];
    }

    public byte Tier { get; }

    public ImmutableArray<RewardPropertyValue> Values { get; }
}
