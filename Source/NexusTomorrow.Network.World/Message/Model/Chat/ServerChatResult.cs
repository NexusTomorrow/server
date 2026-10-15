using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChatResult, Direction.ServerToClient)]
public sealed partial class ServerChatResult
{
    public Channel Channel { get; set; }
    [UInt(5)] public ChatResult ChatResult { get; set; }
    [UInt(16)] public ushort ChatMessageId { get; set; } // removes the message from the chat log with this id
}
