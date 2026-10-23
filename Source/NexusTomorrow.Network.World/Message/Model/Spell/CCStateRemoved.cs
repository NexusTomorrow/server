using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Removes a CC state from a unit.
/// </summary>
/// <remarks>
/// The client raises RemoveCCState.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0880, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CCStateRemoved
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The CC state.
    /// </summary>
    [UInt(5)] public CCState State { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// The effect that carried the state, by id.
    /// </summary>
    [UInt(32)] public uint EffectId { get; set; }

    [UInt(1)] public bool Unknown4 { get; set; }
}
