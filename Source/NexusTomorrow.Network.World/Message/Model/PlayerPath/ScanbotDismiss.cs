using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Dismisses the scientist's scanbot.
/// </summary>
/// <remarks>
/// The scanbot toggle (PlayerPathLib.ScientistToggleScanBot among its callers) sends it in place of <see cref="ScanbotSummon"/> while the
/// scanbot is out (<see cref="ScanbotState"/>).
/// </remarks>
[WireMessage(GameMessageOpcode.Client00F0, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ScanbotDismiss
{
}
