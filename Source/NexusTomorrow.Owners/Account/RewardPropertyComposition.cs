using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.GameTable.Model;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The client's composition of a premium tier's reward properties from <c>RewardPropertyPremiumModifier</c> rows. A tier takes the
/// rows of the Hybrid system at that tier and every lower tier's row whose flag bit 1 is set, grouped by property and sub-key. By the
/// property's value type (<c>RewardProperty</c>), AdditiveScalar is 1.0 plus the sum of the float values, Discrete the sum of the
/// whole values, and MultiplicativeScalar their product from 1.0, each floored at 0. A row tied to an entitlement adds, in place of
/// its own value, the account's count of that entitlement capped at its Entitlement row's maxCount; one on a property that is not
/// Discrete is logged and skipped, as the client logs it.
/// </summary>
public static class RewardPropertyComposition
{
    // The premium system the client runs under the game mode it starts in, with no message to change it (AccountItemLib.GetPremiumSystem).
    private const uint HybridSystem = 1;

    // A lower tier's row with this flag counts toward every tier above it.
    private const uint CarriesUp = 1;

    /// <summary>
    /// The values for <paramref name="tier"/>, in the order their property and sub-key first appear in <paramref name="modifiers"/>.
    /// <paramref name="heldCount"/> is the account's count of an entitlement, and <paramref name="maxCountOf"/> its row's maxCount
    /// (0 for no cap, null for no row).
    /// </summary>
    public static ImmutableArray<RewardPropertyValue> Compose(
        IEnumerable<RewardPropertyPremiumModifierEntry> modifiers, IEnumerable<RewardPropertyEntry> properties, uint tier,
        Func<uint, uint> heldCount, Func<uint, uint?> maxCountOf, ILogger logger)
    {
        var types = new Dictionary<uint, uint>();
        foreach (RewardPropertyEntry property in properties)
            types[property.Id] = property.RewardModifierValueTypeEnum;

        var groups = new Dictionary<(uint Property, uint Data), Group>();
        var order = new List<(uint Property, uint Data)>();
        foreach (RewardPropertyPremiumModifierEntry row in modifiers)
        {
            if (row.PremiumSystemEnum != HybridSystem || !(row.Tier == tier || (row.Tier < tier && (row.Flags & CarriesUp) != 0)))
                continue;
            if (!types.TryGetValue(row.RewardPropertyId, out uint type) || type > (uint)RewardModifierValueType.MultiplicativeScalar)
            {
                AccountLog.RewardRowSkipped(logger, row.Id, "its property has no known value type");
                continue;
            }

            (uint, uint) key = (row.RewardPropertyId, row.RewardPropertyData);
            if (!groups.TryGetValue(key, out Group? group))
            {
                group = new Group((RewardModifierValueType)type);
                groups[key] = group;
                order.Add(key);
            }

            group.Add(row, heldCount, maxCountOf, logger);
        }

        ImmutableArray<RewardPropertyValue>.Builder values = ImmutableArray.CreateBuilder<RewardPropertyValue>(order.Count);
        foreach ((uint property, uint data) in order)
            values.Add(groups[(property, data)].Value((RewardPropertyType)property, data));
        return values.MoveToImmutable();
    }

    // One property and sub-key's running total, kept in the form its value type composes.
    private sealed class Group
    {
        private readonly RewardModifierValueType _type;
        private float _sum;
        private float _product = 1f;
        private long _whole;

        public Group(RewardModifierValueType type)
        {
            _type = type;
        }

        public void Add(RewardPropertyPremiumModifierEntry row, Func<uint, uint> heldCount, Func<uint, uint?> maxCountOf, ILogger logger)
        {
            if (row.EntitlementIdModifierCount != 0)
            {
                if (_type != RewardModifierValueType.Discrete)
                {
                    AccountLog.RewardRowSkipped(logger, row.Id, "it is tied to an entitlement on a property that is not Discrete");
                    return;
                }

                uint held = heldCount(row.EntitlementIdModifierCount);
                _whole += maxCountOf(row.EntitlementIdModifierCount) is uint cap and > 0 ? Math.Min(held, cap) : held;
                return;
            }

            switch (_type)
            {
                case RewardModifierValueType.AdditiveScalar:
                    _sum += row.ModifierValueFloat;
                    break;
                case RewardModifierValueType.Discrete:
                    _whole += (int)row.ModifierValueInt;
                    break;
                case RewardModifierValueType.MultiplicativeScalar:
                    _product *= row.ModifierValueFloat;
                    break;
            }
        }

        public RewardPropertyValue Value(RewardPropertyType property, uint data) => _type switch
        {
            RewardModifierValueType.AdditiveScalar => new(property, data, _type, Math.Max(0f, 1f + _sum), 0),
            RewardModifierValueType.MultiplicativeScalar => new(property, data, _type, Math.Max(0f, _product), 0),
            _ => new(property, data, _type, 0, (uint)Math.Clamp(_whole, 0, uint.MaxValue)),
        };
    }
}
