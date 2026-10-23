using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 7: the client marks its range as profanity and masks it.
/// </summary>
[WireStruct]
[UnionCase(7)]
public sealed partial class ChatLinkProfanity : ChatLinkScramble
{
}
