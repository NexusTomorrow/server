using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Turns decor edit mode on or off on the player's current residence: the request HousingLib.SetEditMode sends.
/// </summary>
/// <remarks>
/// The client sends it only when the mode changes.
/// </remarks>
[WireMessage(GameMessageOpcode.Client053C, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingSetEditMode
{
    /// <summary>
    /// The residence the player is on.
    /// </summary>
    public RealmScopedId ResidenceId { get; } = new();

    /// <summary>
    /// True to turn edit mode on, false to turn it off.
    /// </summary>
    [UInt(1)] public bool EditMode { get; private set; }
}
