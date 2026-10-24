using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Asks to give a pending item group to an account friend.
/// </summary>
[WireMessage(GameMessageOpcode.Client03F1, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class AccountPendingItemGroupGiftToAccount
{
    /// <summary>
    /// The group, by <see cref="AccountPendingItem.GroupName"/>.
    /// </summary>
    [WideString] public string GroupName { get; private set; } = "";

    /// <summary>
    /// The account friend, by account id.
    /// </summary>
    [UInt(64)] public ulong RecipientAccountId { get; private set; }

    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    [UInt(32)] public uint Unknown2 { get; private set; }

    /// <summary>
    /// The player's own character, by realm-scoped character id.
    /// </summary>
    public RealmScopedId SenderId { get; private set; } = new();
}
