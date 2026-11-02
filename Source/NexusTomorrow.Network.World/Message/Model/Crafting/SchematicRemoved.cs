using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

/// <summary>
/// Removes a learned schematic from a tradeskill, which the client reports through its tradeskill interface.
/// </summary>
[WireMessage(GameMessageOpcode.Server085E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SchematicRemoved
{
    /// <summary>
    /// The tradeskill whose learned-schematic list loses the schematic.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Profession { get; set; }

    /// <summary>
    /// The schematic, a TradeskillSchematic2 table id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint SchematicId { get; set; }
}