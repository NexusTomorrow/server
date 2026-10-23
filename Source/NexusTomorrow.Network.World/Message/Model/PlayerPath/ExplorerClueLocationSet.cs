using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Sets a scavenger hunt clue's location.
/// </summary>
/// <remarks>
/// PathMission.GetExplorerClueRatio uses the location for a clue of type 0 or 2, and the clue's units (<see cref="ExplorerClueUnitsAdded"/>)
/// for type 1.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06B8, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerClueLocationSet
{
    /// <summary>
    /// The location, a WorldLocation2 table id.
    /// </summary>
    [UInt(17)] public uint WorldLocationId { get; set; }

    /// <summary>
    /// The clue, a PathExplorerScavengerClue table id.
    /// </summary>
    [UInt(14)] public ushort ClueId { get; set; }
}
