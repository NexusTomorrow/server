using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 1: the client marks its range as alien and scrambles it.
/// </summary>
[WireStruct]
[UnionCase(1)]
public sealed partial class ChatLinkAlien : ChatLinkScramble
{
}
