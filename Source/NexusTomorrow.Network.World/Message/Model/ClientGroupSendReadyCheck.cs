using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupSendReadyCheck)]
public class ClientGroupSendReadyCheck : IReadable
{
    public ulong GroupId { get; set; }

    public string Message { get; set; }

    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        Message = reader.ReadWideString();
    }
}
