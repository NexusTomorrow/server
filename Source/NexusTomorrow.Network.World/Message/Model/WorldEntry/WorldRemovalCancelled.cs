using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// Calls off the removal from the world that <see cref="WorldRemovalWarning"/> announced. The client stops the warning's countdown and
/// raises PendingWorldRemovalCancel, on which its interface closes the warning.
/// </summary>
/// <remarks>
/// The client handles it exactly as it handles <see cref="WorldRemovalWarningCleared"/>; the two names keep apart what the server means by
/// each.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0688, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class WorldRemovalCancelled
{
}
