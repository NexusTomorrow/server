using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipSetNoteByName)]
[WireMessage(GameMessageOpcode.ClientFriendshipSetNoteByName, Direction.ClientToServer)]
public sealed partial class ClientFriendshipSetNoteByName : IReadable
{
    [WideString] public string Name { get; private set; }
    [WideString] public string RealmName { get; private set; }
    [WideString] public string Note { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Name = reader.ReadWideString();
        RealmName = reader.ReadWideString();
        Note = reader.ReadWideString();
    }
}
