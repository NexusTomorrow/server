using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipAccountAddByEmail)]
[WireMessage(GameMessageOpcode.ClientFriendshipAccountAddByEmail, Direction.ClientToServer)]
public sealed partial class ClientFriendshipAccountAddByEmail : IReadable
{
    [WideString] public string Email { get; private set; }
    [WideString] public string Note { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Email = reader.ReadWideString();
        Note = reader.ReadWideString();
    }
}
