using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Reports the scientist's scanbot out or back, with its cooldown.
/// </summary>
/// <remarks>
/// The client takes it only on the Scientist path. A <see cref="ScanbotUnitId"/> other than 0 raises PlayerPathScientistScanBotDeployed;
/// 0 raises the scanbot's despawn and cooldown events. While <see cref="ScanbotUnitId"/> is not 0 the client sends
/// <see cref="ScanbotDismiss"/> or <see cref="ScanbotDismissFromAction"/> where it would send <see cref="ScanbotSummon"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0669, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ScanbotState
{
    /// <summary>
    /// The scanbot's unit, by unit id, which PlayerPathLib.ScientistGetScanBotUnit returns; 0 when the scanbot is not out.
    /// </summary>
    [UInt(32)] public uint ScanbotUnitId { get; set; }

    /// <summary>
    /// The scanbot's cooldown in milliseconds, which the client passes to its scanbot cooldown event in seconds.
    /// </summary>
    [UInt(32)] public uint CooldownMs { get; set; }
}
