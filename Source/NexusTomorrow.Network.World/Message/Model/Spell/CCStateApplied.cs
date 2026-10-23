using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Applies a CC state to a unit.
/// </summary>
/// <remarks>
/// When the unit already carries the effect the client applies the state and raises ApplyCCState; otherwise it files
/// the effect under the state.
/// </remarks>
[WireMessage(GameMessageOpcode.Server087F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CCStateApplied
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The CC state.
    /// </summary>
    [UInt(5)] public CCState State { get; set; }

    /// <summary>
    /// The effect that carries the state, by id.
    /// </summary>
    [UInt(32)] public uint EffectId { get; set; }
}
