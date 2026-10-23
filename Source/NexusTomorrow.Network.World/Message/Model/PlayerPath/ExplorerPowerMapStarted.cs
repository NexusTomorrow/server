using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Starts an explorer power map mission, with the unit to follow.
/// </summary>
/// <remarks>
/// The client keeps the unit on the mission and raises PlayerPathExplorerPowerMapStarted with the mission and the unit.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00F5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerPowerMapStarted
{
    /// <summary>
    /// The power map, a PathExplorerPowerMap table id: the client finds the explorer mission whose object it is.
    /// </summary>
    [UInt(14)] public ushort PowerMapId { get; set; }

    /// <summary>
    /// The unit to follow, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }
}
