using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Asks to give a pending item group to a friend's character.
/// </summary>
[WireMessage(GameMessageOpcode.Client03F2, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class AccountPendingItemGroupGiftToCharacter
{
    /// <summary>
    /// The group, by <see cref="AccountPendingItem.GroupName"/>.
    /// </summary>
    [WideString] public string GroupName { get; private set; } = "";

    /// <summary>
    /// The friend's character, by realm-scoped character id.
    /// </summary>
    public RealmScopedId RecipientId { get; private set; } = new();
}
