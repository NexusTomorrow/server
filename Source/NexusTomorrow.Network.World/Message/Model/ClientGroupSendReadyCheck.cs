using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupSendReadyCheck)]
[WireMessage(GameMessageOpcode.ClientGroupSendReadyCheck, Direction.ClientToServer)]
public sealed partial class ClientGroupSendReadyCheck : IReadable
{
    [UInt(64)] public ulong GroupId { get; set; }

    [WideString] public string Message { get; set; }

    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        Message = reader.ReadWideString();
    }
}
