using System.Collections.Immutable;
using Microsoft.Extensions.Logging.Abstractions;
using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.GameTable.Model;
using NexusTomorrow.Owners.Account;

namespace NexusTomorrow.Owners.Tests;

// Every row here is our own fixture: properties 60 and 61 and entitlement 999 are made-up numbers, never the client's rows.
public sealed class RewardPropertyCompositionTests
{
    private static readonly RewardPropertyEntry[] s_types =
    [
        new() { Id = 60, RewardModifierValueTypeEnum = (uint)RewardModifierValueType.Discrete },
        new() { Id = 61, RewardModifierValueTypeEnum = (uint)RewardModifierValueType.AdditiveScalar },
        new() { Id = 62, RewardModifierValueTypeEnum = (uint)RewardModifierValueType.MultiplicativeScalar },
    ];

    [Fact]
    public void TheTierTakesItsRowsAndTheLowerTiersFlaggedRows()
    {
        ImmutableArray<RewardPropertyValue> values = Compose(tier: 1, Row(1, 60, integer: 3), Row(0, 60, integer: 2, flags: 1), Row(0, 60, integer: 5));

        Assert.Equal(new RewardPropertyValue((RewardPropertyType)60, 0, RewardModifierValueType.Discrete, 0, 5), Assert.Single(values));
    }

    [Fact]
    public void AFreeTierValueComesOnlyFromATestThatSetsIt()
    {
        ImmutableArray<RewardPropertyValue> values = Compose(tier: 0, Row(0, 60, integer: 2), Row(1, 60, integer: 3));

        Assert.Equal(2u, Assert.Single(values).Discrete);
    }

    [Fact]
    public void TypeZeroIsOnePlusTheSum()
    {
        ImmutableArray<RewardPropertyValue> values = Compose(tier: 1, Row(1, 61, scalar: 0.25f), Row(1, 61, scalar: 0.5f));

        Assert.Equal(1.75f, Assert.Single(values).Scalar);
    }

    [Fact]
    public void TypeTwoIsTheProduct()
    {
        ImmutableArray<RewardPropertyValue> values = Compose(tier: 1, Row(1, 62, scalar: 0.5f), Row(1, 62, scalar: 0.5f));

        RewardPropertyValue value = Assert.Single(values);
        Assert.Equal(RewardModifierValueType.MultiplicativeScalar, value.ValueType);
        Assert.Equal(0.25f, value.Scalar);
    }

    [Fact]
    public void AValueIsFlooredAtZero()
    {
        ImmutableArray<RewardPropertyValue> values = Compose(tier: 1, Row(1, 61, scalar: -1.5f));

        Assert.Equal(0f, Assert.Single(values).Scalar);
    }

    [Fact]
    public void APropertyIsKeyedByItsData()
    {
        ImmutableArray<RewardPropertyValue> values = Compose(tier: 1, Row(1, 60, data: 4, integer: 2), Row(1, 60, data: 5, integer: 3));

        (uint Data, uint Value)[] expected = [(4, 2), (5, 3)];
        Assert.Equal(expected, values.Select(value => (value.Data, value.Discrete)));
    }

    [Fact]
    public void ATiedRowAddsTheAccountsCountCappedAtMaxCount()
    {
        ImmutableArray<RewardPropertyValue> values = Compose(
            tier: 1, held: 6, Row(1, 60, integer: 2), Row(1, 60, integer: 7, tied: 999));

        Assert.Equal(6u, Assert.Single(values).Discrete);
    }

    [Fact]
    public void ATiedRowTheAccountLacksAddsNothing()
    {
        ImmutableArray<RewardPropertyValue> values = Compose(
            tier: 1, held: 0, Row(1, 60, integer: 2), Row(1, 60, integer: 7, tied: 999));

        Assert.Equal(2u, Assert.Single(values).Discrete);
    }

    [Fact]
    public void ATiedRowOnAFloatPropertyIsSkipped()
    {
        var logger = new RecordingLogger<TableRewardProperties>();

        ImmutableArray<RewardPropertyValue> values = RewardPropertyComposition.Compose(
            [Row(1, 61, scalar: 0.5f), Row(1, 61, scalar: 0.25f, tied: 999)], s_types, 1, _ => 6, _ => 4, logger);

        Assert.Equal(1.5f, Assert.Single(values).Scalar);
        Assert.StartsWith("Warning", Assert.Single(logger.Entries), StringComparison.Ordinal);
    }

    [Fact]
    public void AnotherSystemsRowsAreIgnored()
    {
        Assert.Empty(Compose(tier: 1, Row(1, 60, integer: 3, system: 2)));
    }

    private static ImmutableArray<RewardPropertyValue> Compose(uint tier, params RewardPropertyPremiumModifierEntry[] rows) =>
        Compose(tier, held: 0, rows);

    // The account holds entitlement 999 at held, which caps at 4.
    private static ImmutableArray<RewardPropertyValue> Compose(uint tier, uint held, params RewardPropertyPremiumModifierEntry[] rows) =>
        RewardPropertyComposition.Compose(
            rows, s_types, tier, id => id == 999 ? held : 0, id => id == 999 ? 4 : null, NullLogger<TableRewardProperties>.Instance);

    private static RewardPropertyPremiumModifierEntry Row(
        uint tier, uint property, uint data = 0, uint integer = 0, float scalar = 0, uint tied = 0, uint flags = 0, uint system = 1) =>
        new()
        {
            PremiumSystemEnum = system,
            Tier = tier,
            RewardPropertyId = property,
            RewardPropertyData = data,
            ModifierValueInt = integer,
            ModifierValueFloat = scalar,
            EntitlementIdModifierCount = tied,
            Flags = flags,
        };
}
