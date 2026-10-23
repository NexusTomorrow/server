using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 4: an item by its type.
/// </summary>
[WireStruct]
[UnionCase(4)]
public sealed partial class ChatLinkItemType : IChatLinkPayload
{
    /// <summary>
    /// The item, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }
}
