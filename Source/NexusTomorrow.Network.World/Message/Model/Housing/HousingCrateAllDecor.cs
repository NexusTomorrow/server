using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Puts all decor of the player's current residence into the crate: the request HousingLib.CrateAllDecor sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client04EC, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingCrateAllDecor
{
    /// <summary>
    /// The residence the player is on.
    /// </summary>
    public RealmScopedId ResidenceId { get; } = new();
}
