using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Adds or removes a quest's spell shortcut.
/// </summary>
/// <remarks>
/// The client keeps the shortcut in a tree keyed by the spell, slot, and quest; the add flag selects the direction.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0176, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class QuestSpellShortcut
{
    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint SpellId { get; set; }

    /// <summary>
    /// The shortcut's slot.
    /// </summary>
    [VerifiedMember, UInt(3)] public uint Slot { get; set; }

    /// <summary>
    /// The quest the shortcut belongs to, by quest id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint QuestId { get; set; }

    /// <summary>
    /// When clear, the client adds the shortcut; when set, it removes it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Add { get; set; }
}