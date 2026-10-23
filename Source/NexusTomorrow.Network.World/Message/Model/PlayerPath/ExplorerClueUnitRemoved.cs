using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Takes a unit off a scavenger hunt clue: the client erases the clue and unit pair <see cref="ExplorerClueUnitsAdded"/> adds.
/// </summary>
[WireMessage(GameMessageOpcode.Server06B6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerClueUnitRemoved
{
    /// <summary>
    /// The clue, a PathExplorerScavengerClue table id.
    /// </summary>
    [UInt(14)] public ushort ClueId { get; set; }

    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
