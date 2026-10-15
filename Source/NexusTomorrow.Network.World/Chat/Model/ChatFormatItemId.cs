using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.ItemId)]
public sealed partial class ChatFormatItemId : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.ItemId;
    [UInt(18)] public uint Item2Id { get; set; }

    public void Read(GamePacketReader reader)
    {
        Item2Id = reader.ReadUInt(18u);
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Item2Id, 18u);
    }
}
