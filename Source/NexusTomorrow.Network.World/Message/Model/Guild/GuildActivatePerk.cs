using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Activates a guild perk: the request GuildTypeLib's ActivatePerk sends, laid out as <see cref="GuildOperationRequest"/>.
/// </summary>
/// <remarks>
/// <see cref="GuildOperationRequest.Value"/> is the perk's id and <see cref="GuildOperationRequest.Operation"/> is 26; the rank
/// and the text are left empty.
/// </remarks>
[WireMessage(GameMessageOpcode.Client04B2, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildActivatePerk : GuildOperationRequest
{
}
