using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 3, which the client gives no meaning.
/// </summary>
[WireStruct]
[UnionCase(3)]
public sealed partial class ChatLinkFlag3 : ChatLinkFlag
{
}
