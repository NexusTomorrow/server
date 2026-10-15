using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatJoinResult)]
[WireMessage(GameMessageOpcode.ServerChatJoinResult, Direction.ServerToClient)]
public sealed partial class ServerChatJoinResult : IWritable
{
    [UInt(14)] public ChatChannelType ChatChannelId { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(5)] public ChatResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ChatChannelId, 14u);
        writer.WriteStringWide(Name);
        writer.Write(Result, 5u);
    }
}
