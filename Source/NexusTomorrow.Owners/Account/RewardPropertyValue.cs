using NexusTomorrow.Game.Static.Reward;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// One composed reward property: the property and its sub-key, how its value counts, and the value, a float for the two scalar types
/// (<see cref="Scalar"/>) and a whole number for Discrete (<see cref="Discrete"/>).
/// </summary>
public readonly record struct RewardPropertyValue(RewardPropertyType Property, uint Data, RewardModifierValueType ValueType, float Scalar, uint Discrete);
