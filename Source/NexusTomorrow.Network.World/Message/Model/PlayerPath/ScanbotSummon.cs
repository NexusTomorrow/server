using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Summons the scientist's scanbot with the profile chosen.
/// </summary>
/// <remarks>
/// The client sends it while the scanbot is not out (<see cref="ScanbotState"/>); while it is, it sends <see cref="ScanbotDismiss"/> or
/// <see cref="ScanbotDismissFromAction"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0180, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ScanbotSummon
{
    /// <summary>
    /// The scanbot profile, as PlayerPathLib.ScientistGetScanBotProfile returns it.
    /// </summary>
    [UInt(32)] public uint Profile { get; private set; }

    [UInt(1)] public bool Unknown1 { get; private set; }
}
