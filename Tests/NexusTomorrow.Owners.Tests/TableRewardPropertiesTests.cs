using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.GameTable;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Owners.Tests;

public sealed class TableRewardPropertiesTests : IDisposable
{
    // RewardPropertyPremiumModifier's columns in model order: id, system, tier, property, data, integer value, float value, tied
    // entitlement, flags.
    private static readonly DataType[] s_modifierColumns =
    [
        DataType.UInt, DataType.UInt, DataType.UInt, DataType.UInt, DataType.UInt, DataType.UInt, DataType.Single, DataType.UInt,
        DataType.UInt,
    ];

    // RewardProperty's columns: id, text id, value type.
    private static readonly DataType[] s_propertyColumns = [DataType.UInt, DataType.UInt, DataType.UInt];

    private readonly string _root = Path.Combine(Path.GetTempPath(), $"nt-rewards-{Guid.NewGuid():N}");

    public TableRewardPropertiesTests()
    {
        Directory.CreateDirectory(Path.Combine(_root, "tbl"));
    }

    public void Dispose() => Directory.Delete(_root, recursive: true);

    [Fact]
    public void NoTableSendsNoRewardProperties()
    {
        var logger = new RecordingLogger<TableRewardProperties>();

        var rewards = new TableRewardProperties(Options.Create(new ContentOptions()), FixedEntitlementCaps.Defaults(), new FixedEntitlementCaps(), logger);

        Assert.Empty(rewards.Values);
        Assert.Equal((byte)1, rewards.Tier);
        Assert.StartsWith("Warning", Assert.Single(logger.Entries), StringComparison.Ordinal);
    }

    [Fact]
    public void TheTablesAreReadAndComposedForTheAccountsTier()
    {
        // Our own fixture rows: property 60 Discrete (3 at tier 1, 4 at tier 0 flagged), property 61 AdditiveScalar (0.5 at tier 1).
        TableFile.Write(
            Path.Combine(_root, "tbl", "RewardPropertyPremiumModifier.tbl"), s_modifierColumns,
            [1, 1, 1, 60, 0, 3, 0, 0, 0],
            [2, 1, 0, 60, 0, 4, 0, 0, 1],
            [3, 1, 1, 61, 0, 0, BitConverter.SingleToUInt32Bits(0.5f), 0, 0]);
        TableFile.Write(
            Path.Combine(_root, "tbl", "RewardProperty.tbl"), s_propertyColumns,
            [60, 0, (uint)RewardModifierValueType.Discrete],
            [61, 0, (uint)RewardModifierValueType.AdditiveScalar]);

        var rewards = new TableRewardProperties(
            Options.Create(new ContentOptions { ClientDataPath = _root }), FixedEntitlementCaps.Defaults(), new FixedEntitlementCaps(),
            NullLogger<TableRewardProperties>.Instance);

        RewardPropertyValue[] expected =
        [
            new((RewardPropertyType)60, 0, RewardModifierValueType.Discrete, 0, 7),
            new((RewardPropertyType)61, 0, RewardModifierValueType.AdditiveScalar, 1.5f, 0),
        ];
        Assert.Equal(expected, rewards.Values);
    }
}
