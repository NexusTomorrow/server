using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

public class Channel : IReadable, IWritable
{
    public ChatChannelType ChatChannelId { get; set; }
    public ulong ChatId { get; set; }

    public void Read(GamePacketReader reader)
    {
        ChatChannelId = reader.ReadEnum<ChatChannelType>(14u);
        ChatId = reader.ReadULong();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ChatChannelId, 14u);
        writer.Write(ChatId);
    }
}
