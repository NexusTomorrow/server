using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientTitleSet)]
[WireMessage(GameMessageOpcode.ClientTitleSet, Direction.ClientToServer)]
public sealed partial class ClientTitleSet : IReadable
{
    [UInt(14)] public ushort TitleId { get; set; }

    public void Read(GamePacketReader reader)
    {
        TitleId = reader.ReadUShort(14u);
    }
}
