using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatLeave)]
public class ServerChatLeave : IWritable
{
    public Channel Channel { get; set; }
    public ChatChannelLeaveReason Leave { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);
        writer.Write(Leave, 2u);
    }
}
