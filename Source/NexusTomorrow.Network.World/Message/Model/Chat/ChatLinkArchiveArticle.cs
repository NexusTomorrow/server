using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 6: a Galactic Archive article.
/// </summary>
[WireStruct]
[UnionCase(6)]
public sealed partial class ChatLinkArchiveArticle : IChatLinkPayload
{
    /// <summary>
    /// The article, by id.
    /// </summary>
    [UInt(14)] public ushort ArticleId { get; set; }
}
