using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 2: the client marks its range as role play.
/// </summary>
[WireStruct]
[UnionCase(2)]
public sealed partial class ChatLinkRolePlay : ChatLinkFlag
{
}
