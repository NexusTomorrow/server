using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Reports the result of unlocking or forgetting a costume item.
/// </summary>
/// <remarks>
/// <see cref="CostumeUnlockResult.UnlockSuccess"/> adds the item to its slot's unlocked items and
/// <see cref="CostumeUnlockResult.ForgetItemSuccess"/> removes it. The client raises CostumeForgetResult for ForgetItemSuccess,
/// ForgetItemFailed, ItemInUse and ItemNotKnown, and CostumeUnlockResult for UnlockSuccess, AlreadyKnown, OutOfSpace,
/// UnknownFailure, FailedPrerequisites and InsufficientCredits.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0259, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CostumeItemResult
{
    /// <summary>
    /// The item, by Item2 id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }

    /// <summary>
    /// The result.
    /// </summary>
    [UInt(32)] public CostumeUnlockResult Result { get; set; }
}
