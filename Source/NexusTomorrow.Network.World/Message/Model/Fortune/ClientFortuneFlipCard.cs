using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[Message(GameMessageOpcode.ClientFortuneFlipCard)]
public class ClientFortuneFlipCard : IReadable
{
    public uint SelectedCardIndex { get; private set; }

    public void Read(GamePacketReader reader)
    {
        SelectedCardIndex = reader.ReadUInt();
    }
}
