using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.ItemGuid)]
public sealed partial class ChatFormatItemGuid : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.ItemGuid;
    [UInt(64)] public ulong ItemGuid { get; set; }
}
