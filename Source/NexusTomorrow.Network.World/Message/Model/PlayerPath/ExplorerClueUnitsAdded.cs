using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Adds units to a scavenger hunt clue.
/// </summary>
/// <remarks>
/// For a clue of type 1, PathMission:GetExplorerClueRatio averages the positions of the clue's units.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06B9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerClueUnitsAdded
{
    /// <summary>
    /// The clue, a PathExplorerScavengerClue table id.
    /// </summary>
    [UInt(14)] public ushort ClueId { get; set; }

    /// <summary>
    /// The units, by unit id.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public uint[] UnitIds { get; set; } = [];
}
