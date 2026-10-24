using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Sets the player's costumes.
/// </summary>
/// <remarks>
/// The client files each by its type and index and raises no event.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00D9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CostumeList
{
    /// <summary>
    /// The costumes.
    /// </summary>
    [Array(CountBits = 32)] public List<CostumeEntry> Costumes { get; set; } = [];
}
