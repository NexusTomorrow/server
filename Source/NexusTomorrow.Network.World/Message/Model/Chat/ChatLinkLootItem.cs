using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 11: an item up for loot.
/// </summary>
[WireStruct]
[UnionCase(11)]
public sealed partial class ChatLinkLootItem : IChatLinkPayload
{
    /// <summary>
    /// The loot entry, by id.
    /// </summary>
    [UInt(32)] public uint LootId { get; set; }
}
