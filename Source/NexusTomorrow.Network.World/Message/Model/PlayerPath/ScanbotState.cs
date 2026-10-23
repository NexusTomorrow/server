using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Reports the scientist's scanbot out or back, with its cooldown.
/// </summary>
/// <remarks>
/// The client takes it only on the Scientist path. A <see cref="Unknown0"/> other than 0 raises PlayerPathScientistScanBotDeployed; 0
/// raises the scanbot's despawn event and its cooldown event, with the cooldown in seconds. While <see cref="Unknown0"/> is not 0 the
/// client sends <see cref="ScanbotDismiss"/> or <see cref="ScanbotDismissFromAction"/> where it would send <see cref="ScanbotSummon"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0669, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ScanbotState
{
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The scanbot's cooldown in milliseconds; when it is not 0 the client keeps the time it ends.
    /// </summary>
    [UInt(32)] public uint CooldownMs { get; set; }
}
