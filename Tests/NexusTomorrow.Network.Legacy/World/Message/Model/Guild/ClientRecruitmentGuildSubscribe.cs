using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ClientRecruitmentGuildSubscribe)]
public class ClientRecruitmentGuildSubscribe : IReadable
{
    public bool Subscribe { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Subscribe = reader.ReadBit();
    }
}
