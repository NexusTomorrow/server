using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Shows a floater that names an item in a fixed sentence.
/// </summary>
/// <remarks>
/// The client raises GenericFloater.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00B0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ItemCreatedFloater
{
    /// <summary>
    /// The item, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }
}
