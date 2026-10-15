using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ClientGuildInviteResponse)]
public class ClientGuildInviteResponse : IReadable
{
    public bool Accepted { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Accepted = reader.ReadBit();
    }
}
