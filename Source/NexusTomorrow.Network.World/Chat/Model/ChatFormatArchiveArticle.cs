using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.ArchiveArticle)]
public sealed partial class ChatFormatArchiveArticle : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.ArchiveArticle;
    [UInt(14)] public ushort ArchiveArticleId { get; set; }

    public void Read(GamePacketReader reader)
    {
        ArchiveArticleId = reader.ReadUShort(14u);
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ArchiveArticleId, 14u);
    }
}
