using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Sets one of the player's costumes.
/// </summary>
/// <remarks>
/// The client files it by its type and index and raises no event.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00D8, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CostumeUpdate
{
    /// <summary>
    /// The costume.
    /// </summary>
    public CostumeEntry Costume { get; set; } = new();
}
