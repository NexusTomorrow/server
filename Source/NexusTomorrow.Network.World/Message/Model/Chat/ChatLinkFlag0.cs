using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 0, which the client gives no meaning.
/// </summary>
[WireStruct]
[UnionCase(0)]
public sealed partial class ChatLinkFlag0 : ChatLinkFlag
{
}
