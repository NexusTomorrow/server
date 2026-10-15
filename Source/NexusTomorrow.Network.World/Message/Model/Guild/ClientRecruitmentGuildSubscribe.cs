using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ClientRecruitmentGuildSubscribe)]
[WireMessage(GameMessageOpcode.ClientRecruitmentGuildSubscribe, Direction.ClientToServer)]
public sealed partial class ClientRecruitmentGuildSubscribe : IReadable
{
    [UInt(1)] public bool Subscribe { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Subscribe = reader.ReadBit();
    }
}
