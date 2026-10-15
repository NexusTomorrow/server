using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[Message(GameMessageOpcode.ClientFriendshipSetNoteByIdentity)]
[WireMessage(GameMessageOpcode.ClientFriendshipSetNoteByIdentity, Direction.ClientToServer)]
public sealed partial class ClientFriendshipSetNoteByIdentity : IReadable
{
    public Identity PlayerIdentity { get; private set; } = new();
    [WideString] public string Note { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PlayerIdentity.Read(reader);
        Note = reader.ReadWideString();
    }
}
