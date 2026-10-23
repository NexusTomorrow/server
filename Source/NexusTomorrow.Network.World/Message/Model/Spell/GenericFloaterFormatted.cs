using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Shows a floater with formatted text.
/// </summary>
/// <remarks>
/// The client formats the text as it does a unit's speech, and raises GenericFloater.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0738, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GenericFloaterFormatted
{
    /// <summary>
    /// The text.
    /// </summary>
    public Server0738Part0 Text { get; set; } = new();
}
