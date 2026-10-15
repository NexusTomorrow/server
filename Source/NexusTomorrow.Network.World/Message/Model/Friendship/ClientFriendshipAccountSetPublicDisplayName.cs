using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountSetPublicDisplayName)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountSetPublicDisplayName, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountSetPublicDisplayName : IReadable
{
    [WideString] public string PublicDisplayName { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PublicDisplayName = reader.ReadWideString();
    }
}
