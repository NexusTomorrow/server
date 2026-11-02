using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

/// <summary>
/// Interrupts a spell.
/// </summary>
/// <remarks>
/// The client looks up the spell by id; when it is a crafting spell owned by the player it raises CraftingInterrupted.
/// </remarks>
[WireMessage(GameMessageOpcode.Server07F9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellInterrupted
{
    /// <summary>
    /// The spell, by spell id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint SpellId { get; set; }

    /// <summary>
    /// The spell's cast result, a Spell4CastResult table id.
    /// </summary>
    [VerifiedMember, UInt(9)] public ushort CastResult { get; set; }

    /// <summary>
    /// The spell's target, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TargetUnitId { get; set; }

    /// <summary>
    /// The interrupt's flags.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Flags { get; set; }
}