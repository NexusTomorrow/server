using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireStruct]
public sealed partial class Channel : IReadable, IWritable
{
    [UInt(14)] public ChatChannelType ChatChannelId { get; set; }
    [UInt(64)] public ulong ChatId { get; set; }

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
