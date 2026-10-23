using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Dismisses the scientist's scanbot from the path action.
/// </summary>
/// <remarks>
/// PlayerPathLib.PathAction sends it in place of <see cref="ScanbotSummon"/> while the scanbot is out (<see cref="ScanbotState"/>).
/// </remarks>
[WireMessage(GameMessageOpcode.Client015F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ScanbotDismissFromAction
{
}
