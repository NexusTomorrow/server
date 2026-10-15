using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.Loot)]
public sealed partial class ChatFormatLoot : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.Loot;
    [UInt(32)] public uint LootUnitId { get; set; } // must match loot sent in ServerLootNotification (0x8A6)
}
