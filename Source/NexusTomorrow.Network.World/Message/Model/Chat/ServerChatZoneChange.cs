using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;


[WireMessage(GameMessageOpcode.ServerChatZoneChange, Direction.ServerToClient)]
public sealed partial class ServerChatZoneChange
{
    [UInt(15)] public ushort WorldZoneId { get; set; }
}
