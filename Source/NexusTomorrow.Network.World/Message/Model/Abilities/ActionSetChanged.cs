using NexusTomorrow.Game.Static.Abilities;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

/// <summary>
/// Sets an action set's slots and flags, with the result of the last <see cref="ActionSetChangeRequest"/>.
/// </summary>
/// <remarks>
/// The client raises ActionSetError for any <see cref="Result"/> but 1.
/// </remarks>
[WireMessage(GameMessageOpcode.Server019D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ActionSetChanged
{
    /// <summary>
    /// One action set slot's shortcut.
    /// </summary>
    [WireStruct]
    public sealed partial class ActionSetShortcut
    {
        /// <summary>
        /// The shortcut's kind. 0 leaves the slot empty, and the client refuses 5, 8 and 9, which empty it too; each other kind looks
        /// <see cref="ObjectId"/> up in its own store. With kind 6 and the ability in the book, the slot holds that spell.
        /// </summary>
        [VerifiedMember, UInt(4)] public ActionSetShortcutType ShortcutType { get; set; }

        /// <summary>
        /// The client gives this value no meaning. The shortcut builder reads only <see cref="Slot"/>.
        /// </summary>
        [UInt(9)] public ushort Unknown1 { get; set; }

        /// <summary>
        /// The action bar slot, 0 to 47. A slot of 48 or more stops the client's handler there: the shortcuts before it have landed, and
        /// the flags, result and event do not follow. ActionSetLib.GetCurrentActionSet reports slots 0 to 11, showing a class ability
        /// (Spell4 type 5) only in slots 0 to 7 and any other only in 8 to 11.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Slot { get; set; }

        /// <summary>
        /// What the shortcut points at, by kind. For kind 6, a spell the client already holds (the player's, after
        /// <see cref="AbilityBookChanged"/>), whose base id ActionSetLib.GetCurrentActionSet reports; other kinds' ids go to their own
        /// lookups.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint ObjectId { get; set; }
    }

    /// <summary>
    /// Which action set the slots and flags are for, counted from 0; 4 or more refuses the whole message.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte ActionSetIndex { get; set; }

    /// <summary>
    /// The action set's flags. The client never tests 0x02.
    /// </summary>
    [VerifiedMember, UInt(2)] public ActionSetFlags Flags { get; set; }

    /// <summary>
    /// The result of the last action set request, an ActionSetLib.CodeEnumLimitedActionSetResult value: Ok is 1.
    /// </summary>
    [VerifiedMember, UInt(6)] public byte Result { get; set; }

    /// <summary>
    /// The slots to set, one shortcut each.
    /// </summary>
    [VerifiedMember, Array(CountBits = 6)] public List<ActionSetShortcut> Shortcuts { get; set; } = [];
}
