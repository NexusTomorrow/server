using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Saves changes to a costume: the request the SaveCostumeChanges confirm action and Costume:SaveChanges send.
/// </summary>
/// <remarks>
/// The confirm action prices the change, sends only when the player can pay, and then raises CostumeSaveResult with
/// <see cref="CostumeSaveResult.Saving"/>. Costume:SaveChanges sends only a change that costs nothing.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0255, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class CostumeSave
{
    /// <summary>
    /// One costume slot's item and dyes.
    /// </summary>
    [WireStruct]
    public sealed partial class CostumeSaveSlot
    {
        /// <summary>
        /// The slot's item, by Item2 id.
        /// </summary>
        [UInt(18)] public uint ItemId { get; private set; }

        /// <summary>
        /// The slot's three dye channels, by DyeColorRamp id.
        /// </summary>
        [FixedArray(3), UInt(32)] public uint[] DyeIds { get; } = new uint[3];
    }

    /// <summary>
    /// The costume's index.
    /// </summary>
    [SInt(32)] public int Index { get; private set; }

    /// <summary>
    /// Whether the costume is the character's or a mannequin's.
    /// </summary>
    [UInt(2)] public CostumeType Type { get; private set; }

    [UInt(64)] public ulong Unknown2 { get; private set; }

    /// <summary>
    /// Each slot's item and dyes, indexed by <see cref="CostumeItemSlot"/>.
    /// </summary>
    [FixedArray(7)] public CostumeSaveSlot[] Slots { get; } = [.. Enumerable.Range(0, 7).Select(_ => new CostumeSaveSlot())];

    /// <summary>
    /// Which slots show, bit i for <see cref="CostumeItemSlot"/> i.
    /// </summary>
    [UInt(32)] public uint VisibleSlots { get; private set; }

    /// <summary>
    /// True to pay with service tokens at the flat price, false to pay credits for each changed dye channel.
    /// </summary>
    [UInt(1)] public bool UseServiceTokens { get; private set; }
}
