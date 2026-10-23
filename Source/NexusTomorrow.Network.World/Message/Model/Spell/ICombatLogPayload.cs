using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry's payload, one arm for each value of <see cref="CombatLog.Type"/>.
/// </summary>
public interface ICombatLogPayload : IWireStruct
{
}
