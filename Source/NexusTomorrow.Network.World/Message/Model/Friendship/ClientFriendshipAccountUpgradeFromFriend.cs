using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

// Takes an existing friend and uses the identity of that character to
// send an account friend invite.
[Message(GameMessageOpcode.ClientFriendshipAccountUpgradeFromFriend)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountUpgradeFromFriend, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountUpgradeFromFriend : IReadable
{
    public Identity ExisitingFriend { get; private set; } = new();
    [WideString] public string Note { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ExisitingFriend.Read(reader);
        Note = reader.ReadWideString();
    }
}
