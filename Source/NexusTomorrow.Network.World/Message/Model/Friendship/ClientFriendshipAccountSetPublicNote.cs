using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountSetPublicNote)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountSetPublicNote, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountSetPublicNote : IReadable
{
    [WideString] public string PublicNote { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PublicNote = reader.ReadWideString();
    }
}
