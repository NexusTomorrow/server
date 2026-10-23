using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 8: an item, described in full.
/// </summary>
[WireStruct]
[UnionCase(8)]
public sealed partial class ChatLinkItem : IChatLinkPayload
{
    /// <summary>
    /// The item.
    /// </summary>
    public ChatItemRecord Item { get; set; } = new();
}
