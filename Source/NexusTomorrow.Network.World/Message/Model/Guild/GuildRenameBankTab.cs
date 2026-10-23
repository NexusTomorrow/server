using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Renames a guild bank tab: the request GuildTypeLib's RenameBankTab sends, laid out as <see cref="GuildOperationRequest"/>.
/// </summary>
/// <remarks>
/// <see cref="GuildOperationRequest.Value"/> is the tab's inventory location, <see cref="GuildOperationRequest.Text"/> the new
/// name and <see cref="GuildOperationRequest.Operation"/> 22; the rank is left empty.
/// </remarks>
[WireMessage(GameMessageOpcode.Client04B3, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildRenameBankTab : GuildOperationRequest
{
}
