using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Shows a floater with a line of text.
/// </summary>
/// <remarks>
/// The client raises GenericFloater.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0723, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GenericFloaterText
{
    /// <summary>
    /// The text, a localized text id.
    /// </summary>
    [UInt(32)] public uint TextId { get; set; }
}
