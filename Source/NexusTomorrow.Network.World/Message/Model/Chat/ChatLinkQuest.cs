using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 5: a quest.
/// </summary>
[WireStruct]
[UnionCase(5)]
public sealed partial class ChatLinkQuest : IChatLinkPayload
{
    /// <summary>
    /// The quest, a Quest2 table id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; set; }
}
