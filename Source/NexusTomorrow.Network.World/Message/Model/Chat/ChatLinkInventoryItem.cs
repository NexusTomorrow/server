using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 9: an item in the sender's inventory.
/// </summary>
[WireStruct]
[UnionCase(9)]
public sealed partial class ChatLinkInventoryItem : IChatLinkPayload
{
    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; set; }
}
