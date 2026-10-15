using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[Message(GameMessageOpcode.ClientRequestInputKeySet)]
public class ClientRequestInputKeySet : IReadable
{
    public ulong CharacterId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        CharacterId = reader.ReadULong();
    }
}
