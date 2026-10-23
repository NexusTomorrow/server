using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Puts an explorer power map mission into its wait.
/// </summary>
/// <remarks>
/// The client raises PlayerPathExplorerPowerMapWaiting with a delay it takes from the tables (the PathExplorerPowerMap row's
/// victoryPauseMS, or a GameFormula value), not from the message.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00FA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerPowerMapWaiting
{
    /// <summary>
    /// The power map, a PathExplorerPowerMap table id: the client finds the explorer mission whose object it is.
    /// </summary>
    [UInt(14)] public ushort PowerMapId { get; set; }
}
