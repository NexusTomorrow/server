using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[Message(GameMessageOpcode.ClientFortuneFlipCard)]
[WireMessage(GameMessageOpcode.ClientFortuneFlipCard, Direction.ClientToServer)]
public sealed partial class ClientFortuneFlipCard : IReadable
{
    [UInt(32)] public uint SelectedCardIndex { get; private set; }

    public void Read(GamePacketReader reader)
    {
        SelectedCardIndex = reader.ReadUInt();
    }
}
