using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ServerPremiumRewards, Direction.ServerToClient)]
public sealed partial class ServerPremiumRewards
{
    /// <summary>
    /// A reward modifier's value: a float for the scalar <see cref="RewardModifierValueType"/> values, an integer for <see cref="RewardModifierValueType.Discrete"/>.
    /// </summary>
    public interface IModifierValue : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(RewardModifierValueType.AdditiveScalar)]
    public sealed partial class AdditiveScalarValue : IModifierValue
    {
        [Float32] public float Value { get; set; }
    }

    [WireStruct]
    [UnionCase(RewardModifierValueType.Discrete)]
    public sealed partial class DiscreteValue : IModifierValue
    {
        [UInt(32)] public uint Value { get; set; }
    }

    [WireStruct]
    [UnionCase(RewardModifierValueType.MultiplicativeScalar)]
    public sealed partial class MultiplicativeScalarValue : IModifierValue
    {
        [Float32] public float Value { get; set; }
    }

    [WireStruct]
    public sealed partial class RewardProperty
    {
        [WireStruct]
        public sealed partial class RewardOwnerMultiplier
        {
            [UInt(4)] public RewardModifierOwner OwnerType { get; set; }
            [UInt(32)] public uint OwnerId { get; set; }
            [UInt(2)] public RewardModifierValueType ModifierType { get; set; }
            [Union(nameof(ModifierType))] public IModifierValue Payload { get; set; } = new AdditiveScalarValue();
        }

        [UInt(6)] public RewardPropertyType RewardPropertyId { get; set; }
        [UInt(32)] public uint Data { get; set; }
        [UInt(2)] public RewardModifierValueType ModifierType { get; set; }
        [Union(nameof(ModifierType))] public IModifierValue Payload { get; set; } = new AdditiveScalarValue();
        [Array(CountBits = 8)] public List<RewardOwnerMultiplier> OwnerMultipliers { get; set; } = [];
    }

    [Array(CountBits = 8)] public List<RewardProperty> Properties { get; set; } = [];
}
