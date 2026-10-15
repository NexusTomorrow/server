using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;


[Message(GameMessageOpcode.ServerChatZoneChange)]
[WireMessage(GameMessageOpcode.ServerChatZoneChange, Direction.ServerToClient)]
public sealed partial class ServerChatZoneChange : IWritable
{
    [UInt(15)] public ushort WorldZoneId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(WorldZoneId, 15u);
    }
}
