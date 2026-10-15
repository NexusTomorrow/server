using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatResult)]
[WireMessage(GameMessageOpcode.ServerChatResult, Direction.ServerToClient)]
public sealed partial class ServerChatResult : IWritable
{
    public Channel Channel { get; set; }
    [UInt(5)] public ChatResult ChatResult { get; set; }
    [UInt(16)] public ushort ChatMessageId { get; set; } // removes the message from the chat log with this id

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);
        writer.Write(ChatResult, 5u);
        writer.Write(ChatMessageId);
    }
}
