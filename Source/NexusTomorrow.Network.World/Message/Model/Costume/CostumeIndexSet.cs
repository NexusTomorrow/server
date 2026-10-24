using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Wears one of the character's costumes, or none: the request CostumesLib.SetCostumeIndex sends.
/// </summary>
/// <remarks>
/// The client sends it only when the index changes, is in range and below the number of costumes unlocked, the player is
/// out of combat, and the 15-second cooldown has passed; it then starts the cooldown.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0256, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CostumeIndexSet
{
    /// <summary>
    /// The costume's index, or -1 to wear none: the call's argument less one.
    /// </summary>
    [SInt(32)] public int Index { get; private set; }
}
