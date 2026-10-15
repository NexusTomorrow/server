using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.QuestId)]
public sealed partial class ChatFormatQuestId : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.QuestId;
    [UInt(15)] public ushort Quest2Id { get; set; }
}
