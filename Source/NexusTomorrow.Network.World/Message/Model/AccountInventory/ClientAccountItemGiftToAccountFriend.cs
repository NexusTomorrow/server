using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.ClientAccountItemGiftToAccountFriend, Direction.ClientToServer)]
public sealed partial class ClientAccountItemGiftToAccountFriend
{
    [WideString] public string TransactionId { get; private set; }
    [UInt(64)] public ulong RecipientAccountFriendId { get; private set; }
    [UInt(32)] public uint Unused { get; private set; }
    public Identity Gifter { get; private set; } = new(); // always the sending player's identity, redundant as this comes from the player's session
}
