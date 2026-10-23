using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Clears a scavenger hunt clue: the client drops every unit (<see cref="ExplorerClueUnitsAdded"/>) and the location
/// (<see cref="ExplorerClueLocationSet"/>) it holds for the clue.
/// </summary>
[WireMessage(GameMessageOpcode.Server06B7, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerClueRemoved
{
    /// <summary>
    /// The clue, a PathExplorerScavengerClue table id.
    /// </summary>
    [UInt(14)] public ushort ClueId { get; set; }
}
